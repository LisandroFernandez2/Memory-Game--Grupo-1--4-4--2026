using juegojuego;
using juegojuego.modelos;
using System;
using System.Collections.Generic;

namespace juegojuego
{
    // Orquesta una partida completa: mantiene el tablero (array 2D), procesa
    // las jugadas del usuario, y coordina todas las estructuras de datos
    // (pila de historial, cola de eventos, lista de pares descubiertos,
    // contador y cronómetro) definidas para el juego
    // Clase con la logica de una partida: junta el tablero el historial la cola la lista de pares el contador y el cronometro
    public class Partida
    {
        // El tablero con todas las cartas solo se puede cambiar desde dentro de la clase
        public tablero Tablero { get; set; }
        // Dificultad elegida para esta partida
        public Nivel NivelActual { get; set; }
        // Nombre del jugador
        public string NombreJugador { get; set; }

        // Jugadores de la partida: 1 en solitario, 2 o mas en los modos multijugador
        public List<Jugador> Jugadores { get; } = new List<Jugador>();
        // Posicion en la lista Jugadores del jugador al que le toca jugar
        public int TurnoActual { get; private set; }
        // El jugador al que le toca jugar ahora
        public Jugador JugadorActual => Jugadores[TurnoActual];

        // PILA: guarda los intentos hechos
        public HistorialMovimientos Historial { get; } = new HistorialMovimientos();
        // COLA: acciones pendientes sobre las cartas voltear ocultar marcar
        public ColaEventos Eventos { get; } = new ColaEventos();
        // LISTA ENLAZADA: pares que ya se encontraron
        public ListaParesDescubiertos ParesDescubiertos { get; } = new ListaParesDescubiertos();
        // Cuenta intentos aciertos y fallos
        public ContadorPartida Contador { get; } = new ContadorPartida();
        // Mide el tiempo de la partida
        public Cronometro Cronometro { get; } = new Cronometro();

        // Vidas del jugador (solo se usan en el modo Supervivencia)
        public Vidas Vidas { get; set; } = new Vidas();


        // Primera carta que se dio vuelta en el intento actual null si todavia no se eligio ninguna
        private carta primeraSeleccion;
        // Segunda carta del intento actual
        private carta segundaSeleccion;

        // true mientras hay un fallo esperando que se tapen las 2 cartas Durante ese tiempo no se pueden elegir mas
        public bool EsperandoOcultarPorFallo { get; set; }

        // Constructor
        // Recibe: nombreJugador nivel fabricarSprites una funcion que recibe el valor de la carta y devuelve sus dos sprites: dorso y cara
        // y cantidadJugadores cuantos juegan (si no se pone es 1 o sea solitario)
        // Devuelve: nada crea la partida
        // Como funciona: crea el tablero lo llena de cartas pone el contador en 0 y arranca el cronometro
        public Partida(string nombreJugador, Nivel nivel, Func<int, (Sprite cubierta, Sprite descubierta)> fabricarSprites, int cantidadJugadores = 1)
        {
            // Guarda el jugador y el nivel
            NombreJugador = nombreJugador;
            NivelActual = nivel;
            // Crea los jugadores: uno solo se llama como nombreJugador; si son varios JUGADOR 1, JUGADOR 2, etc.
            if (cantidadJugadores <= 1)
                Jugadores.Add(new Jugador(nombreJugador));
            else
                for (int i = 1; i <= cantidadJugadores; i++)
                    Jugadores.Add(new Jugador($"JUGADOR {i}"));
            // Crea el tablero vacio con el tamano del nivel
            Tablero = new tablero(nivel.Filas, nivel.Columnas);
            // Llena el tablero con cartas mezcladas al azar
            GeneradorTablero.GenerarTablero(Tablero, fabricarSprites);

            // >>> NUEVO: si el nivel es de supervivencia, el jugador arranca con las vidas del nivel
            if (nivel is NivelSupervivencia supervivencia)
                Vidas.vidas = supervivencia.vidas;

            // Contadores en 0 y cronometro andando
            Contador.Reiniciar();
            Cronometro.Iniciar();
            // Anota en el log que arranco la partida
            GestorArchivos.RegistrarLog($"Partida iniciada: jugador={nombreJugador}, nivel={nivel.Nombre}");
        }

        // Procesa la selección de una carta por parte del jugador. Devuelve true
        // si con esta selección se completó un intento
        // Recibe: fila y columna de la carta clickeada
        // Devuelve: true si con esta carta se completo un intento ya hay 2 cartas false si no paso nada o si es la primera del intento
        public bool SeleccionarCarta(int fila, int columna)
        {
            // Si hay un fallo pendiente ignora el click
            if (EsperandoOcultarPorFallo)
                return false; // hay que resolver el evento de "ocultar" primero

            // Busca la carta que esta en esa posicion
            carta seleccionada = Tablero.ObtenerCarta(fila, columna);
            // Ignora el click si no hay carta si ya esta emparejada o si ya esta dada vuelta
            if (seleccionada == null || seleccionada.estaEmparejada || seleccionada.estaDescubierta)
                return false;

            // La da vuelta y avisa a la cola de eventos
            seleccionada.Voltear();
            Eventos.Encolar(new EventoVolteo(fila, columna, TipoEvento.Voltear));

            // Si es la primera del intento la guarda y espera a la segunda
            if (primeraSeleccion == null)
            {
                primeraSeleccion = seleccionada;
                return false;
            }

            // Si es la segunda verifica si forman par
            segundaSeleccion = seleccionada;
            return VerificarPar();
        }

        // Compara las dos cartas seleccionadas y actualiza todas las estructuras
        // Recibe: nada usa las dos cartas elegidas
        // Devuelve: siempre true porque el intento ya termino
        // Como funciona: compara las cartas guarda el intento y actualiza el contador la lista y la cola
        private bool VerificarPar()
        {
            // Hay acierto si las dos cartas tienen el mismo idValor
            bool acierto = primeraSeleccion.idValor == segundaSeleccion.idValor;

            // Arma el registro del intento
            var movimiento = new Movimiento(
                primeraSeleccion.fila, primeraSeleccion.columna,
                segundaSeleccion.fila, segundaSeleccion.columna,
                acierto);
            Historial.Registrar(movimiento); // PILA

            // Suma el intento al contador y tambien al jugador al que le tocaba
            Contador.RegistrarIntento(acierto);
            if (acierto) JugadorActual.aciertos++;
            else JugadorActual.fallos++;

            // ACIERTO: las deja fijas emparejadas y suma el par con 100 puntos
            if (acierto)
            {
                primeraSeleccion.estaEmparejada = true;
                segundaSeleccion.estaEmparejada = true;
                Eventos.Encolar(new EventoVolteo(primeraSeleccion.fila, primeraSeleccion.columna, TipoEvento.MarcarEmparejada));
                Eventos.Encolar(new EventoVolteo(segundaSeleccion.fila, segundaSeleccion.columna, TipoEvento.MarcarEmparejada));

                ParesDescubiertos.AgregarPar(primeraSeleccion.idValor, 100); // LISTA ENLAZADA
            }
            else
            {
                // FALLO: bloquea los clicks hasta que se tapen las dos cartas
                // (las vidas las resta el modo de juego en AlCompletarIntento, no se toca aca)
                EsperandoOcultarPorFallo = true; // se ocultarán al procesar los eventos encolados
                Eventos.Encolar(new EventoVolteo(primeraSeleccion.fila, primeraSeleccion.columna, TipoEvento.Ocultar));
                Eventos.Encolar(new EventoVolteo(segundaSeleccion.fila, segundaSeleccion.columna, TipoEvento.Ocultar));
            }

            // Limpia la seleccion para el proximo intento
            primeraSeleccion = null;
            segundaSeleccion = null;
            return true;
        }

        // Debe llamarse desde Update para ir drenando la COLA de eventos y aplicar
        // las animaciones/estado correspondiente en el orden en que ocurrieron.
        // Recibe: nada Devuelve: nada
        // Como funciona: vacia la cola aplicando cada evento a su carta en el orden en que llegaron Se llama desde Update
        public void ProcesarEventosPendientes()
        {
            // Repite mientras queden eventos en la cola
            while (Eventos.HayEventosPendientes())
            {
                // Saca el evento mas viejo
                EventoVolteo evento = Eventos.ProcesarSiguiente();
                // Busca la carta a la que afecta ese evento
                carta c = Tablero.ObtenerCarta(evento.Fila, evento.Columna);

                // Hace lo que corresponde segun el tipo de evento
                switch (evento.Tipo)
                {
                    case TipoEvento.Ocultar:
                        c.Ocultar();
                        break;
                    case TipoEvento.Voltear:
                        c.Voltear();
                        break;
                    case TipoEvento.MarcarEmparejada:
                        c.estaEmparejada = true;
                        break;
                }
            }
            // Ya se taparon las cartas: se puede volver a elegir
            EsperandoOcultarPorFallo = false;
        }

        // Le pasa el turno al siguiente jugador (despues del ultimo vuelve al primero)
        // Recibe: nada Devuelve: nada
        // Lo usan los modos multijugador, ej: cuando un jugador falla en AlCompletarIntento
        public void PasarTurno()
        {
            TurnoActual = (TurnoActual + 1) % Jugadores.Count;
        }

        // Deshace el último intento realizado
        // Recibe: nada
        // Devuelve: el Movimiento que se deshizo o null si no habia ninguno
        // Como funciona: revierte el ultimo intento y tapa de nuevo sus dos cartas
        public Movimiento DeshacerUltimoMovimiento()
        {
            // Saca el ultimo movimiento de la pila
            Movimiento anterior = Historial.DeshacerUltimo();
            // Si no habia ninguno no hay nada que deshacer
            if (anterior == null) return null;

            carta c1 = Tablero.ObtenerCarta(anterior.Fila1, anterior.Columna1);
            carta c2 = Tablero.ObtenerCarta(anterior.Fila2, anterior.Columna2);

            // Si habia sido acierto las vuelve a dejar sin emparejar y saca el par de la lista
            if (anterior.Acierto)
            {
                c1.estaEmparejada = false;
                c2.estaEmparejada = false;
                ParesDescubiertos.QuitarPar(c1.idValor);
            }
            // Tapa de nuevo las dos cartas
            c1.Ocultar();
            c2.Ocultar();

            return anterior;
        }

        // Devuelve: true cuando todas las cartas estan emparejadas o sea ganaste
        public bool Termino => Tablero.TableroCompleto();

        // Cierra la partida: detiene el cronómetro y arma el registro de puntaje final
        // Recibe: nada
        // Devuelve: el Puntaje final de la partida
        // Como funciona: frena el cronometro calcula los puntos arma el registro y lo anota en el log
        public Puntaje Finalizar()
        {
            Cronometro.Pausar();
            // Calcula los puntos finales segun aciertos fallos y tiempo
            int puntuacion = Contador.CalcularPuntuacion(Cronometro.SegundosTranscurridos);

            // Arma el registro con todos los datos de la partida
            var puntaje = new Puntaje(
                NombreJugador, DateTime.Now, NivelActual.Nombre,
                Contador.Intentos, Contador.Aciertos, Contador.Fallos,
                Cronometro.TiempoTranscurrido, puntuacion);

            GestorArchivos.RegistrarLog($"Partida finalizada: {puntaje}");
            return puntaje;
        }
    }
}
