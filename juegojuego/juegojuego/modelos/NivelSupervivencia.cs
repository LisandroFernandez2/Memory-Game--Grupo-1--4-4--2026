namespace juegojuego
{
    // Un nivel de SUPERVIVENCIA: tablero + cantidad de vidas. Es el nivel del modo "Supervivencia".
    // A mayor dificultad, mas pares (y mas vidas para compensar).
    // Para crear un nivel nuevo: new NivelSupervivencia("Nombre", filas, columnas, vidas)
    // OJO: filas x columnas tiene que ser PAR (cada par ocupa 2 casillas)
    public class NivelSupervivencia : Nivel
    {
        // Vidas con las que arranca el jugador
        public int vidas { get; set; }
 
        public NivelSupervivencia(string nombre, int filas, int columnas, int vidas)
            : base(nombre, filas, columnas)
        {
            this.vidas = vidas;
        }
 
        // En el boton del menu se ve el tamano y las vidas ej: 4X5  6V
        public override string Detalle => $"{Filas}X{Columnas}  {vidas}V";
 
        // Formato: nombre filas columnas vidas (los numeros se pueden ajustar a gusto)
        public static NivelSupervivencia Muy_Facil = new NivelSupervivencia("Muy Facil", 4, 4, 6);
        public static NivelSupervivencia Facil = new NivelSupervivencia("Facil", 4, 5, 6);
        public static NivelSupervivencia Normal = new NivelSupervivencia("Normal", 6, 6, 8);
        public static NivelSupervivencia Dificil = new NivelSupervivencia("Dificil", 7, 8, 10);
        public static NivelSupervivencia MuyDificil = new NivelSupervivencia("Muy Dificil", 8, 9, 12);
        public static NivelSupervivencia Imposible = new NivelSupervivencia("Imposible", 9, 10, 15);
 
        // Los 6 niveles en orden (tiene que ir DESPUES de los niveles de arriba)
        public static NivelSupervivencia[] Estandar = { Muy_Facil, Facil, Normal, Dificil, MuyDificil, Imposible };
    }
}