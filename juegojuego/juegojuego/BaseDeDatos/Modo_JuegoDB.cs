using System.Collections.Generic;

namespace juegojuego.BasedeDatos
{
    // Tabla "modo_juego": las REGLAS del juego (la clase ModoJuego: Clasico, Cronometro, etc.).
    public class Modo_JuegoDB
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public bool Disponible { get; set; }
        public int CantidadJugadores { get; set; } = 1;
        public bool GuardaRecords { get; set; } = true;

        // Niveles que ofrece este modo
        public List<NivelDB> Niveles { get; set; } = new List<NivelDB>();
    }
}
