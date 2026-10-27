using System;
using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Pila (Stack)
    // Guarda el historial de movimientos (intentos) realizados por el jugador
    // Al ser una pila, el último movimiento realizado es el primero en poder
    // deshacerse (Undo) o el primero en reproducirse hacia atrás en un replay
    // inverso, respetando el orden natural LIFO en que ocurrieron los intentos
    // Clase que guarda los intentos del jugador El ultimo que hiciste es el primero que sale pila LIFO
    public class HistorialMovimientos
    {
        // La pila real donde se apilan los movimientos
        private readonly Stack<Movimiento> pilaMovimientos = new Stack<Movimiento>();

        // Propiedad de solo lectura Devuelve: cuantos movimientos hay guardados
        public int Cantidad => pilaMovimientos.Count;

        // Registra un nuevo movimiento en la cima de la pila
        // Recibe: movimiento el intento a guardar
        // Devuelve: nada
        // Como funciona: lo apila arriba de todo
        public void Registrar(Movimiento movimiento)
        {
            pilaMovimientos.Push(movimiento);
        }

        // Deshace (retira) el último movimiento registrado.
        // Recibe: nada
        // Devuelve: el ultimo movimiento y lo saca de la pila o null si no hay ninguno
        public Movimiento DeshacerUltimo()
        {
            // Si la pila esta vacia no hay nada que deshacer
            if (pilaMovimientos.Count == 0)
                return null;

            // Pop = saca y devuelve el movimiento de arriba
            return pilaMovimientos.Pop();
        }

        // Consulta el último movimiento sin retirarlo de la pila
        // Recibe: nada
        // Devuelve: el ultimo movimiento SIN sacarlo solo mirar o null si no hay ninguno
        public Movimiento VerUltimo()
        {
            // Peek = mira el de arriba sin sacarlo
            return pilaMovimientos.Count == 0 ? null : pilaMovimientos.Peek();
        }

        // Devuelve el historial completo en orden (del más reciente al más
        // antiguo), útil para un replay de la partida o para mostrar el registro.
        // Recibe: nada
        // Devuelve: todos los movimientos del mas nuevo al mas viejo no los saca de la pila
        public IEnumerable<Movimiento> ObtenerHistorialCompleto()
        {
            return pilaMovimientos; // Stack<T> se enumera de cima (más reciente) a base.
        }

        // Recibe: nada Devuelve: nada
        // Vacia la pila borra todo el historial
        public void Limpiar()
        {
            pilaMovimientos.Clear();
        }
    }
}
