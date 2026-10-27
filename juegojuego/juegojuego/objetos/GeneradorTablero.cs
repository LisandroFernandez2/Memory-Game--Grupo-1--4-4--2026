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
    // Clase estatica no se crea con new que arma el tablero con las cartas mezcladas al azar
    public static class GeneradorTablero
    {
        // Generador de numeros al azar
        private static readonly Random rng = new Random();

        // Genera y coloca las cartas dentro del tablero de forma recursiva.
        // Recibe: t el tablero vacio y fabricarSprites funcion que recibe el valor de una carta y devuelve sus sprites cubierta/descubierta
        // Devuelve: nada Llena el tablero directamente
        public static void GenerarTablero(tablero t, Func<int, (Sprite cubierta, Sprite descubierta)> fabricarSprites)
        {
            // Cantidad total de casillas del tablero
            int totalCartas = t.filas * t.columnas;
            // Cada par ocupa 2 casillas
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
        // Recibe: t tablero valoresDisponibles valores que todavia no se usaron posicionesLibres casillas vacias y fabricarSprites
        // Devuelve: nada
        // Como funciona: coloca una carta y se llama a si misma hasta llenar todas las casillas
        private static void AsignarRecursivo(tablero t, List<int> valoresDisponibles,
            List<(int fila, int col)> posicionesLibres, Func<int, (Sprite cubierta, Sprite descubierta)> fabricarSprites)
        {
            // Caso base de la recursividad.
            if (posicionesLibres.Count == 0)
                return;

            // Elegimos una posición libre al azar para no dejar el tablero ordenado.
            // Numero al azar para elegir una de las posiciones libres
            int idxPos = rng.Next(posicionesLibres.Count);
            // Toma esa posicion y la saca de las libres
            var (fila, col) = posicionesLibres[idxPos];
            posicionesLibres.RemoveAt(idxPos);

            // Elige un valor al azar de la bolsa y lo saca cada valor esta 2 veces
            int idxVal = rng.Next(valoresDisponibles.Count);
            int valor = valoresDisponibles[idxVal];
            valoresDisponibles.RemoveAt(idxVal);

            // Crea los sprites dorso y cara para ese valor
            var sprites = fabricarSprites(valor);
            // Crea la carta con su valor y su posicion
            carta nueva = new carta(sprites.cubierta, sprites.descubierta, false, false, valor, fila, col);
            // La guarda en el tablero
            t.ColocarCarta(fila, col, nueva);

            // Llamada recursiva para continuar con la siguiente posición.
            AsignarRecursivo(t, valoresDisponibles, posicionesLibres, fabricarSprites);
        }

        // Crea la lista [0,0,1,1,2,2,...] con cada valor repetido dos veces.
        // Recibe: totalPares int
        // Devuelve: lista con cada numero de 0 a totalPares-1 repetido 2 veces Ej: con 3 pares -> [0 0 1 1 2 2]
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
