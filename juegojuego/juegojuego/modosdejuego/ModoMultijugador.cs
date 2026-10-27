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

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => false;

        // Cuantos jugadores comparten la partida
        public override int CantidadJugadores => 2;

        // Sin tiempo limite: lo importante es quien junta mas pares
        public override Nivel[] Niveles => Nivel.TamanosEstandar;

        // El puntaje de una partida de varios jugadores no va a la tabla de mejores puntajes (que es de uno solo)
        public override bool GuardaRecords => false;
    }
}
