using System.Collections.Generic;

namespace juegojuego
{
    // ALGORITMO: Ordenamiento por Insercion
    // Mantiene la tabla de records (personales y globales) siempre ordenada de
    // mayor a menor puntuación. Se eligio inserción porque la lista de puntajes
    // es pequeña y, ademas, cada vez que termina una partida solo se agrega UN
    // registro nuevo: insertarlo ya ordenado es más eficiente que reordenar toda
    // la lista con un algoritmo generico
    public class TablaRecords
    {
        private readonly List<Puntaje> registros = new List<Puntaje>();

        public IReadOnlyList<Puntaje> Registros => registros;

        // Inserta un nuevo puntaje manteniendo la lista ordenada de mayor a menor
        // puntuacion, usando la logica de ordenamiento por inserción
        public void InsertarOrdenado(Puntaje nuevo)
        {
            int i = registros.Count - 1;
            registros.Add(nuevo); // se agrega al final y se "hunde" hasta su lugar

            while (i >= 0 && registros[i].Puntuacion < nuevo.Puntuacion)
            {
                registros[i + 1] = registros[i];
                i--;
            }
            registros[i + 1] = nuevo;
        }

        // Ordena por inserción una lista completa de puntajes (por ejemplo, luego
        // de cargar los registros desde el archivo binario)
        public static void OrdenarPorInsercion(List<Puntaje> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Puntaje actual = lista[i];
                int j = i - 1;

                // Desplaza los elementos mayores (o iguales) una posición a la derecha
                while (j >= 0 && lista[j].Puntuacion < actual.Puntuacion)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                lista[j + 1] = actual;
            }
        }

        // Devuelve el top N de la tabla (records globales).
        public List<Puntaje> ObtenerTop(int cantidad)
        {
            var top = new List<Puntaje>();
            for (int i = 0; i < registros.Count && i < cantidad; i++)
                top.Add(registros[i]);
            return top;
        }

        // Records personales: filtra por nombre de jugador (ya vienen ordenados)
        public List<Puntaje> ObtenerRecordsDe(string jugador)
        {
            var propios = new List<Puntaje>();
            foreach (var r in registros)
                if (r.Jugador == jugador)
                    propios.Add(r);
            return propios;
        }

        public void CargarDesde(List<Puntaje> lista)
        {
            registros.Clear();
            registros.AddRange(lista);
            OrdenarPorInsercion(registros);
        }
    }
}
