namespace juegojuego
{
    // MODO MULTIJUGADOR (todavia no disponible): 2 jugadores por turnos en la misma computadora.
    // Si un jugador acierta sigue jugando; si falla le toca al otro. Gana quien encuentra mas pares.
    //
    // La base ya esta hecha: la Partida tiene la lista Jugadores (cada uno con sus Aciertos y Fallos),
    // el TurnoActual, el JugadorActual y el metodo PasarTurno().
    //
    // PARA TERMINARLO (los metodos estan explicados en ModoJuego.cs):
    //   - AlCompletarIntento(Partida partida, bool acierto):  if (!acierto) partida.PasarTurno();
    //   - LineasInfo(Partida partida):  mostrar de quien es el turno y los pares de cada uno,
    //         ej: partida.JugadorActual.Nombre y partida.Jugadores[0].Aciertos
    //   - TituloVictoria(Partida partida):  comparar los Aciertos de los jugadores y decir quien gano (o EMPATE)
    //   - Para 3 o 4 jugadores alcanza con cambiar CantidadJugadores
    //   - Cuando este listo: borrar la linea de Disponible (o poner true)
    public class ModoMultijugador : ModoJuego
    {
        public ModoMultijugador()
            : base("Multijugador", "2 jugadores por turnos: gana quien encuentra mas pares")
        {
        }

        // Si el jugador acierta, sigue su turno. Si falla el jugador, el turno pasa al siguiente jugador
        public override void AlCompletarIntento(Partida partida, bool acierto) 
        {
            if(!acierto) 
            {
                partida.PasarTurno(); 
            } 
        }

        // Informa el turno del jugador y los pares que tiene
        public override string[] LineasInfo(Partida partida)
        {
            return new[] { $"Turno del jugador {partida.JugadorActual.Nombre}, cantidad de pares {partida.Jugadores[0].Aciertos}" };
        }


        // Muestra el ganador de la partida, el tiempo y los intentos
        public override string TituloVictoria(Partida partida)
        {
            return base.TituloVictoria(partida);
        }

        // Juego disponible
        public override bool Disponible => true;

        // Cuantos jugadores comparten la partida
        public override int CantidadJugadores => 2;

        // Sin tiempo limite: lo importante es quien junta mas pares
        public override Nivel[] Niveles => Nivel.TamanosEstandar;

        // El puntaje de una partida de varios jugadores no va a la tabla de mejores puntajes (que es de uno solo)
        public override bool GuardaRecords => false;
    }
}
