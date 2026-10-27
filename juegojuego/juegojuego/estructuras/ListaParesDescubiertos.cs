using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Lista enlazada
    // Gestiona dinamicamente los pares de cartas que el jugador ya descubrio,
    // junto con el puntaje temporal (puntos) que otorgó cada par. Una lista
    // enlazada permite insertar cada nuevo par al final en tiempo O(1) y,
    // si se necesitara, eliminar un par puntual (por ejemplo al deshacer una
    // jugada) sin desplazar el resto de los elementos como ocurriría en un array
    // Clase que guarda la lista de pares que ya encontraste y los puntos que dio cada uno
    public class ListaParesDescubiertos
    {
        // Par descubierto: valor de la carta y puntos que otorgo
        // Clase interna que representa UN par encontrado su valor y sus puntos
        public class ParDescubierto
        {
            // Numero que identifica al par es el mismo idValor de las cartas
            public int IdValor { get; set; }
            // Puntos que dio este par
            public int Puntos { get; set; }

            // Constructor
            // Recibe: idValor y puntos
            // Devuelve: nada crea el par Solo guarda los valores
            public ParDescubierto(int idValor, int puntos)
            {
                IdValor = idValor;
                Puntos = puntos;
            }
        }

        // La lista enlazada real donde se guardan los pares
        private readonly LinkedList<ParDescubierto> pares = new LinkedList<ParDescubierto>();

        // Propiedad de solo lectura Devuelve: cuantos pares encontraste hasta ahora
        public int CantidadPares => pares.Count;

        // Inserta un nuevo par descubierto al final de la lista enlazada
        // Recibe: idValor valor del par y puntos lo que suma
        // Devuelve: nada
        // Como funciona: agrega el par al final de la lista
        public void AgregarPar(int idValor, int puntos)
        {
            pares.AddLast(new ParDescubierto(idValor, puntos));
        }

        // Elimina un par de la lista (por ejemplo, al deshacer un acierto)
        // Inserción/eliminación eficiente: no requiere desplazar elementos
        // Recibe: idValor valor del par a sacar
        // Devuelve: true si lo encontro y lo borro false si no estaba
        // Como funciona: recorre la lista hasta dar con ese valor
        public bool QuitarPar(int idValor)
        {
            // Recorre nodo por nodo desde el primero hasta el ultimo
            for (var nodo = pares.First; nodo != null; nodo = nodo.Next)
            {
                // Si este nodo es el par buscado lo elimina y termina
                if (nodo.Value.IdValor == idValor)
                {
                    pares.Remove(nodo);
                    return true;
                }
            }
            return false;
        }

        // Suma total de puntos acumulados hasta el momento
        // Recibe: nada
        // Devuelve: la suma de los puntos de todos los pares int
        public int PuntajeTemporalTotal()
        {
            // Acumulador: arranca en 0 y suma los puntos de cada par
            int total = 0;
            foreach (var par in pares)
                total += par.Puntos;
            return total;
        }
    }
}
