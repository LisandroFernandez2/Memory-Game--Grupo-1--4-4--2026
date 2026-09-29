namespace juegojuego
{
    // Tipo de evento que puede ocurrir sobre una carta durante la animación
    public enum TipoEvento
    {
        Voltear,
        Ocultar,
        MarcarEmparejada
    }

    // Evento individual que representa una acción de animación pendiente sobre
    // una carta del tablero (por ejemplo: voltearla, ocultarla o marcarla como
    // emparejada). Se procesan en el orden en que se generaron
    public class EventoVolteo
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public TipoEvento Tipo { get; set; }

        public EventoVolteo(int fila, int columna, TipoEvento tipo)
        {
            Fila = fila;
            Columna = columna;
            Tipo = tipo;
        }
    }
}
