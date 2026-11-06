using System;

namespace juegojuego.BasedeDatos
{
    // Tabla "record": partidas ganadas de modos que guardan records.
    // No se inserta desde C#: la llena sola un trigger de MySQL al guardar una PartidaDB ganada.
    // Jugador, Modo, Nivel y Tematica se guardan como texto (copia) para que el ranking se lea sin JOINs.
    public class RecordDB
    {
        public int Id { get; set; }
        public int PartidaId { get; set; }
        public int JugadorId { get; set; }
        public string Jugador { get; set; } = "";
        public string Modo { get; set; } = "";
        public string Nivel { get; set; } = "";
        public string Tematica { get; set; } = "";
        public int Intentos { get; set; }
        public int Aciertos { get; set; }
        public int Fallos { get; set; }
        public int TiempoSegundos { get; set; }
        public int Puntuacion { get; set; }
        public DateTime Fecha { get; set; }

        public PartidaDB Partida { get; set; }
    }
}
