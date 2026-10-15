namespace juegojuego
{
    // Tipo de evento que puede ocurrir sobre una carta durante la animación
    // Lista de acciones posibles enum Se usa asi: TipoEvento Voltear
    public enum TipoEvento
    {
        // Dar vuelta la carta para mostrar su cara
        Voltear,
        // Volver a taparla cuando fallas el par
        Ocultar,
        // Dejarla fija porque ya encontraste su pareja
        MarcarEmparejada
    }

    // Evento individual que representa una acción de animación pendiente sobre
    // una carta del tablero (por ejemplo: voltearla, ocultarla o marcarla como
    // emparejada). Se procesan en el orden en que se generaron
    // Clase que guarda QUE hay que hacerle a QUE carta: su posicion fila columna y la accion
    public class EventoVolteo
    {
        // Fila de la carta en el tablero empieza en 0
        public int Fila { get; set; }
        // Columna de la carta en el tablero empieza en 0
        public int Columna { get; set; }
        // Accion a aplicar: Voltear Ocultar o MarcarEmparejada
        public TipoEvento Tipo { get; set; }

        // Constructor
        // Recibe: fila columna y tipo la accion
        // Devuelve: nada crea el evento Solo guarda los valores recibidos
        public EventoVolteo(int fila, int columna, TipoEvento tipo)
        {
            Fila = fila;
            Columna = columna;
            Tipo = tipo;
        }
    }
}
