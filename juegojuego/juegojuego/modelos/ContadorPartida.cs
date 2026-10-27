namespace juegojuego
{
    // Lleva la cuenta de intentos, aciertos y fallos de la partida en curso,
    // y calcula la puntuación resultante.
    // Clase que cuenta los intentos aciertos y fallos de la partida
    public class ContadorPartida
    {
        // Total de intentos cada vez que das vuelta 2 cartas Solo se modifica desde esta clase
        public int Intentos { get; set; }
        // Cuantos intentos fueron pares correctos
        public int Aciertos { get; set; }
        // Cuantos intentos no coincidieron
        public int Fallos { get; set; }

        // Recibe: fueAcierto true si las 2 cartas coincidieron
        // Devuelve: nada
        // Como funciona: suma 1 a Intentos y ademas 1 a Aciertos o a Fallos
        public void RegistrarIntento(bool fueAcierto)
        {
            Intentos++;
            if (fueAcierto)
                Aciertos++;
            else
                Fallos++;
        }

        // Recibe: nada Devuelve: nada
        // Pone todos los contadores en 0
        public void Reiniciar()
        {
            Intentos = 0;
            Aciertos = 0;
            Fallos = 0;
        }

        // Puntuación simple: cada acierto suma, cada fallo resta un poco,
        // y se bonifica terminar en menos tiempo.
        // Recibe: segundosTranscurridos int
        // Devuelve: la puntuacion final int nunca menor a 0
        // Formula: aciertos x 100 - fallos x 10 + bonus por tiempo
        public int CalcularPuntuacion(int segundosTranscurridos)
        {
            // +100 por cada acierto -10 por cada fallo
            int puntos = (Aciertos * 100) - (Fallos * 10);
            // Bonus de tiempo: 300 menos los segundos usados si pasas de 300 s el bonus es 0
            int bonoTiempo = System.Math.Max(0, 300 - segundosTranscurridos);
            // Suma todo y evita que el resultado sea negativo
            return System.Math.Max(0, puntos + bonoTiempo);
        }
    }
}
