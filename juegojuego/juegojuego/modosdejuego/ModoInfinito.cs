namespace juegojuego
{
    // MODO INFINITO (todavia no disponible): rondas sin fin. Cada tablero que se completa trae otro mas grande
    // y el juego sigue hasta que el jugador pierde.
    //
    // PARA TERMINARLO (los metodos estan explicados en ModoJuego.cs):
    //   - Agregar un campo para la ronda actual:  private int _ronda;
    //   - AlEmpezarJuego():  _ronda = 1;
    //   - SiguienteNivel(Nivel actual):  sumar 1 a _ronda y devolver un tablero un poco mas grande.
    //         OJO: filas x columnas tiene que ser PAR. Si el tablero necesita mas caras de las que tiene la tematica,
    //         el juego no sigue y sale el cartel de victoria (lo controla Game1).
    //   - Perdio(Partida partida):  decidir cuando termina, ej: partida.Contador.Fallos >= 10
    //   - LineasInfo(Partida partida):  mostrar la ronda, ej: return new[] { $"RONDA {_ronda}" };
    //   - MotivoDerrota:  ej: "LLEGASTE A LA RONDA ..." (hay que armarlo con la ronda)
    //   - Cuando este listo: borrar la linea de Disponible (o poner true)
    public class ModoInfinito : ModoJuego
    {
        public ModoInfinito()
            : base("Infinito", "Cada tablero que completas trae otro mas grande. Hasta donde llegas?")
        {
        }

        // Todavia no se puede elegir: aparece en la lista como PROXIMAMENTE
        public override bool Disponible => true;

        // Un solo nivel de arranque: los siguientes los entrega SiguienteNivel
        public override Nivel[] Niveles => new Nivel[] { new Nivel("Ronda 1", 1, 2) };

        // Variable que determina el nivel de la ronda
        public int _ronda;
        // cuando empieza, la ronda es 1
        public override void AlEmpezarJuego() => _ronda = 1;
        // al terminar la ronda, se suma 1 a la variable _ronda y se crea una nueva variable Nivel con 2 filas y 2 columnas mas de cartas
        public override Nivel SiguienteNivel(Nivel Actual)
        {
            _ronda++;
            if (Actual.Columnas > Actual.Filas)
            {
                return new Nivel(
                 "Ronda " + _ronda,
                 Actual.Filas + 1,
                 Actual.Columnas
                );
            }
            else
            {
                return new Nivel(
                    "Ronda " + _ronda,
                    Actual.Filas,
                    Actual.Columnas + 2
                );
            }
        }
        // si fallo 10 veces, entonces perdio
        public override bool Perdio(Partida partida)
        {
            return partida.JugadorActual.fallos > 15;
        }
        // muestra en que ronda esta el jugador
        public override string[] LineasInfo(Partida partida)
        {
            return new[] { $"RONDA: {_ronda}  Fallas:{partida.JugadorActual.fallos}" };
        }
        // al perder, se muestra hasta que ronda llego el usuario
        public override string MotivoDerrota => "LLEGASTE A LA RONDA " + _ronda;
    }
}
