using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace juegojuego
{
    public enum EstadoJuego { Menu, Jugando, Gano, Perdio }

    public class Game1 : Game
    {
        private const int Ancho = 1900;
        private const int Alto = 1050;
        private const float RatioCarta = 300f / 392f;
        private const float EsperaFallo = 0.8f;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel, _mesa;
        private readonly List<Texture2D> _caras = new List<Texture2D>();
        private readonly List<Texture2D> _dorsos = new List<Texture2D>();

        private readonly Random _rng = new Random();
        private readonly TablaRecords _records = new TablaRecords();
        private EstadoJuego _estado = EstadoJuego.Menu;
        private Partida _partida;
        private Nivel _nivel;
        private Puntaje _ultimoPuntaje;
        private float _espera;
        private float _cartaAncho, _cartaAlto;

        private KeyboardState _tecPrev;
        private MouseState _mouPrev;

        private readonly Nivel[] _niveles = {Nivel.MuyFacil, Nivel.Facil, Nivel.Normal, Nivel.Dificil,Nivel.MuyDificil ,Nivel.Imposible};

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Ancho,
                PreferredBackBufferHeight = Alto
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Juego de Memoria";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            string dir = Path.Combine(AppContext.BaseDirectory, "Assets", "img");
            _mesa = CargarTextura(Path.Combine(dir, "mesa.jpg"));

            foreach (string archivo in Directory.GetFiles(dir, "face_*.png"))
                _caras.Add(CargarTextura(archivo));
            foreach (string archivo in Directory.GetFiles(dir, "back_*.png"))
                _dorsos.Add(CargarTextura(archivo));

            try { _records.CargarDesde(GestorArchivos.CargarRecordsBinario()); }
            catch (Exception) { /* archivo de records daniado: se empieza vacio */ }
        }

        // Carga una imagen desde disco y la convierte a alfa premultiplicado
        private Texture2D CargarTextura(string ruta)
        {
            Texture2D tex;
            using (FileStream fs = File.OpenRead(ruta))
                tex = Texture2D.FromStream(GraphicsDevice, fs);

            Color[] datos = new Color[tex.Width * tex.Height];
            tex.GetData(datos);
            for (int i = 0; i < datos.Length; i++)
            {
                Color c = datos[i];
                datos[i] = new Color(c.R * c.A / 255, c.G * c.A / 255, c.B * c.A / 255, (int)c.A);
            }
            tex.SetData(datos);
            return tex;
        }


        private void IniciarPartida(Nivel nivel)
        {
            _nivel = nivel;
            int pares = nivel.Filas * nivel.Columnas / 2;

            // Elegimos al azar que caras (de todas las tematicas) se usan en esta partida.
            List<int> indices = new List<int>();
            for (int i = 0; i < _caras.Count; i++) indices.Add(i);
            for (int i = indices.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (indices[i], indices[j]) = (indices[j], indices[i]);
            }
            List<int> elegidas = indices.GetRange(0, pares);
            Texture2D dorso = _dorsos[_rng.Next(_dorsos.Count)];

            _partida = new Partida("JUGADOR", nivel, valor =>
                (new Sprite(dorso, Vector2.Zero), new Sprite(_caras[elegidas[valor]], Vector2.Zero)));

            AcomodarCartas();
            _espera = 0;
            _ultimoPuntaje = null;
            _estado = EstadoJuego.Jugando;
        }

        private void AcomodarCartas()
        {
            tablero t = _partida.Tablero;
            const float gap = 10f, top = 90f, bajo = 20f;
            float dispW = Ancho - 40f, dispH = Alto - top - bajo;

            float altoPorFila = (dispH - (t.filas - 1) * gap) / t.filas;
            float altoPorColumna = (dispW - (t.columnas - 1) * gap) / t.columnas / RatioCarta;
            _cartaAlto = Math.Min(altoPorFila, altoPorColumna);
            _cartaAncho = _cartaAlto * RatioCarta;

            float totalW = t.columnas * _cartaAncho + (t.columnas - 1) * gap;
            float totalH = t.filas * _cartaAlto + (t.filas - 1) * gap;
            float x0 = (Ancho - totalW) / 2f;
            float y0 = top + (dispH - totalH) / 2f;

            foreach (carta c in t.ObtenerTodas())
            {
                Vector2 pos = new Vector2(x0 + c.columna * (_cartaAncho + gap), y0 + c.fila * (_cartaAlto + gap));
                c.cubierta.position = pos;
                c.descubierta.position = pos;
            }
        }

        private Rectangle RectDe(carta c)
        {
            return new Rectangle((int)c.cubierta.position.X, (int)c.cubierta.position.Y, (int)_cartaAncho, (int)_cartaAlto);
        }

        private void Terminar(bool gano)
        {
            if (gano)
            {
                _ultimoPuntaje = _partida.Finalizar();
                _records.InsertarOrdenado(_ultimoPuntaje);
                try { GestorArchivos.GuardarRecordsBinario(new List<Puntaje>(_records.Registros)); }
                catch (Exception) { }
                _estado = EstadoJuego.Gano;
            }
            else
            {
                _partida.Cronometro.Pausar();
                _estado = EstadoJuego.Perdio;
            }
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardState tec = Keyboard.GetState();
            MouseState mou = Mouse.GetState();
            bool Tecla(Keys k) => tec.IsKeyDown(k) && _tecPrev.IsKeyUp(k);
            bool click = mou.LeftButton == ButtonState.Pressed && _mouPrev.LeftButton == ButtonState.Released;
            Point mp = mou.Position;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (_estado)
            {
                case EstadoJuego.Menu:
                    if (Tecla(Keys.Escape)) Exit();
                    if (Tecla(Keys.D1) || Tecla(Keys.NumPad1)) IniciarPartida(_niveles[0]);
                    else if (Tecla(Keys.D2) || Tecla(Keys.NumPad2)) IniciarPartida(_niveles[1]);
                    else if (Tecla(Keys.D3) || Tecla(Keys.NumPad3)) IniciarPartida(_niveles[2]);
                    else if (click)
                    {
                        for (int i = 0; i < _niveles.Length; i++)
                            if (RectBoton(i).Contains(mp)) { IniciarPartida(_niveles[i]); break; }
                    }
                    break;

                case EstadoJuego.Jugando:
                    if (Tecla(Keys.Escape)) { _estado = EstadoJuego.Menu; break; }
                    ActualizarJugando(click, mp, dt);
                    break;

                case EstadoJuego.Gano:
                case EstadoJuego.Perdio:
                    if (Tecla(Keys.Escape) || Tecla(Keys.Enter)) _estado = EstadoJuego.Menu;
                    else if (Tecla(Keys.R)) IniciarPartida(_nivel);
                    break;
            }

            _tecPrev = tec;
            _mouPrev = mou;
            base.Update(gameTime);
        }

        private void ActualizarJugando(bool click, Point mp, float dt)
        {
            // Si hubo un fallo, se dejan ver las dos cartas un momento y luego se ocultan (cola de eventos).
            if (_partida.EsperandoOcultarPorFallo)
            {
                _espera -= dt;
                if (_espera <= 0) _partida.ProcesarEventosPendientes();
            }
            else
            {
                _partida.ProcesarEventosPendientes();

                if (click)
                {
                    foreach (carta c in _partida.Tablero.ObtenerTodas())
                    {
                        if (RectDe(c).Contains(mp))
                        {
                            bool intentoCompleto = _partida.SeleccionarCarta(c.fila, c.columna);
                            if (intentoCompleto && _partida.EsperandoOcultarPorFallo)
                                _espera = EsperaFallo;
                            break;
                        }
                    }
                }
            }

            if (_partida.Termino) Terminar(true);
            else if (_partida.Cronometro.SeAgotoElTiempo(_nivel.TiempoLimiteSegundos)) Terminar(false);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);

            _spriteBatch.Draw(_mesa, new Rectangle(0, 0, Ancho, Alto), Color.White);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Ancho, Alto), Color.Black * 0.35f);

            if (_estado == EstadoJuego.Menu) DibujarMenu();
            else DibujarPartida();

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private Rectangle RectBoton(int i) => new Rectangle(Ancho / 2 - 220, 260 + i * 90, 440, 70);

        private void Texto(string s, int x, int y, int escala, Color color, bool centrado = false)
        {
            if (centrado) x -= FuentePixel.Ancho(s, escala) / 2;
            FuentePixel.Dibujar(_spriteBatch, _pixel, s, new Vector2(x + escala, y + escala), escala, Color.Black * 0.6f);
            FuentePixel.Dibujar(_spriteBatch, _pixel, s, new Vector2(x, y), escala, color);
        }

        private void DibujarMenu()
        {
            Texto("JUEGO DE MEMORIA", Ancho / 2, 100, 7, Color.White, true);
            Texto("ELEGI LA DIFICULTAD", Ancho / 2, 190, 3, Color.LightGray, true);

            Point mp = Mouse.GetState().Position;
            for (int i = 0; i < _niveles.Length; i++)
            {
                Nivel n = _niveles[i];
                Rectangle r = RectBoton(i);
                bool hover = r.Contains(mp);
                _spriteBatch.Draw(_pixel, r, (hover ? new Color(90, 60, 160) : new Color(40, 30, 80)) * 0.9f);
                Borde(r, hover ? Color.Gold : Color.White * 0.7f, 3);
                Texto($"{i + 1} - {n.Nombre.ToUpper()}", r.X + 20, r.Y + 12, 4, Color.White);
                Texto($"{n.Filas}X{n.Columnas}  {n.TiempoLimiteSegundos}S", r.X + 20, r.Y + 44, 2, Color.LightGray);
            }

            Texto("MEJORES PUNTAJES", Ancho / 2, 800, 3, Color.Gold, true);
            List<Puntaje> top = _records.ObtenerTop(5);
            if (top.Count == 0) Texto("TODAVIA NO HAY PARTIDAS", Ancho / 2, 610, 2, Color.LightGray, true);
            for (int i = 0; i < top.Count; i++)
                Texto($"{i + 1}. {top[i].Nivel.ToUpper()}  {top[i].Puntuacion} PTS  {top[i].Tiempo:mm\\:ss}", Ancho / 2, 850 + i * 26, 2, Color.White, true);

            Texto("ESC: SALIR", 20, Alto - 30, 2, Color.LightGray);
        }

        private void DibujarPartida()
        {
            Point mp = Mouse.GetState().Position;
            bool bloqueado = _partida.EsperandoOcultarPorFallo || _estado != EstadoJuego.Jugando;

            foreach (carta c in _partida.Tablero.ObtenerTodas())
            {
                Rectangle r = RectDe(c);
                bool visible = c.estaDescubierta || c.estaEmparejada;
                Sprite s = visible ? c.descubierta : c.cubierta;
                Color tinte = Color.White;

                if (c.estaEmparejada) tinte = Color.White * 0.6f;
                else if (!visible && !bloqueado && r.Contains(mp)) r.Y -= 6;

                if (!c.estaEmparejada)
                    _spriteBatch.Draw(_pixel, new Rectangle(r.X + 4, r.Y + 6, r.Width, r.Height), Color.Black * 0.35f);
                _spriteBatch.Draw(s.texture, r, tinte);
            }

            int restante = Math.Max(0, _nivel.TiempoLimiteSegundos - _partida.Cronometro.SegundosTranscurridos);
            Color colorTiempo = restante <= 10 ? Color.OrangeRed : Color.White;
            Texto($"TIEMPO {restante / 60:00}:{restante % 60:00}", 20, 20, 4, colorTiempo);
            Texto($"INTENTOS {_partida.Contador.Intentos}", 20, 56, 2, Color.LightGray);
            int totalPares = _nivel.Filas * _nivel.Columnas / 2;
            Texto($"PARES {_partida.ParesDescubiertos.CantidadPares}/{totalPares}", Ancho - 20 - FuentePixel.Ancho($"PARES {_partida.ParesDescubiertos.CantidadPares}/{totalPares}", 4), 20, 4, Color.White);
            Texto(_nivel.Nombre.ToUpper(), Ancho - 20 - FuentePixel.Ancho(_nivel.Nombre, 2), 56, 2, Color.LightGray);

            if (_estado == EstadoJuego.Gano || _estado == EstadoJuego.Perdio)
            {
                _spriteBatch.Draw(_pixel, new Rectangle(0, 0, Ancho, Alto), Color.Black * 0.7f);
                bool gano = _estado == EstadoJuego.Gano;
                Texto(gano ? "GANASTE!" : "SE ACABO EL TIEMPO", Ancho / 2, 250, gano ? 9 : 6, gano ? Color.Gold : Color.OrangeRed, true);
                if (gano && _ultimoPuntaje != null)
                {
                    Texto($"PUNTOS: {_ultimoPuntaje.Puntuacion}", Ancho / 2, 380, 4, Color.White, true);
                    Texto($"TIEMPO {_ultimoPuntaje.Tiempo:mm\\:ss}   INTENTOS {_ultimoPuntaje.Intentos}", Ancho / 2, 440, 3, Color.LightGray, true);
                }
                Texto("R: REINTENTAR     ENTER: MENU", Ancho / 2, 540, 3, Color.White, true);
            }
        }

        private void Borde(Rectangle r, Color color, int grosor)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, r.Width, grosor), color);
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Bottom - grosor, r.Width, grosor), color);
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, grosor, r.Height), color);
            _spriteBatch.Draw(_pixel, new Rectangle(r.Right - grosor, r.Y, grosor, r.Height), color);
        }
    }
}
