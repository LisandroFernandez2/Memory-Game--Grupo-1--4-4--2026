namespace juegojuego.BasedeDatos
{
    // Tabla "par_descubierto": un par encontrado en una partida y los puntos que dio.
    public class Par_descubiertoDB
    {
        public int Id { get; set; }
        public int PartidaId { get; set; }
        // Mismo idValor de las cartas
        public int IdValor { get; set; }
        public int Puntos { get; set; }

        public PartidaDB Partida { get; set; }
    }
}
