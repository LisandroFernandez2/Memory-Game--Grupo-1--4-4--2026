using juegojuego.modelos;
using System;


namespace juegojuego
{
    // MODO SUPERVIVENCIA: el jugador tiene 3 vidas y cada intento fallido le quita una.
    // Sin cronometro: si se queda sin vidas pierde, si encuentra todos los pares gana.
    public class ModoSupervivencia : ModoJuego
    {
        public ModoSupervivencia()
            : base("Supervivencia", "Tenes una cantidad limitada vidas: cada intento fallido te quita una")
        {
        }


        // Game1 lo llama cada vez que se completa un intento (2 cartas dadas vuelta)
        // Si fallo resta una vida
        public override void AlCompletarIntento(Partida partida, bool acierto)
        {
            if (!acierto) partida.Vidas.vidas--;
            else partida.Vidas.vidas++;
        }

        // Game1 lo llama en cada cuadro: pierde cuando no quedan vidas
        public override bool Perdio(Partida partida)
        {
            return partida.NivelActual is NivelSupervivencia && partida.Vidas.SeAgotoLasVidas();
        }

        // Texto grande de arriba a la izquierda
        public override string TextoTiempo(Partida partida)
        {
            return $"VIDAS {Math.Max(0, partida.Vidas.vidas)}";
        }

        // Se pone en rojo cuando queda 1 vida o menos
        public override bool TiempoUrgente(Partida partida) => partida.Vidas.vidas <= 1;

        // Cartel cuando se pierde
        public override string MotivoDerrota => "TE HAS QUEDADO SIN VIDAS";

        // Modo listo para elegir en el menu
        public override bool Disponible => true;

        // Niveles que ofrece este modo
        public override Nivel[] Niveles => NivelSupervivencia.Estandar;
    }
}