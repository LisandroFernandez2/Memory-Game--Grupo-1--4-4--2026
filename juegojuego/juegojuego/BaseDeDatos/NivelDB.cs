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

        // Solo lectura: se calcula aca (filas * columnas / 2) y EF no la guarda.
        // En la base la calcula MySQL, asi que nunca se asigna desde C#
        public int Pares => Filas * Columnas / 2;

        // Solo NivelCronometro (modos Cronometro y Contrarreloj): segundos limite. null en los demas
        public int? TiempoLimite { get; set; }
        // Solo NivelSupervivencia: vidas iniciales. null en los demas
        public int? Vidas { get; set; }

        public Modo_JuegoDB ModoJuego { get; set; }
    }
}
