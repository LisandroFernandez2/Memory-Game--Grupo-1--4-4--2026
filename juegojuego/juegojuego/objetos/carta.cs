using System;

namespace juegojuego
{
    // Representa una carta individual del tablero.
    // Cada carta tiene una cara oculta y una cara visible,
    // un identificador de par que permite saber qué dos cartas hacen pareja,
    // y su posición dentro del arreglo 2D del tablero.
    // Clase que representa UNA carta del tablero
    public class carta
    {
        // Imagen del dorso cuando esta boca abajo
        public Sprite cubierta { get; set; }
        // Imagen de la cara cuando esta boca arriba
        public Sprite descubierta { get; set; }
        // true si esta dada vuelta en este momento
        public bool estaDescubierta { get; set; }
        // true si ya encontraste su pareja queda fija y no se puede tocar
        public bool estaEmparejada { get; set; }

        // Identificador del valor/símbolo de la carta. Dos cartas con el mismo
        // idValor forman un par. Se usa tanto para la generación del tablero
        // como para la búsqueda secuencial y la verificación de coincidencias.
        public int idValor { get; set; }

        // Posición de la carta dentro del arreglo 2D del tablero.
        // Numero de fila en el tablero empieza en 0
        public int fila { get; set; }
        // Numero de columna en el tablero empieza en 0
        public int columna { get; set; }

        // Constructor
        // Recibe: cubierta y descubierta sprites estaDescubierta y estaEmparejada estado inicial e idValor fila y columna opcionales
        // Devuelve: nada crea la carta Solo guarda los valores
        public carta(Sprite cubierta, Sprite descubierta, bool estaDescubierta, bool estaEmparejada, int idValor = -1, int fila = 0, int columna = 0)
        {
            this.cubierta = cubierta;
            this.descubierta = descubierta;
            this.estaDescubierta = estaDescubierta;
            this.estaEmparejada = estaEmparejada;
            this.idValor = idValor;
            this.fila = fila;
            this.columna = columna;
        }

        // Voltea la carta hacia arriba
        // Recibe: nada Devuelve: nada
        // La da vuelta salvo que ya este emparejada
        public void Voltear()
        {
            if (!estaEmparejada)
                estaDescubierta = true;
        }

        // Oculta nuevamente la carta
        // Recibe: nada Devuelve: nada
        // La tapa de nuevo salvo que ya este emparejada
        public void Ocultar()
        {
            if (!estaEmparejada)
                estaDescubierta = false;
        }
    }
}
