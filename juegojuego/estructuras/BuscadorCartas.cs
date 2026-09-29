namespace juegojuego
{
    // ALGORITMO: Búsqueda secuencial
    // Recorre el tablero (array 2D) casilla por casilla para localizar cartas
    // pendientes de emparejar o una carta específica por su valor. Es adecuada
    // aquí porque el tablero es pequeño (máximo 36 cartas en un 6x6), por lo que
    // no se justifica una estructura de búsqueda más compleja
    public static class BuscadorCartas
    {
        // Busca, recorriendo el tablero de forma secuencial, la primera carta que
        // todavía no fue emparejada. Devuelve null si no queda ninguna.
        public static carta BuscarCartaPendiente(tablero t)
        {
            for (int f = 0; f < t.filas; f++)
            {
                for (int c = 0; c < t.columnas; c++)
                {
                    carta actual = t.cartas[f, c];
                    if (actual != null && !actual.estaEmparejada)
                        return actual;
                }
            }
            return null;
        }

        // Busca de forma secuencial la carta "pareja" de un valor dado, excluyendo
        // la posición de origen (util para validar coincidencias o dar pistas)
        public static carta BuscarParPorValor(tablero t, int idValor, int filaOrigen, int columnaOrigen)
        {
            for (int f = 0; f < t.filas; f++)
            {
                for (int c = 0; c < t.columnas; c++)
                {
                    carta actual = t.cartas[f, c];
                    bool esLaMisma = f == filaOrigen && c == columnaOrigen;

                    if (actual != null && actual.idValor == idValor && !esLaMisma && !actual.estaEmparejada)
                        return actual;
                }
            }
            return null;
        }

        // Cuenta, recorriendo el tablero, cuantas cartas quedan sin emparejar
        public static int ContarCartasPendientes(tablero t)
        {
            int contador = 0;
            for (int f = 0; f < t.filas; f++)
                for (int c = 0; c < t.columnas; c++)
                    if (t.cartas[f, c] != null && !t.cartas[f, c].estaEmparejada)
                        contador++;

            return contador;
        }
    }
}
