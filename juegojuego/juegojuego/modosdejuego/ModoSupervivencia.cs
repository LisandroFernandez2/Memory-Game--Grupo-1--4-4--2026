namespace juegojuego
{
    // MODO SUPERVIVENCIA: el jugador tiene 3 vidas y cada intento fallido le quita una.
    // Sin cronometro: si se queda sin vidas pierde, si encuentra todos los pares gana.
    public class ModoSupervivencia : ModoJuego
    {
        public ModoSupervivencia()
            : base("Supervivencia", "Tenes 3 vidas: cada intento fallido te quita una")
        {
        }

        // variable que determinan el modo de juego:

        // vidas del jugador
        private int _vidas;
        private int _contador;

        // el jugador empieza el juego con 3 vidas (_vidas = 3) y empieza en la ronda 1 (_ronda = 1)  
        public override void AlEmpezarJuego() => _vidas = 3;

        // si cuando el jugador da vuelta 2 cartas pero no acertó, _vidas se le resta 1. Si acierta, se le suma 1 a _vidas
        public override void AlCompletarIntento(Partida partida, bool acierto) 
        {
            if (!acierto) _vidas--;
            else if (acierto) _vidas++; _contador++;
        }

        // si _vidas es 0, el jugador pierde porque no tiene más vidas
        public override bool Perdio(Partida partida) 
        { 
            return _vidas <= 0; 
        }

        // informacción de cómo está transcurriendo la partida
        public override string[] LineasInfo(Partida partida) 
        { 
            return new[] { $"VIDAS {_vidas}"}; 
        }

        // mensaje con el motivo del cual perdió la partida
        public override string MotivoDerrota => $"TE HAS QUEDADO SIN VIDAS, CANTIDAD DE ACIERTOS {_contador}";

        // juego listo para elegir en el menú
        public override bool Disponible => true;

        // un tablero 8X8
        public override Nivel[] Niveles => new Nivel[] { new Nivel("", 8, 8) };

    }
}
