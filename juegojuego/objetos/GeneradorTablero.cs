using System;
using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA/ALGORITMO: Recursividad.
    // Se encarga de generar la distribución aleatoria de pares de cartas dentro del
    // tablero, garantizando que cada valor aparezca exactamente dos veces
    // y sin repetir una posición ya ocupada. Se modela como un proceso recursivo:
    // en cada llamada se coloca una carta en una posición libre y se invoca la función
    // nuevamente para la siguiente posición, hasta agotar las casillas
    public static class GeneradorTablero
    {
        private static readonly Random rng = new Random();

        // Genera y coloca las cartas dentro del tablero de forma recursiva.
        public static void GenerarTablero(tablero t, Func<int, (Sprite cubierta, Sprite descubierta)> fabricarSprites)
        {
            int totalCartas = t.filas * t.columnas;
            int totalPares = totalCartas / 2;

            // 1) Generar la "bolsa" de valores: cada id de par aparece dos veces.
            List<int> valores = GenerarBolsaDeValores(totalPares);

            // 2) Generar las posiciones libres (fila, columna) como lista de coordenadas.
            List<(int fila, int col)> posiciones = new List<(int, int)>();
            for (int f = 0; f < t.filas; f++)
                for (int c = 0; c < t.columnas; c++)
                    posiciones.Add((f, c));

            // 3) Asignación recursiva sin repetición: coloca un valor en una posición
            //    aleatoria libre y avanza recursivamente hasta que no queden posiciones.
            AsignarRecursivo(t, valores, posiciones, fabricarSprites);
        }

        // Caso recursivo: toma una posición libre al azar, le asigna el último valor
        // disponible de la bolsa, remueve ambos de las listas y se llama a sí misma
        // con las listas reducidas. Caso base: no quedan posiciones libres.
        private static void AsignarRecursivo(tablero t, List<int> valoresDisponibles,
            List<(int fila, int col)> posicionesLibres, Func<int, (Sprite cubierta, Sprite descubierta)> fabricarSprites)
        {
            // Caso base de la recursividad.
            if (posicionesLibres.Count == 0)
                return;

            // Elegimos una posición libre al azar para no dejar el tablero ordenado.
            int idxPos = rng.Next(posicionesLibres.Count);
            var (fila, col) = posicionesLibres[idxPos];
            posicionesLibres.RemoveAt(idxPos);

            int idxVal = rng.Next(valoresDisponibles.Count);
            int valor = valoresDisponibles[idxVal];
            valoresDisponibles.RemoveAt(idxVal);

            var sprites = fabricarSprites(valor);
            carta nueva = new carta(sprites.cubierta, sprites.descubierta, false, false, valor, fila, col);
            t.ColocarCarta(fila, col, nueva);

            // Llamada recursiva para continuar con la siguiente posición.
            AsignarRecursivo(t, valoresDisponibles, posicionesLibres, fabricarSprites);
        }

        // Crea la lista [0,0,1,1,2,2,...] con cada valor repetido dos veces.
        private static List<int> GenerarBolsaDeValores(int totalPares)
        {
            var valores = new List<int>();
            for (int i = 0; i < totalPares; i++)
            {
                valores.Add(i);
                valores.Add(i);
            }
            return valores;
        }
    }
}
