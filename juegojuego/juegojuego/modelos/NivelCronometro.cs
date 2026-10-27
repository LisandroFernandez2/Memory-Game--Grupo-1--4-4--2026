namespace juegojuego
{
    // Un nivel con CRONOMETRO: tablero + tiempo limite. Es el nivel del modo "Nivel Cronometro".
    // A mayor dificultad, mas pares y menos tiempo.
    // Para crear un nivel nuevo: new NivelCronometro("Nombre", filas, columnas, segundos)
    // OJO: filas x columnas tiene que ser PAR (cada par ocupa 2 casillas)
    public class NivelCronometro : Nivel
    {
        // Tiempo maximo para ganar en segundos
        public int TiempoLimiteSegundos { get; set; }

        // Constructor
        // Recibe: nombre filas columnas y tiempoLimiteSegundos
        // Devuelve: nada crea el nivel Solo guarda los valores
        public NivelCronometro(string nombre, int filas, int columnas, int tiempoLimiteSegundos)
            : base(nombre, filas, columnas)
        {
            TiempoLimiteSegundos = tiempoLimiteSegundos;
        }

        // En el boton del menu se ve el tamano y el tiempo ej: 4X5  90S
        public override string Detalle => $"{Filas}X{Columnas}  {TiempoLimiteSegundos}S";

        // Niveles ya armados para usar directo ej: NivelCronometro.Facil
        // Formato: nombre filas columnas segundos
        // 4x4 = 16 cartas = 8 pares 120 s
        public static NivelCronometro Muy_Facil = new NivelCronometro("Muy Facil", 4, 4, 120);
        // 4x5 = 20 cartas = 10 pares 90 s
        public static NivelCronometro Facil = new NivelCronometro("Facil", 4, 5, 90);
        // 6x6 = 36 cartas = 18 pares 90 s
        public static NivelCronometro Normal = new NivelCronometro("Normal", 6, 6, 90);
        // 7x8 = 56 cartas = 28 pares 150 s
        public static NivelCronometro Dificil = new NivelCronometro("Dificil", 7, 8, 150);
        // 8x9 = 72 cartas = 36 pares 170 s
        public static NivelCronometro MuyDificil = new NivelCronometro("Muy Dificil", 8, 9, 170);
        // 9x10 = 90 cartas = 45 pares 220 s
        public static NivelCronometro Imposible = new NivelCronometro("Imposible", 9, 10, 220);

        // Los 6 niveles de siempre, en orden. Es la lista que ofrece el modo Nivel Cronometro
        // (tiene que ir DESPUES de los niveles de arriba)
        public static NivelCronometro[] Estandar = { Muy_Facil, Facil, Normal, Dificil, MuyDificil, Imposible };
    }
}
