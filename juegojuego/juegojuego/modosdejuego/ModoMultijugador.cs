using Microsoft.Xna.Framework;

namespace juegojuego
{
    // MODO MULTIJUGADOR: 2 jugadores por turnos en la misma computadora.
    // Si un jugador acierta sigue jugando; si falla le toca al otro. Gana quien encuentra mas pares.
    public class ModoMultijugador : ModoJuego
    {
        // Un color para cada jugador (alcanza para hasta 4)
        private static readonly Color[] Colores = { Color.DeepSkyBlue, Color.OrangeRed, Color.LimeGreen, Color.Yellow };

        public ModoMultijugador()
            : base("Multijugador", "2,3 0 4 jugadores por turnos: gana quien encuentra mas pares")
        {
        }

        // Si el jugador acierta, sigue su turno. Si falla, el turno pasa al siguiente jugador
        public override void AlCompletarIntento(Partida partida, bool acierto)
        {
            if (!acierto)
            {
                partida.PasarTurno();
            }
        }

        // Una linea por jugador con sus pares; el ">" marca a quien le toca
        public override string[] LineasInfo(Partida partida)
        {
            string[] lineas = new string[partida.Jugadores.Count];
            for (int i = 0; i < lineas.Length; i++)
            {
                Jugador j = partida.Jugadores[i];
                int intentos = j.aciertos + j.fallos;
                string marca = i == partida.TurnoActual ? "> " : "  ";
                lineas[i] = $"{marca} {j.nombre}  PARES: {j.aciertos}  INTENTOS: {intentos}";
            }
            return lineas;
        }

        // Cada linea se pinta con el color de su jugador (el indice de la linea es el del jugador)
        public override Color ColorLineaInfo(Partida partida, int indice)
        {
            return Colores[indice % Colores.Length];
        }

        // Muestra el ganador de la partida, el tiempo y los intentos
        public override string TituloVictoria(Partida partida)
        {
            Jugador jugador1 = partida.Jugadores[0];
            Jugador jugador2 = partida.Jugadores[1];
            Jugador jugador3 = partida.Jugadores[2];
            Jugador jugador4 = partida.Jugadores[3];
            if (jugador1.aciertos > jugador2.aciertos && jugador1.aciertos > jugador3.aciertos && jugador1.aciertos > jugador4.aciertos)
            {
                return $"GANO pepe {jugador1.nombre}";
            }
            else if (jugador2.aciertos > jugador1.aciertos && jugador2.aciertos > jugador3.aciertos && jugador2.aciertos > jugador4.aciertos)
            {
                return $"GANO pedro {jugador2.nombre}";
            }
            if (jugador3.aciertos > jugador2.aciertos && jugador3.aciertos > jugador1.aciertos && jugador3.aciertos > jugador4.aciertos)
            {
                return $"GANO juan {jugador3.nombre}";
            }
            if (jugador4.aciertos > jugador2.aciertos && jugador4.aciertos > jugador3.aciertos && jugador4.aciertos > jugador1.aciertos)
            {
                return $"GANO carlos {jugador4.nombre}";
            }
            else
            {
                return "empate";
            }

        }

        // Juego disponible
        public override bool Disponible => true;

        // Cuantos jugadores comparten la partida
        public override int CantidadJugadores => 4;

        // Sin tiempo limite: lo importante es quien junta mas pares
        public override Nivel[] Niveles => Nivel.TamanosEstandar;

        // El puntaje de una partida de varios jugadores no va a la tabla de mejores puntajes (que es de uno solo)
        public override bool GuardaRecords => false;
    }
}