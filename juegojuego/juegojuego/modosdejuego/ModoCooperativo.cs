namespace juegojuego
{
    // MODO COOPERATIVO (todavia no disponible): 2 jugadores en equipo contra el reloj.
    // Se turnan para dar vuelta las cartas y entre los dos tienen que encontrar todos los pares antes de que se acabe el tiempo.
    //
    // Hereda de ModoNivelCronometro para reutilizar el cronometro y los niveles con tiempo limite:
    // solo agrega que hay 2 jugadores y que se cambia el turno.
    //
    // PARA TERMINARLO (los metodos estan explicados en ModoJuego.cs):
    //   - AlCompletarIntento(Partida partida, bool acierto):  partida.PasarTurno();   (cambia el turno siempre)
    //   - LineasInfo(Partida partida):  mostrar de quien es el turno, ej: return new[] { partida.JugadorActual.Nombre };
    //   - Cuando este listo: borrar la linea de Disponible (o poner true)
    public class ModoCooperativo : ModoNivelCronometro
    {
        public ModoCooperativo()
            : base("Cooperativo", "2 jugadores en equipo contra el reloj")
        {
        }

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => false;

        // Cuantos jugadores comparten la partida
        public override int CantidadJugadores => 2;
    }
}
