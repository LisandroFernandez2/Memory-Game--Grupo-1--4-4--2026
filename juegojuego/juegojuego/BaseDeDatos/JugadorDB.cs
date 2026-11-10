using System.Collections.Generic;

namespace juegojuego.BasedeDatos
{
    // Tabla "jugador": una fila por cada nombre de jugador distinto.
    public class JugadorDB
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";

        // Partidas jugadas por este jugador
        public List<PartidaDB> Partidas { get; set; } = new List<PartidaDB>();
    }
}
