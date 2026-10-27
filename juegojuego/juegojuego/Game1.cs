using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace juegojuego
{
    // Pantallas posibles del juego, en el orden en que se recorren:
    // Inicio (boton JUGAR) -> SeleccionModo (elegir el modo de juego) -> ElegirNivel -> Jugando -> Gano o Perdio
    public enum EstadoJuego { Inicio, SeleccionModo, ElegirNivel, Jugando, Gano, Perdio }

    // Clase principal del juego hereda de Game de MonoGame
    // Maneja los menus la partida y el dibujo.
    // Hay dos cosas que se llaman modo y se agregan sin tocar esta clase:
    //   Modo (tematica): imagenes y musica de las cartas -> modelos/CatalogoModos.cs
    //   ModoJuego (reglas): Nivel Cronometro, Clasico, Infinito, multijugador, etc. -> modosdejuego/CatalogoModosJuego.cs
    public class Game1 : Game
    {
        // Ancho de la ventana en pixeles
        int Ancho = 1900;
        // Alto de la ventana en pixeles
        int Alto = 1050;
        // Segundos que se ven las 2 cartas cuando fallas antes de taparse
        float EsperaFallo = 0.8f;

        // Botones de nivel del menu: tamano separacion y posicion de arriba
        int BotonAncho = 440, BotonAlto = 70, BotonPaso = 90, BotonSeparacion = 20, BotonesY = 260;
        // Cuantos botones entran en una columna del menu: si un modo tiene mas niveles se abre otra columna
        int BotonesPorColumna = 6;

        // Botones de la lista de modos de juego: ancho, separacion entre botones y posicion de arriba
        // (el alto se achica solo si hay muchos modos para que todos entren en pantalla)
        int ModoAncho = 1000, ModoPasoMaximo = 104, ModoSeparacion = 20, ModosY = 250;

        // Maneja la ventana y la placa de video
        GraphicsDeviceManager _graphics;
        // Herramienta para dibujar imagenes en pantalla
        SpriteBatch _spriteBatch;
        // Textura blanca de 1x1 para dibujar rectangulos de colores
        Texture2D _pixel;
        // Carga las imagenes y la musica del disco
        Recursos _recursos;

        // Generador de numeros al azar
        Random _rng = new Random();
        // Tabla con los mejores puntajes
        TablaRecords _records = new TablaRecords();
        // Pantalla en la que esta el juego ahora arranca en la pantalla de inicio
        EstadoJuego _estado = EstadoJuego.Inicio;
        // Partida en curso null hasta que empieza una
        Partida _partida;
        // Nivel que se esta jugando
        Nivel _nivel;
        // Nivel con el que empezo el juego: R vuelve a este (en un modo con rondas _nivel va cambiando)
        Nivel _nivelInicial;
        // Puntaje de la ultima partida ganada
        Puntaje _ultimoPuntaje;
        // Segundos que faltan para tapar las cartas despues de un fallo
        float _espera;
        // Tamano en pixeles de cada carta se calcula segun el tablero
        float _cartaAncho, _cartaAlto;
        // Proporcion ancho/alto de cada carta: sale de las imagenes del modo (300 x 392 en el juego original)
        float _ratioCarta;

        // Todas las tematicas del juego (se agregan en CatalogoModos)
        Modo[] _modos = CatalogoModos.Todos;
        // Cual de las tematicas esta elegida en el menu de niveles
        int _indiceModo;
        // La tematica elegida: de ahi salen las imagenes y la musica
        Modo ModoActual => _modos[_indiceModo];

        // Todos los modos de juego (se agregan en CatalogoModosJuego)
        ModoJuego[] _modosJuego = CatalogoModosJuego.Todos;
        // El modo de juego elegido en la pantalla de modos: de ahi salen los niveles y las reglas
        // (empieza en el primero que es Nivel Cronometro y se cambia al elegir otro)
        ModoJuego _modoJuego = CatalogoModosJuego.Todos[0];
        // Niveles del modo de juego elegido que se pueden jugar con la tematica actual (los botones del menu de niveles)
        Nivel[] _niveles = new Nivel[0];

        // La musica que esta sonando ahora
        SoundEffectInstance _musicaActual;

        // Estado del teclado en el cuadro anterior sirve para detectar 'recien apretada'
        KeyboardState _tecPrev;
        // Estado del mouse en el cuadro anterior
        MouseState _mouPrev;

        // Constructor
        // Recibe: nada
        // Devuelve: nada crea el juego Configura el tamano de la ventana la carpeta de contenido el mouse visible y el titulo
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Ancho,
                PreferredBackBufferHeight = Alto
            };
            Content.RootDirectory = "Content";
            // Muestra el cursor del mouse
            IsMouseVisible = true;
            Window.Title = "Juego de Memoria";
        }

        // Recibe: nada Devuelve: nada
        // Se ejecuta una sola vez al arrancar: crea el SpriteBatch carga el primer modo y los records
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Crea una textura de 1 pixel blanco sirve para dibujar rectangulos
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Carga las imagenes y la musica del primer modo (los demas se cargan la primera vez que se eligen)
            _recursos = new Recursos(GraphicsDevice);
            ModoActual.Cargar(_recursos);

            // Carga los records guardados en el archivo
            try { _records.CargarDesde(GestorArchivos.CargarRecordsBinario()); }
            catch (Exception) { /* archivo de records daniado: se empieza vacio */ }

            ActualizarMusica(); // arranca con la musica del menu
        }

        // Reproduce una pista desde el principio, frenando la que estaba sonando.
        // Recibe: musica la pista que debe sonar puede ser null Devuelve: nada
        void Reproducir(SoundEffectInstance musica)
        {
            try
            {
                if (_musicaActual != null) _musicaActual.Stop();
                _musicaActual = musica;
                if (_musicaActual != null) _musicaActual.Play();
            }
            catch (Exception) { /* sin audio disponible */ }
        }

        // Pantallas de menu (inicio, modos y niveles) -> musica del menu de la tematica.
        // Cualquier otro estado (partida, ganar, perder) -> musica de la partida de la tematica.
        // Recibe: nada Devuelve: nada
        // Elige la musica segun el estado y solo la cambia si es distinta de la que ya suena
        void ActualizarMusica()
        {
            bool enMenu = _estado == EstadoJuego.Inicio || _estado == EstadoJuego.SeleccionModo || _estado == EstadoJuego.ElegirNivel;
            SoundEffectInstance deseada = enMenu ? ModoActual.MusicaMenu : ModoActual.MusicaPartida;
            if (deseada != _musicaActual) Reproducir(deseada);
        }

        // Recibe: paso 1 para ir a la tematica siguiente -1 para ir a la anterior Devuelve: nada
        // Cambia la tematica elegida en el menu de niveles (de la ultima se vuelve a la primera) y carga su contenido si es la primera vez
        // Como cada tematica tiene distinta cantidad de caras, tambien recalcula que niveles se pueden jugar
        void CambiarModo(int paso)
        {
            _indiceModo = (_indiceModo + paso + _modos.Length) % _modos.Length;
            ModoActual.Cargar(_recursos);
            _niveles = _modoJuego.NivelesPara(ModoActual);
        }

        // Recibe: modo el ModoJuego que se eligio en la pantalla de modos Devuelve: nada
        // Si el modo esta disponible lo deja elegido y pasa a la pantalla de niveles; si todavia no esta hecho no hace nada
        void ElegirModoJuego(ModoJuego modo)
        {
            if (!modo.Disponible) return;
            _modoJuego = modo;
            // Los niveles que se ofrecen dependen del modo de juego y de las caras que tenga la tematica
            _niveles = modo.NivelesPara(ModoActual);
            _estado = EstadoJuego.ElegirNivel;
        }

        // Recibe: nivel el Nivel con el que se empieza Devuelve: nada
        // Empieza un juego nuevo desde cero: avisa al modo de juego (para que ponga en 0 lo suyo) y arma la primera partida
        void ComenzarJuego(Nivel nivel)
        {
            _nivelInicial = nivel;
            _modoJuego.AlEmpezarJuego();
            IniciarPartida(nivel);
        }

        // Recibe: nivel el Nivel elegido Devuelve: nada
        // Arma la partida: elige caras al azar crea la Partida acomoda las cartas y pasa al estado Jugando
        void IniciarPartida(Nivel nivel)
        {
            _nivel = nivel;
            Modo modo = ModoActual;

            // Elige al azar las caras de esta partida (una por cada par) y un dorso para todas las cartas
            List<Texture2D> caras = ElegirCaras(nivel.Pares);
            Texture2D dorso = modo.Dorsos[_rng.Next(modo.Dorsos.Count)];
            // La proporcion de las cartas sale de la imagen de las caras
            _ratioCarta = (float)caras[0].Width / caras[0].Height;

            // Crea la partida La funcion 'valor => ' arma los sprites de dorso y cara para cada valor
            // El modo de juego dice cuantos jugadores hay
            _partida = new Partida("JUGADOR", nivel, valor =>
                (new Sprite(dorso, Vector2.Zero), new Sprite(caras[valor], Vector2.Zero)), _modoJuego.CantidadJugadores);

            // Calcula donde va cada carta en pantalla
            AcomodarCartas();
            // Reinicia la espera y el ultimo puntaje
            _espera = 0;
            _ultimoPuntaje = null;
            // Cambia a la pantalla de juego
            _estado = EstadoJuego.Jugando;
            Reproducir(modo.MusicaPartida); // la musica de la partida arranca de cero en cada partida
        }

        // Recibe: cantidad cuantas caras hacen falta
        // Devuelve: esa cantidad de caras del modo elegidas al azar sin repetir
        List<Texture2D> ElegirCaras(int cantidad)
        {
            // Copia de todas las caras del modo
            List<Texture2D> mezcladas = new List<Texture2D>(ModoActual.Caras);
            // Las mezcla algoritmo Fisher-Yates: cada una se intercambia con otra al azar
            for (int i = mezcladas.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (mezcladas[i], mezcladas[j]) = (mezcladas[j], mezcladas[i]);
            }
            // Se queda con las primeras que ya estan mezcladas
            return mezcladas.GetRange(0, cantidad);
        }

        // Recibe: nada Devuelve: nada
        // Calcula el tamano de las cartas para que entren en pantalla y le asigna a cada una su posicion x y
        void AcomodarCartas()
        {
            tablero t = _partida.Tablero;
            // gap: separacion entre cartas top: espacio arriba para el texto bajo: margen de abajo
            const float gap = 10f, top = 90f, bajo = 20f;
            // Espacio disponible para el tablero ancho y alto
            float dispW = Ancho - 40f, dispH = Alto - top - bajo;

            // Alto maximo de carta si se limita por la cantidad de filas
            float altoPorFila = (dispH - (t.filas - 1) * gap) / t.filas;
            // Alto maximo de carta si se limita por la cantidad de columnas
            float altoPorColumna = (dispW - (t.columnas - 1) * gap) / t.columnas / _ratioCarta;
            // Usa el mas chico para que entre en las dos direcciones
            _cartaAlto = Math.Min(altoPorFila, altoPorColumna);
            _cartaAncho = _cartaAlto * _ratioCarta;

            // Tamano total que ocupa el tablero
            float totalW = t.columnas * _cartaAncho + (t.columnas - 1) * gap;
            float totalH = t.filas * _cartaAlto + (t.filas - 1) * gap;
            // Esquina superior izquierda del tablero para que quede centrado
            float x0 = (Ancho - totalW) / 2f;
            float y0 = top + (dispH - totalH) / 2f;

            // Le asigna a cada carta su posicion segun su fila y columna
            foreach (carta c in t.ObtenerTodas())
            {
                Vector2 pos = new Vector2(x0 + c.columna * (_cartaAncho + gap), y0 + c.fila * (_cartaAlto + gap));
                c.cubierta.position = pos;
                c.descubierta.position = pos;
            }
        }

        // Recibe: c una carta
        // Devuelve: el rectangulo x y ancho alto que ocupa en pantalla Sirve para detectar clicks
        Rectangle RectDe(carta c)
        {
            return new Rectangle((int)c.cubierta.position.X, (int)c.cubierta.position.Y, (int)_cartaAncho, (int)_cartaAlto);
        }

        // Recibe: gano true si gano false si perdio (segun las reglas del modo de juego) Devuelve: nada
        // Si gano guarda el puntaje y los records (o pasa a la ronda siguiente si el modo la tiene); si perdio solo frena el cronometro
        void Terminar(bool gano)
        {
            if (gano)
            {
                // Si el modo tiene otra ronda (ej: infinito) arranca el nivel siguiente sin mostrar el cartel final
                // Si ese nivel necesita mas caras de las que tiene la tematica no se puede jugar y el juego termina aca
                Nivel siguiente = _modoJuego.SiguienteNivel(_nivel);
                if (siguiente != null && siguiente.Pares <= ModoActual.Caras.Count)
                {
                    IniciarPartida(siguiente);
                    return;
                }

                // Calcula el puntaje final
                _ultimoPuntaje = _partida.Finalizar();
                // El record aclara de que modo y nivel es ej: "UNDERTALE MUY FACIL"
                _ultimoPuntaje.Nivel = EtiquetaRecord();
                // Los modos que no guardan records (ej: multijugador) muestran el resultado pero no lo anotan en la tabla
                if (_modoJuego.GuardaRecords)
                {
                    // Lo mete en la tabla de records ya ordenado
                    _records.InsertarOrdenado(_ultimoPuntaje);
                    // Guarda los records en el archivo si falla se ignora
                    try { GestorArchivos.GuardarRecordsBinario(new List<Puntaje>(_records.Registros)); }
                    catch (Exception) { }
                }
                _estado = EstadoJuego.Gano;
            }
            else
            {
                _partida.Cronometro.Pausar();
                _estado = EstadoJuego.Perdio;
            }
        }

        // Recibe: nada
        // Devuelve: el texto que identifica la partida en la tabla de records ej: "CLASICO UNDERTALE MUY FACIL"
        // Solo aclara la tematica y el modo de juego cuando hay mas de uno disponible, para que con un solo modo se vea como siempre
        string EtiquetaRecord()
        {
            string etiqueta = _nivel.Nombre;
            if (_modos.Length > 1) etiqueta = $"{ModoActual.Nombre} {etiqueta}";
            if (CatalogoModosJuego.CantidadDisponibles > 1) etiqueta = $"{_modoJuego.Nombre} {etiqueta}";
            return etiqueta;
        }


        // Recibe: gameTime tiempo transcurrido Devuelve: nada
        // Se ejecuta en cada cuadro unas 60 veces por segundo: lee el teclado y el mouse y actua segun la pantalla actual
        protected override void Update(GameTime gameTime)
        {
            // Estado actual del teclado y del mouse
            KeyboardState tec = Keyboard.GetState();
            MouseState mou = Mouse.GetState();
            // Funcion local Recibe: k una tecla Devuelve: true solo en el cuadro en que se aprieta no mientras se mantiene
            bool Tecla(Keys k) => tec.IsKeyDown(k) && _tecPrev.IsKeyUp(k);
            // true solo en el cuadro en que se hace click izquierdo
            bool click = mou.LeftButton == ButtonState.Pressed && _mouPrev.LeftButton == ButtonState.Released;
            // Posicion del mouse
            Point mp = mou.Position;
            // Segundos que pasaron desde el cuadro anterior
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cada pantalla tiene sus propios controles
            switch (_estado)
            {
                // INICIO: ENTER o click en JUGAR pasa a elegir el modo; ESC o click en SALIR cierra el juego
                case EstadoJuego.Inicio:
                    ActualizarInicio(Tecla, click, mp);
                    break;

                // SELECCION DE MODO: 1 a 9 o click en un modo lo eligen; ESC vuelve al inicio
                case EstadoJuego.SeleccionModo:
                    ActualizarSeleccionModo(Tecla, click, mp);
                    break;

                // ELEGIR NIVEL: ESC vuelve a los modos; 1 a 9 o click en un boton inician un nivel; flechas o click en las flechas cambian de tematica
                case EstadoJuego.ElegirNivel:
                    ActualizarMenu(Tecla, click, mp);
                    break;

                // JUGANDO: ESC vuelve a elegir nivel; si no se actualiza la partida
                case EstadoJuego.Jugando:
                    if (Tecla(Keys.Escape)) { _estado = EstadoJuego.ElegirNivel; break; }
                    ActualizarJugando(click, mp, dt);
                    break;

                // GANO / PERDIO: ESC o ENTER vuelve a elegir nivel; R vuelve a empezar desde el primer nivel
                case EstadoJuego.Gano:
                case EstadoJuego.Perdio:
                    if (Tecla(Keys.Escape) || Tecla(Keys.Enter)) _estado = EstadoJuego.ElegirNivel;
                    else if (Tecla(Keys.R)) ComenzarJuego(_nivelInicial);
                    break;
            }

            // Cambia la musica si hizo falta
            ActualizarMusica();

            // Guarda el estado actual para compararlo en el proximo cuadro
            _tecPrev = tec;
            _mouPrev = mou;
            base.Update(gameTime);
        }

        // Recibe: tecla funcion que dice si una tecla se acaba de apretar click true si hubo click en este cuadro mp posicion del mouse
        // Devuelve: nada
        // Controles de la pantalla de inicio: JUGAR pasa a elegir el modo de juego y SALIR cierra el juego
        void ActualizarInicio(Func<Keys, bool> tecla, bool click, Point mp)
        {
            if (tecla(Keys.Escape)) { Exit(); return; }

            // ENTER o ESPACIO o click en JUGAR: pasa a la pantalla de modos
            if (tecla(Keys.Enter) || tecla(Keys.Space) || (click && RectJugar.Contains(mp)))
            {
                _estado = EstadoJuego.SeleccionModo;
                return;
            }

            // Click en SALIR: cierra el juego
            if (click && RectSalir.Contains(mp)) Exit();
        }

        // Recibe: tecla funcion que dice si una tecla se acaba de apretar click true si hubo click en este cuadro mp posicion del mouse
        // Devuelve: nada
        // Controles de la lista de modos de juego: ESC vuelve al inicio; 1 a 9 o click eligen un modo
        void ActualizarSeleccionModo(Func<Keys, bool> tecla, bool click, Point mp)
        {
            if (tecla(Keys.Escape)) { _estado = EstadoJuego.Inicio; return; }

            // Teclas 1 a 9 (fila de numeros o teclado numerico): eligen el modo con ese numero
            for (int i = 0; i < _modosJuego.Length && i < 9; i++)
            {
                if (tecla((Keys)((int)Keys.D1 + i)) || tecla((Keys)((int)Keys.NumPad1 + i)))
                {
                    ElegirModoJuego(_modosJuego[i]);
                    return;
                }
            }

            // Click: sobre el boton de un modo
            if (!click) return;
            for (int i = 0; i < _modosJuego.Length; i++)
                if (RectModoJuego(i).Contains(mp)) { ElegirModoJuego(_modosJuego[i]); break; }
        }

        // Recibe: tecla funcion que dice si una tecla se acaba de apretar click true si hubo click en este cuadro mp posicion del mouse
        // Devuelve: nada
        // Controles del menu de niveles: volver a los modos elegir nivel y cambiar de tematica
        void ActualizarMenu(Func<Keys, bool> tecla, bool click, Point mp)
        {
            if (tecla(Keys.Escape)) { _estado = EstadoJuego.SeleccionModo; return; }

            // Flechas izquierda y derecha: cambian de tematica (solo si hay mas de una)
            if (_modos.Length > 1)
            {
                if (tecla(Keys.Left)) CambiarModo(-1);
                else if (tecla(Keys.Right)) CambiarModo(1);
            }

            // Niveles del modo de juego elegido que se pueden jugar con esta tematica en el mismo orden que los botones
            Nivel[] niveles = _niveles;

            // Teclas 1 a 9 (fila de numeros o teclado numerico): inician el nivel con ese numero
            for (int i = 0; i < niveles.Length && i < 9; i++)
            {
                if (tecla((Keys)((int)Keys.D1 + i)) || tecla((Keys)((int)Keys.NumPad1 + i)))
                {
                    ComenzarJuego(niveles[i]);
                    return;
                }
            }

            // Click: en una flecha de tematica o en un boton de nivel
            if (!click) return;
            if (_modos.Length > 1 && RectFlecha(false).Contains(mp)) CambiarModo(-1);
            else if (_modos.Length > 1 && RectFlecha(true).Contains(mp)) CambiarModo(1);
            else
            {
                for (int i = 0; i < niveles.Length; i++)
                    if (RectBoton(i).Contains(mp)) { ComenzarJuego(niveles[i]); break; }
            }
        }

        // Recibe: click true si hubo click en este cuadro mp posicion del mouse y dt segundos desde el cuadro anterior Devuelve: nada
        // Maneja los clicks sobre las cartas la espera tras un fallo y el final de la partida
        void ActualizarJugando(bool click, Point mp, float dt)
        {
            // Si hubo un fallo, se dejan ver las dos cartas un momento y luego se ocultan (cola de eventos).
            if (_partida.EsperandoOcultarPorFallo)
            {
                // Va restando tiempo a la espera
                _espera -= dt;
                // Cuando llega a 0 se tapan las cartas
                if (_espera <= 0) _partida.ProcesarEventosPendientes();
            }
            else
            {
                _partida.ProcesarEventosPendientes();

                // Si hubo click busca sobre que carta cayo
                if (click)
                {
                    // Revisa todas las cartas
                    foreach (carta c in _partida.Tablero.ObtenerTodas())
                    {
                        // ¿El mouse esta dentro de esta carta?
                        if (RectDe(c).Contains(mp))
                        {
                            // Selecciona la carta Devuelve true si con ella se completo un intento 2 cartas
                            bool intentoCompleto = _partida.SeleccionarCarta(c.fila, c.columna);
                            if (intentoCompleto)
                            {
                                // Si quedo esperando para taparlas fue un fallo y arranca la espera
                                bool acierto = !_partida.EsperandoOcultarPorFallo;
                                if (!acierto) _espera = EsperaFallo;
                                // Le avisa al modo de juego (ej: multijugador pasa el turno, supervivencia resta una vida)
                                _modoJuego.AlCompletarIntento(_partida, acierto);
                            }
                            break;
                        }
                    }
                }
            }

            // Si ya estan todos los pares gano; si no y el modo de juego dice que perdio (ej: se acabo el tiempo) perdio
            if (_partida.Termino) Terminar(true);
            else if (_modoJuego.Perdio(_partida)) Terminar(false);
        }


        // Recibe: nada Devuelve: nada
        // Se ejecuta al cerrar el juego: libera la musica y los sonidos de la memoria
        protected override void UnloadContent()
        {
            _recursos?.Liberar();
            base.UnloadContent();
        }

        // Recibe: gameTime tiempo transcurrido Devuelve: nada
        // Se ejecuta en cada cuadro despues de Update: dibuja el fondo y luego el menu o la partida
        protected override void Draw(GameTime gameTime)
        {
            // Borra la pantalla con negro
            GraphicsDevice.Clear(Color.Black);
            // Empieza a juntar todo lo que se va a dibujar
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);

            // Dibuja el fondo del modo
            _spriteBatch.Draw(ModoActual.Fondo, new Rectangle(0, 0, Ancho, Alto), Color.White);
            // Capa negra semitransparente para oscurecer el fondo
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Ancho, Alto), Color.Black * 0.35f);

            // Dibuja la pantalla que corresponde al estado: inicio modos niveles o la partida (jugando gano o perdio)
            switch (_estado)
            {
                case EstadoJuego.Inicio: DibujarInicio(); break;
                case EstadoJuego.SeleccionModo: DibujarSeleccionModo(); break;
                case EstadoJuego.ElegirNivel: DibujarMenu(); break;
                default: DibujarPartida(); break;
            }

            // Termina y dibuja todo junto
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // Recibe: i numero de boton empieza en 0
        // Devuelve: el rectangulo de ese boton del menu los botones se apilan hacia abajo
        // y si el modo tiene mas de 6 niveles se abre otra columna al lado (el conjunto queda centrado)
        Rectangle RectBoton(int i)
        {
            int columnas = (_niveles.Length + BotonesPorColumna - 1) / BotonesPorColumna;
            int anchoTotal = columnas * BotonAncho + (columnas - 1) * BotonSeparacion;
            int x0 = (Ancho - anchoTotal) / 2;
            int columna = i / BotonesPorColumna;
            int fila = i % BotonesPorColumna;
            return new Rectangle(x0 + columna * (BotonAncho + BotonSeparacion), BotonesY + fila * BotonPaso, BotonAncho, BotonAlto);
        }

        // Recibe: derecha true para la flecha de la derecha false para la de la izquierda
        // Devuelve: el rectangulo de la flecha que cambia de tematica arriba del menu de niveles
        Rectangle RectFlecha(bool derecha)
        {
            return new Rectangle(derecha ? Ancho / 2 + 330 : Ancho / 2 - 390, 30, 60, 50);
        }

        // Boton JUGAR de la pantalla de inicio (grande y centrado)
        Rectangle RectJugar => new Rectangle(Ancho / 2 - 250, 520, 500, 110);

        // Boton SALIR de la pantalla de inicio (debajo de JUGAR)
        Rectangle RectSalir => new Rectangle(Ancho / 2 - 250, 660, 500, 80);

        // Recibe: i numero de modo de juego empieza en 0
        // Devuelve: el rectangulo de su boton en la lista de modos los botones se apilan hacia abajo centrados
        // Si hay muchos modos el boton se achica para que todos entren antes del borde de abajo
        Rectangle RectModoJuego(int i)
        {
            // Espacio que queda de ModosY hasta el borde de abajo (dejando 90 pixeles para el texto de ayuda)
            int disponible = Alto - 90 - ModosY;
            int paso = Math.Min(ModoPasoMaximo, disponible / _modosJuego.Length);
            return new Rectangle((Ancho - ModoAncho) / 2, ModosY + i * paso, ModoAncho, paso - ModoSeparacion);
        }

        // Recibe: s texto x y posicion escala tamano color y centrado opcional por defecto false; si es true x es el centro del texto
        // Devuelve: nada Dibuja el texto con una sombra negra detras
        void Texto(string s, int x, int y, int escala, Color color, bool centrado = false)
        {
            // Si va centrado corre x la mitad del ancho del texto
            if (centrado) x -= FuentePixel.Ancho(s, escala) / 2;
            // Primero dibuja la sombra un poco corrida
            FuentePixel.Dibujar(_spriteBatch, _pixel, s, new Vector2(x + escala, y + escala), escala, Color.Black * 0.6f);
            // Despues dibuja el texto encima
            FuentePixel.Dibujar(_spriteBatch, _pixel, s, new Vector2(x, y), escala, color);
        }

        // Recibe: r rectangulo del boton hover true si el mouse esta encima habilitado false si el boton no se puede apretar
        // Devuelve: nada Dibuja el fondo y el borde de un boton (el texto lo pone quien lo llama)
        void DibujarBoton(Rectangle r, bool hover, bool habilitado = true)
        {
            // Fondo: apagado si no se puede apretar mas claro si el mouse esta encima
            Color fondo = !habilitado ? new Color(30, 30, 40) : hover ? new Color(90, 60, 160) : new Color(40, 30, 80);
            _spriteBatch.Draw(_pixel, r, fondo * 0.9f);
            // Borde: gris apagado si no se puede apretar dorado si el mouse esta encima blanco si no
            Borde(r, !habilitado ? Color.Gray * 0.5f : hover ? Color.Gold : Color.White * 0.7f, 3);
        }

        // Recibe: r rectangulo del boton s texto escala tamano y color
        // Devuelve: nada Escribe el texto centrado dentro del boton
        void TextoEnBoton(Rectangle r, string s, int escala, Color color)
        {
            // La letra mide 7 pixeles de alto por la escala: se centra tambien hacia abajo
            Texto(s, r.X + r.Width / 2, r.Y + (r.Height - 7 * escala) / 2, escala, color, true);
        }

        // Recibe: nada Devuelve: nada
        // Dibuja la pantalla de inicio: el titulo y los botones JUGAR y SALIR
        void DibujarInicio()
        {
            // Posicion del mouse para el efecto al pasar por encima de un boton
            Point mp = Mouse.GetState().Position;

            Texto("JUEGO DE MEMORIA", Ancho / 2, 250, 11, Color.White, true);
            Texto("ENCONTRA TODOS LOS PARES", Ancho / 2, 380, 3, Color.LightGray, true);

            // Boton JUGAR grande
            Rectangle jugar = RectJugar;
            DibujarBoton(jugar, jugar.Contains(mp));
            TextoEnBoton(jugar, "JUGAR", 7, Color.White);

            // Boton SALIR mas chico
            Rectangle salir = RectSalir;
            DibujarBoton(salir, salir.Contains(mp));
            TextoEnBoton(salir, "SALIR", 4, Color.LightGray);

            Texto("ENTER: JUGAR     ESC: SALIR", 20, Alto - 30, 2, Color.LightGray);
        }

        // Recibe: nada Devuelve: nada
        // Dibuja la lista de modos de juego: los disponibles se pueden elegir y los que faltan terminar dicen PROXIMAMENTE
        void DibujarSeleccionModo()
        {
            // Posicion del mouse para el efecto al pasar por encima de un boton
            Point mp = Mouse.GetState().Position;

            Texto("JUEGO DE MEMORIA", Ancho / 2, 90, 7, Color.White, true);
            Texto("ELEGI EL MODO DE JUEGO", Ancho / 2, 180, 3, Color.LightGray, true);

            for (int i = 0; i < _modosJuego.Length; i++)
            {
                ModoJuego m = _modosJuego[i];
                Rectangle r = RectModoJuego(i);
                // Solo reacciona al mouse si el modo se puede elegir
                DibujarBoton(r, m.Disponible && r.Contains(mp), m.Disponible);

                // Los modos que todavia no estan se ven en gris
                Color colorNombre = m.Disponible ? Color.White : Color.Gray;
                Color colorDescripcion = m.Disponible ? Color.LightGray : Color.Gray * 0.8f;
                Texto($"{i + 1} - {m.Nombre.ToUpper()}", r.X + 20, r.Y + 10, 4, colorNombre);
                Texto(m.Descripcion.ToUpper(), r.X + 20, r.Y + 46, 2, colorDescripcion);

                // Cartel a la derecha para los modos que todavia no se pueden jugar
                if (!m.Disponible)
                {
                    string cartel = "PROXIMAMENTE";
                    Texto(cartel, r.Right - 20 - FuentePixel.Ancho(cartel, 3), r.Y + 14, 3, Color.Gold * 0.8f);
                }
            }

            Texto("ESC: VOLVER", 20, Alto - 30, 2, Color.LightGray);
            string ayuda = "TECLAS 1 A 9 O CLICK: ELEGIR MODO";
            Texto(ayuda, Ancho - 20 - FuentePixel.Ancho(ayuda, 2), Alto - 30, 2, Color.LightGray);
        }

        // Recibe: nada Devuelve: nada
        // Dibuja el titulo el selector de tematica los botones de nivel y los mejores puntajes
        void DibujarMenu()
        {
            Modo modo = ModoActual;
            // Posicion del mouse para el efecto al pasar por encima de un boton
            Point mp = Mouse.GetState().Position;

            Texto("JUEGO DE MEMORIA", Ancho / 2, 100, 7, Color.White, true);
            // Debajo del titulo va el modo de juego elegido y que hay que hacer ej: NIVEL CRONOMETRO - ELEGI LA DIFICULTAD
            Texto($"{_modoJuego.Nombre.ToUpper()} - ELEGI LA DIFICULTAD", Ancho / 2, 190, 3, Color.LightGray, true);

            // Selector de tematica: solo aparece si hay mas de una
            if (_modos.Length > 1)
            {
                DibujarFlecha(false, mp);
                DibujarFlecha(true, mp);
                Texto(modo.Nombre.ToUpper(), Ancho / 2, 41, 4, Color.Gold, true);
            }

            // Si la tematica no tiene caras para ningun nivel de este modo de juego avisa en vez de dejar la pantalla vacia
            if (_niveles.Length == 0)
                Texto("ESTA TEMATICA NO TIENE CARAS SUFICIENTES PARA ESTE MODO", Ancho / 2, BotonesY + 40, 3, Color.OrangeRed, true);

            // Dibuja un boton por cada nivel del modo de juego
            for (int i = 0; i < _niveles.Length; i++)
            {
                Nivel n = _niveles[i];
                Rectangle r = RectBoton(i);
                // true si el mouse esta encima de este boton
                bool hover = r.Contains(mp);
                // Fondo del boton mas claro si el mouse esta encima
                _spriteBatch.Draw(_pixel, r, (hover ? new Color(90, 60, 160) : new Color(40, 30, 80)) * 0.9f);
                // Borde dorado si el mouse esta encima blanco si no
                Borde(r, hover ? Color.Gold : Color.White * 0.7f, 3);
                // Numero y nombre del nivel
                Texto($"{i + 1} - {n.Nombre.ToUpper()}", r.X + 20, r.Y + 12, 4, Color.White);
                // Datos del nivel: el tamano del tablero y, si el nivel lo tiene, el tiempo limite
                Texto(n.Detalle, r.X + 20, r.Y + 44, 2, Color.LightGray);
            }

            // Seccion de mejores puntajes: va justo debajo de los botones (con 6 niveles queda en y = 800)
            int yRecords = BotonesY + Math.Min(Math.Max(_niveles.Length, 1), BotonesPorColumna) * BotonPaso;
            Texto("MEJORES PUNTAJES", Ancho / 2, yRecords, 3, Color.Gold, true);
            // Los 5 mejores puntajes
            List<Puntaje> top = _records.ObtenerTop(5);
            // Si no hay ninguno muestra un aviso
            if (top.Count == 0) Texto("TODAVIA NO HAY PARTIDAS", Ancho / 2, yRecords + 50, 2, Color.LightGray, true);
            // Una linea por puntaje: puesto nivel puntos y tiempo
            for (int i = 0; i < top.Count; i++)
                Texto($"{i + 1}. {top[i].Nivel.ToUpper()}  {top[i].Puntuacion} PTS  {top[i].Tiempo:mm\\:ss}", Ancho / 2, yRecords + 50 + i * 26, 2, Color.White, true);

            Texto("ESC: VOLVER", 20, Alto - 30, 2, Color.LightGray);
            if (_modos.Length > 1)
            {
                string ayuda = "FLECHAS: CAMBIAR TEMATICA";
                Texto(ayuda, Ancho - 20 - FuentePixel.Ancho(ayuda, 2), Alto - 30, 2, Color.LightGray);
            }
        }

        // Recibe: derecha true para la flecha de la derecha false para la de la izquierda mp posicion del mouse
        // Devuelve: nada Dibuja un boton con una flecha para cambiar de tematica
        void DibujarFlecha(bool derecha, Point mp)
        {
            Rectangle r = RectFlecha(derecha);
            bool hover = r.Contains(mp);
            _spriteBatch.Draw(_pixel, r, (hover ? new Color(90, 60, 160) : new Color(40, 30, 80)) * 0.9f);
            Borde(r, hover ? Color.Gold : Color.White * 0.7f, 3);
            Texto(derecha ? ">" : "<", r.X + r.Width / 2, r.Y + 11, 4, Color.White, true);
        }

        // Recibe: nada Devuelve: nada
        // Dibuja las cartas los textos de arriba tiempo intentos pares nivel y el cartel final si la partida termino
        void DibujarPartida()
        {
            // Posicion del mouse para levantar la carta que esta debajo
            Point mp = Mouse.GetState().Position;
            // bloqueado = no se puede elegir carta hay un fallo esperando o la partida termino
            bool bloqueado = _partida.EsperandoOcultarPorFallo || _estado != EstadoJuego.Jugando;

            // Dibuja cada carta
            foreach (carta c in _partida.Tablero.ObtenerTodas())
            {
                Rectangle r = RectDe(c);
                // Se ve la cara si esta dada vuelta o emparejada
                bool visible = c.estaDescubierta || c.estaEmparejada;
                // Elige cara o dorso
                Sprite s = visible ? c.descubierta : c.cubierta;
                Color tinte = Color.White;

                // Las emparejadas se ven mas apagadas; la carta bajo el mouse se levanta 6 pixeles
                if (c.estaEmparejada) tinte = Color.White * 0.6f;
                else if (!visible && !bloqueado && r.Contains(mp)) r.Y -= 6;

                // Dibuja una sombra detras de la carta
                if (!c.estaEmparejada)
                    _spriteBatch.Draw(_pixel, new Rectangle(r.X + 4, r.Y + 6, r.Width, r.Height), Color.Black * 0.35f);
                // Dibuja la carta
                _spriteBatch.Draw(s.textura, r, tinte);
            }

            // Esquina de arriba a la izquierda: lo que muestra el modo de juego
            // Texto del tiempo (ej: TIEMPO 01:30) null si el modo no tiene cronometro (ej: clasico)
            string tiempo = _modoJuego.TextoTiempo(_partida);
            // Intentos hechos hasta ahora
            string intentos = $"INTENTOS {_partida.Contador.Intentos}";
            // Linea de abajo de la 1ra donde empiezan las lineas chicas
            int yLinea = 56;
            if (tiempo != null)
            {
                // El tiempo se pone rojo cuando el modo dice que es urgente (ej: ultimos 10 segundos)
                Texto(tiempo, 20, 20, 4, _modoJuego.TiempoUrgente(_partida) ? Color.OrangeRed : Color.White);
                Texto(intentos, 20, yLinea, 2, Color.LightGray);
                yLinea += 22;
            }
            else
            {
                // Sin cronometro los intentos pasan a ser el dato grande
                Texto(intentos, 20, 20, 4, Color.White);
            }
            string[] lineasInfo = _modoJuego.LineasInfo(_partida);
            for (int i = 0; i < lineasInfo.Length; i++)
            {
                Texto(lineasInfo[i], 20, yLinea, 2, _modoJuego.ColorLineaInfo(_partida, i));
                yLinea += 22;
            }
            // Pares encontrados / pares totales del nivel pegado a la derecha
            string pares = $"PARES {_partida.ParesDescubiertos.CantidadPares}/{_nivel.Pares}";
            Texto(pares, Ancho - 20 - FuentePixel.Ancho(pares, 4), 20, 4, Color.White);
            // Nombre del nivel
            Texto(_nivel.Nombre.ToUpper(), Ancho - 20 - FuentePixel.Ancho(_nivel.Nombre, 2), 56, 2, Color.LightGray);

            // Si la partida termino oscurece todo y muestra el resultado
            if (_estado == EstadoJuego.Gano || _estado == EstadoJuego.Perdio)
            {
                _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Ancho, Alto), Color.Black * 0.7f);
                // true si gano false si perdio
                bool gano = _estado == EstadoJuego.Gano;
                // Titulo: lo pone el modo de juego ej: GANASTE! o SE ACABO EL TIEMPO
                Texto(gano ? _modoJuego.TituloVictoria(_partida) : _modoJuego.MotivoDerrota, Ancho / 2, 250, gano ? 9 : 6, gano ? Color.Gold : Color.OrangeRed, true);
                // Si gano muestra puntos tiempo e intentos
                if (gano && _ultimoPuntaje != null)
                {
                    Texto($"PUNTOS: {_ultimoPuntaje.Puntuacion}", Ancho / 2, 380, 4, Color.White, true);
                    Texto($"TIEMPO {_ultimoPuntaje.Tiempo:mm\\:ss}   INTENTOS {_ultimoPuntaje.Intentos}", Ancho / 2, 440, 3, Color.LightGray, true);
                }
                // Indica las teclas disponibles
                Texto("R: REINTENTAR     ENTER: MENU", Ancho / 2, 540, 3, Color.White, true);
            }
        }

        // Recibe: r rectangulo color y grosor en pixeles Devuelve: nada
        // Dibuja solo el contorno usando 4 rectangulos finos
        void Borde(Rectangle r, Color color, int grosor)
        {
            // Linea de arriba
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, r.Width, grosor), color);
            // Linea de abajo
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Bottom - grosor, r.Width, grosor), color);
            // Linea izquierda
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, grosor, r.Height), color);
            // Linea derecha
            _spriteBatch.Draw(_pixel, new Rectangle(r.Right - grosor, r.Y, grosor, r.Height), color);
        }
    }
}