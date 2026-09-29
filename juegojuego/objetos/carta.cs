using System;

namespace juegojuego
{
    // Representa una carta individual del tablero.
    // Cada carta tiene una cara oculta y una cara visible,
    // un identificador de par que permite saber qué dos cartas hacen pareja,
    // y su posición dentro del arreglo 2D del tablero.
    public class carta
    {
        public Sprite cubierta { get; set; }
        public Sprite descubierta { get; set; }
        public bool estaDescubierta { get; set; }
        public bool estaEmparejada { get; set; }

        // Identificador del valor/símbolo de la carta. Dos cartas con el mismo
        // idValor forman un par. Se usa tanto para la generación del tablero
        // como para la búsqueda secuencial y la verificación de coincidencias.
        public int idValor { get; set; }

        // Posición de la carta dentro del arreglo 2D del tablero.
        public int fila { get; set; }
        public int columna { get; set; }

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
        public void Voltear()
        {
            if (!estaEmparejada)
                estaDescubierta = true;
        }

        // Oculta nuevamente la carta
        public void Ocultar()
        {
            if (!estaEmparejada)
                estaDescubierta = false;
        }

        public override string ToString()
        {
            return $"Carta[{fila},{columna}] valor={idValor} descubierta={estaDescubierta} emparejada={estaEmparejada}";
        }
    }
}
