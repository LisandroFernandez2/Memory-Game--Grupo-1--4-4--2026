using System;

namespace juegojuego.BasedeDatos
{
    // Tabla "movimiento": un intento (dos cartas dadas vuelta) de una partida.
    public class MovimientoDB
    {
        public int Id { get; set; }
        public int PartidaId { get; set; }
        // Orden del intento dentro de la partida (1, 2, 3...)
        public int Numero { get; set; }
        public int Fila1 { get; set; }
        public int Columna1 { get; set; }
        public int Fila2 { get; set; }
        public int Columna2 { get; set; }
        public bool Acierto { get; set; }
        public DateTime Momento { get; set; }

        public PartidaDB Partida { get; set; }
    }
}
