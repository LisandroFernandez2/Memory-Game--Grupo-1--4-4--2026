using Microsoft.Xna.Framework.Graphics;

namespace juegojuego
{
    public class ModoInfinito : ModoJuego
    {
        public ModoInfinito()
            : base("Infinito", "Cada tablero que completas trae otro mas grande. Hasta donde llegas?")
        {
        }
      // Variable que determina el nivel de la ronda
        private int _ronda;
      // cuando empieza, la ronda es 1
        public override void AlEmpezarJuego() => _ronda = 1;
      // al terminar la ronda, se suma 1 a la variable _ronda y se crea una nueva variable Nivel con 2 filas y 2 columnas mas de cartas
        public override Nivel SiguienteNivel(Nivel Actual) {
            _ronda++;
            return new Nivel(
                     "Ronda " + _ronda,
                    Actual.Filas + 2,
                    Actual.Columnas + 2
            );
        }
       // si fallo 10 veces, entonces perdio
        public override bool Perdio(Partida partida)
        {
            return partida.Contador.Fallos >= 10;
        }
       // muestra en que ronda esta el jugador
        public override string[] LineasInfo(Partida partida)
        {
            return new[] { $"RONDA " + _ronda };
        }
        // al perder, se muestra hasta que ronda llego el usuario
        public override string MotivoDerrota => "LLEGASTE A LA RONDA " + _ronda;
        // la funcion Disponible pasa a ser true (el modo ya esta disponible)
        public override bool Disponible => true;

        // Un solo nivel de arranque: los siguientes los entrega SiguienteNivel
        public override Nivel[] Niveles => new Nivel[] { new Nivel("Ronda 1", 4, 4) };
    }
}
