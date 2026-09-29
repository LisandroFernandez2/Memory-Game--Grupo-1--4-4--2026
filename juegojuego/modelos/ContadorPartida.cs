namespace juegojuego
{
    // Lleva la cuenta de intentos, aciertos y fallos de la partida en curso,
    // y calcula la puntuación resultante.
    public class ContadorPartida
    {
        public int Intentos { get; private set; }
        public int Aciertos { get; private set; }
        public int Fallos { get; private set; }

        public void RegistrarIntento(bool fueAcierto)
        {
            Intentos++;
            if (fueAcierto)
                Aciertos++;
            else
                Fallos++;
        }

        public void Reiniciar()
        {
            Intentos = 0;
            Aciertos = 0;
            Fallos = 0;
        }

        // Puntuación simple: cada acierto suma, cada fallo resta un poco,
        // y se bonifica terminar en menos tiempo.
        public int CalcularPuntuacion(int segundosTranscurridos)
        {
            int puntos = (Aciertos * 100) - (Fallos * 10);
            int bonoTiempo = System.Math.Max(0, 300 - segundosTranscurridos);
            return System.Math.Max(0, puntos + bonoTiempo);
        }
    }
}
