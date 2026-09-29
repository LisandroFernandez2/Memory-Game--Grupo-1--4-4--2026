using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Lista enlazada
    // Gestiona dinamicamente los pares de cartas que el jugador ya descubrio,
    // junto con el puntaje temporal (puntos) que otorgó cada par. Una lista
    // enlazada permite insertar cada nuevo par al final en tiempo O(1) y,
    // si se necesitara, eliminar un par puntual (por ejemplo al deshacer una
    // jugada) sin desplazar el resto de los elementos como ocurriría en un array
    public class ListaParesDescubiertos
    {
        // Par descubierto: valor de la carta y puntos que otorgo
        public class ParDescubierto
        {
            public int IdValor { get; set; }
            public int Puntos { get; set; }

            public ParDescubierto(int idValor, int puntos)
            {
                IdValor = idValor;
                Puntos = puntos;
            }
        }

        private readonly LinkedList<ParDescubierto> pares = new LinkedList<ParDescubierto>();

        public int CantidadPares => pares.Count;

        // Inserta un nuevo par descubierto al final de la lista enlazada
        public void AgregarPar(int idValor, int puntos)
        {
            pares.AddLast(new ParDescubierto(idValor, puntos));
        }

        // Elimina un par de la lista (por ejemplo, al deshacer un acierto)
        // Inserción/eliminación eficiente: no requiere desplazar elementos
        public bool QuitarPar(int idValor)
        {
            for (var nodo = pares.First; nodo != null; nodo = nodo.Next)
            {
                if (nodo.Value.IdValor == idValor)
                {
                    pares.Remove(nodo);
                    return true;
                }
            }
            return false;
        }

        // Suma total de puntos acumulados hasta el momento
        public int PuntajeTemporalTotal()
        {
            int total = 0;
            foreach (var par in pares)
                total += par.Puntos;
            return total;
        }

        public IEnumerable<ParDescubierto> ObtenerTodos()
        {
            return pares;
        }

        public void Limpiar()
        {
            pares.Clear();
        }
    }
}
