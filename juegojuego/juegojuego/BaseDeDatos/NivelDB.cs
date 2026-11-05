namespace juegojuego.BasedeDatos
{
    // Tabla "nivel": un tablero de un modo de juego (Nivel, NivelCronometro y NivelSupervivencia del juego).
    public class NivelDB
    {
        public int Id { get; set; }
        public int ModoJuegoId { get; set; }
        public string Nombre { get; set; } = "";
        public int Filas { get; set; }
        public int Columnas { get; set; }

        // Columna calculada por MySQL (filas * columnas / 2): nunca se asigna desde C#
        public int Pares { get; set; }

        // Solo NivelCronometro (modos Cronometro y Contrarreloj): segundos limite. null en los demas
        public int? TiempoLimite { get; set; }
        // Solo NivelSupervivencia: vidas iniciales. null en los demas
        public int? Vidas { get; set; }

        public Modo_JuegoDB ModoJuego { get; set; }
    }
}
