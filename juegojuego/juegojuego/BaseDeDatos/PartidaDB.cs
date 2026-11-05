using System;
using System.Collections.Generic;

namespace juegojuego.BasedeDatos
{
    // Tabla "partida": una partida terminada (ganada o perdida).
    // El modo de juego se obtiene por Nivel.ModoJuego (no se repite aca).
    // Los records NO tienen tabla propia: son las partidas ganadas de modos con GuardaRecords, ordenadas por puntaje.
    public class PartidaDB
    {
        public int Id { get; set; }
        public int JugadorId { get; set; }
        public int NivelId { get; set; }
        public int TematicaId { get; set; }
        public DateTime Fecha { get; set; }
        public int Intentos { get; set; }
        public int Aciertos { get; set; }
        public int Fallos { get; set; }
        public int TiempoSegundos { get; set; }
        public int Puntuacion { get; set; }
        public bool Gano { get; set; }

        public JugadorDB Jugador { get; set; }
        public NivelDB Nivel { get; set; }
        public TematicaDB Tematica { get; set; }

        public List<MovimientoDB> Movimientos { get; set; } = new List<MovimientoDB>();
        public List<Par_descubiertoDB> ParesDescubiertos { get; set; } = new List<Par_descubiertoDB>();
    }
}
