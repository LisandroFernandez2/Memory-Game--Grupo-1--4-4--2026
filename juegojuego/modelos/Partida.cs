using System;

namespace juegojuego
{
    // Orquesta una partida completa: mantiene el tablero (array 2D), procesa
    // las jugadas del usuario, y coordina todas las estructuras de datos
    // (pila de historial, cola de eventos, lista de pares descubiertos,
    // contador y cronómetro) definidas para el juego
    public class Partida
    {
        public tablero Tablero { get; private set; }
        public Nivel NivelActual { get; private set; }
        public string NombreJugador { get; private set; }

        public HistorialMovimientos Historial { get; } = new HistorialMovimientos();
        public ColaEventos Eventos { get; } = new ColaEventos();
        public ListaParesDescubiertos ParesDescubiertos { get; } = new ListaParesDescubiertos();
        public ContadorPartida Contador { get; } = new ContadorPartida();
        public Cronometro Cronometro { get; } = new Cronometro();

        private carta primeraSeleccion;
        private carta segundaSeleccion;

        public bool EsperandoOcultarPorFallo { get; private set; }

        public Partida(string nombreJugador, Nivel nivel, Func<int, (Sprite cubierta, Sprite descubierta)> fabricarSprites)
        {
            NombreJugador = nombreJugador;
            NivelActual = nivel;
            Tablero = new tablero(nivel.Filas, nivel.Columnas);
            GeneradorTablero.GenerarTablero(Tablero, fabricarSprites);

            Contador.Reiniciar();
            Cronometro.Iniciar();
            GestorArchivos.RegistrarLog($"Partida iniciada: jugador={nombreJugador}, nivel={nivel.Nombre}");
        }

        // Procesa la selección de una carta por parte del jugador. Devuelve true
        // si con esta selección se completó un intento
        public bool SeleccionarCarta(int fila, int columna)
        {
            if (EsperandoOcultarPorFallo)
                return false; // hay que resolver el evento de "ocultar" primero

            carta seleccionada = Tablero.ObtenerCarta(fila, columna);
            if (seleccionada == null || seleccionada.estaEmparejada || seleccionada.estaDescubierta)
                return false;

            seleccionada.Voltear();
            Eventos.Encolar(new EventoVolteo(fila, columna, TipoEvento.Voltear));

            if (primeraSeleccion == null)
            {
                primeraSeleccion = seleccionada;
                return false;
            }

            segundaSeleccion = seleccionada;
            return VerificarPar();
        }

        // Compara las dos cartas seleccionadas y actualiza todas las estructuras
        private bool VerificarPar()
        {
            bool acierto = primeraSeleccion.idValor == segundaSeleccion.idValor;

            var movimiento = new Movimiento(
                primeraSeleccion.fila, primeraSeleccion.columna,
                segundaSeleccion.fila, segundaSeleccion.columna,
                acierto);
            Historial.Registrar(movimiento); // PILA

            Contador.RegistrarIntento(acierto);

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
                EsperandoOcultarPorFallo = true; // se ocultarán al procesar los eventos encolados
                Eventos.Encolar(new EventoVolteo(primeraSeleccion.fila, primeraSeleccion.columna, TipoEvento.Ocultar));
                Eventos.Encolar(new EventoVolteo(segundaSeleccion.fila, segundaSeleccion.columna, TipoEvento.Ocultar));
            }

            primeraSeleccion = null;
            segundaSeleccion = null;
            return true;
        }

        // Debe llamarse desde Update para ir drenando la COLA de eventos y aplicar
        // las animaciones/estado correspondiente en el orden en que ocurrieron.
        public void ProcesarEventosPendientes()
        {
            while (Eventos.HayEventosPendientes())
            {
                EventoVolteo evento = Eventos.ProcesarSiguiente();
                carta c = Tablero.ObtenerCarta(evento.Fila, evento.Columna);

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
            EsperandoOcultarPorFallo = false;
        }

        // Deshace el último intento realizado
        public Movimiento DeshacerUltimoMovimiento()
        {
            Movimiento anterior = Historial.DeshacerUltimo();
            if (anterior == null) return null;

            carta c1 = Tablero.ObtenerCarta(anterior.Fila1, anterior.Columna1);
            carta c2 = Tablero.ObtenerCarta(anterior.Fila2, anterior.Columna2);

            if (anterior.Acierto)
            {
                c1.estaEmparejada = false;
                c2.estaEmparejada = false;
                ParesDescubiertos.QuitarPar(c1.idValor);
            }
            c1.Ocultar();
            c2.Ocultar();

            return anterior;
        }

        public bool Termino => Tablero.TableroCompleto();

        // Cierra la partida: detiene el cronómetro y arma el registro de puntaje final
        public Puntaje Finalizar()
        {
            Cronometro.Pausar();
            int puntuacion = Contador.CalcularPuntuacion(Cronometro.SegundosTranscurridos);

            var puntaje = new Puntaje(
                NombreJugador, DateTime.Now, NivelActual.Nombre,
                Contador.Intentos, Contador.Aciertos, Contador.Fallos,
                Cronometro.TiempoTranscurrido, puntuacion);

            GestorArchivos.RegistrarLog($"Partida finalizada: {puntaje}");
            return puntaje;
        }
    }
}
