using System.Security;

namespace juegojuego
{
    // Define los niveles de dificultad del juego: tamaño del tablero
    // y tiempo límite disponible para resolverlo. A mayor dificultad, más pares
    // y menos tiempo, tal como pide la descripción del juego
    public class Nivel
    {
        public string Nombre { get; set; }
        public int Filas { get; set; }
        public int Columnas { get; set; }
        public int TiempoLimiteSegundos { get; set; }

        public Nivel(string nombre, int filas, int columnas, int tiempoLimiteSegundos)
        {
            Nombre = nombre;
            Filas = filas;
            Columnas = columnas;
            TiempoLimiteSegundos = tiempoLimiteSegundos;
        }

        public static Nivel MuyFacil => new Nivel("Muy Facil", 4, 4, 120);
        public static Nivel Facil => new Nivel("Facil", 4, 5, 90);
        public static Nivel Normal => new Nivel("Normal", 6, 6, 90);
        public static Nivel Dificil => new Nivel("Dificil", 7, 8, 150);
        public static Nivel MuyDificil => new Nivel("Muy Dificil", 8, 9, 170);
        public static Nivel Imposible => new Nivel("Imposible", 9, 10, 220);
    }
}
