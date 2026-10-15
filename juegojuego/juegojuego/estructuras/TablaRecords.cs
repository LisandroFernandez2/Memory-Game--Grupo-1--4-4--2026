using System.Collections.Generic;

namespace juegojuego
{
    // ALGORITMO: Ordenamiento por Insercion
    // Mantiene la tabla de records (personales y globales) siempre ordenada de
    // mayor a menor puntuación. Se eligio inserción porque la lista de puntajes
    // es pequeña y, ademas, cada vez que termina una partida solo se agrega UN
    // registro nuevo: insertarlo ya ordenado es más eficiente que reordenar toda
    // la lista con un algoritmo generico
    // Clase que guarda los puntajes siempre ordenados de mayor a menor
    public class TablaRecords
    {
        // Lista interna con todos los puntajes
        private readonly List<Puntaje> registros = new List<Puntaje>();

        // Devuelve la lista para poder leerla pero desde afuera no se puede modificar
        public IReadOnlyList<Puntaje> Registros => registros;

        // Inserta un nuevo puntaje manteniendo la lista ordenada de mayor a menor
        // puntuacion, usando la logica de ordenamiento por inserción
        // Recibe: nuevo el Puntaje a guardar
        // Devuelve: nada
        // Como funciona: lo ubica en su lugar para que la lista siga ordenada de mayor a menor
        public void InsertarOrdenado(Puntaje nuevo)
        {
            // i apunta al ultimo elemento que ya estaba en la lista
            int i = registros.Count - 1;
            registros.Add(nuevo); // se agrega al final y se "hunde" hasta su lugar

            // Mientras el elemento de la izquierda tenga menos puntos lo corre un lugar a la derecha
            while (i >= 0 && registros[i].Puntuacion < nuevo.Puntuacion)
            {
                registros[i + 1] = registros[i];
                i--;
            }
            // Coloca el nuevo en el hueco que quedo libre
            registros[i + 1] = nuevo;
        }

        // Ordena por inserción una lista completa de puntajes (por ejemplo, luego
        // de cargar los registros desde el archivo binario)
        // Recibe: lista List<Puntaje>
        // Devuelve: nada ordena la misma lista que le pasas
        // Es estatico: se usa asi TablaRecords OrdenarPorInsercionlista
        public static void OrdenarPorInsercion(List<Puntaje> lista)
        {
            // Arranca en 1 porque el primer elemento solo ya cuenta como ordenado
            for (int i = 1; i < lista.Count; i++)
            {
                // actual = el elemento que hay que ubicar
                Puntaje actual = lista[i];
                int j = i - 1;

                // Desplaza los elementos mayores (o iguales) una posición a la derecha
                // Corre a la derecha a todos los que tengan menos puntos que actual
                while (j >= 0 && lista[j].Puntuacion < actual.Puntuacion)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                // Guarda actual en su posicion final
                lista[j + 1] = actual;
            }
        }

        // Devuelve el top N de la tabla (records globales).
        // Recibe: cantidad cuantos puntajes queres
        // Devuelve: lista con los mejores 'cantidad' puntajes o menos si hay pocos
        public List<Puntaje> ObtenerTop(int cantidad)
        {
            var top = new List<Puntaje>();
            for (int i = 0; i < registros.Count && i < cantidad; i++)
                top.Add(registros[i]);
            return top;
        }

        // Records personales: filtra por nombre de jugador (ya vienen ordenados)
        // Recibe: jugador nombre string
        // Devuelve: lista solo con los puntajes de ese jugador ya ordenados
        public List<Puntaje> ObtenerRecordsDe(string jugador)
        {
            var propios = new List<Puntaje>();
            foreach (var r in registros)
                if (r.Jugador == jugador)
                    propios.Add(r);
            return propios;
        }

        // Recibe: lista de puntajes ej: los leidos del archivo
        // Devuelve: nada
        // Como funciona: borra lo que habia copia la lista nueva y la ordena
        public void CargarDesde(List<Puntaje> lista)
        {
            registros.Clear();
            registros.AddRange(lista);
            OrdenarPorInsercion(registros);
        }
    }
}
