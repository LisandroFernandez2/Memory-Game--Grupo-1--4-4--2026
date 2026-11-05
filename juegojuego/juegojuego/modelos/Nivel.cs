namespace juegojuego
{
    // Un NIVEL es un tablero: nombre y cantidad de filas y columnas.
    // Esta es la clase BASE y NO tiene tiempo limite. Cada modo de juego usa el tipo de nivel que necesita:
    //     NivelCronometro   tablero + tiempo limite (lo usa el modo Nivel Cronometro)
    //     Nivel             tablero solo, sin tiempo (lo pueden usar el modo Clasico y otros)
    // Para crear un nivel nuevo sin tiempo: new Nivel("Nombre", filas, columnas)
    // OJO: filas x columnas tiene que ser PAR (cada par ocupa 2 casillas)
    public class Nivel
    {
        // Nombre del nivel que se muestra en pantalla (sin tildes: la fuente no las tiene)
        public string Nombre { get; set; }
        // Cantidad de filas del tablero
        public int Filas { get; set; }
        // Cantidad de columnas del tablero
        public int Columnas { get; set; }

        // Cantidad de pares de cartas que necesita el tablero
        // Tambien es la cantidad de caras distintas (imagenes) que necesita la tematica para poder jugarlo
        public int Pares => Filas * Columnas / 2;

        // Constructor
        // Recibe: nombre filas y columnas
        // Devuelve: nada crea el nivel Solo guarda los valores
        public Nivel(string nombre, int filas, int columnas)
        {
            Nombre = nombre;
            Filas = filas;
            Columnas = columnas;
        }

        // Texto corto con los datos del nivel que se muestra en su boton del menu ej: 4X5
        // Los niveles con mas datos (ej: tiempo limite) lo sobreescriben para agregarlos
        public virtual string Detalle => $"{Filas}X{Columnas}";

        // Los 6 tableros de siempre pero SIN tiempo limite
        // Sirve para modos que no tienen cronometro: public override Nivel[] Niveles => Nivel.TamanosEstandar;
        public static readonly Nivel[] TamanosEstandar =
        {
            // 4x4 = 16 cartas = 8 pares
            new Nivel("Muy Facil", 4, 4),
            // 4x5 = 20 cartas = 10 pares
            new Nivel("Facil", 4, 5),
            // 6x6 = 36 cartas = 18 pares
            new Nivel("Normal", 6, 6),
            // 7x8 = 56 cartas = 28 pares
            new Nivel("Dificil", 7, 8),
            // 8x9 = 72 cartas = 36 pares
            new Nivel("Muy Dificil", 8, 9),
            // 9x10 = 90 cartas = 45 pares
            new Nivel("Imposible", 9, 10),
        };
    }
}
