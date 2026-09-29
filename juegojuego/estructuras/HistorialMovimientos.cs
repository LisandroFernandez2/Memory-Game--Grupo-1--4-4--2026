using System;
using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Pila (Stack)
    // Guarda el historial de movimientos (intentos) realizados por el jugador
    // Al ser una pila, el último movimiento realizado es el primero en poder
    // deshacerse (Undo) o el primero en reproducirse hacia atrás en un replay
    // inverso, respetando el orden natural LIFO en que ocurrieron los intentos
    public class HistorialMovimientos
    {
        private readonly Stack<Movimiento> pilaMovimientos = new Stack<Movimiento>();

        public int Cantidad => pilaMovimientos.Count;

        // Registra un nuevo movimiento en la cima de la pila
        public void Registrar(Movimiento movimiento)
        {
            pilaMovimientos.Push(movimiento);
        }

        // Deshace (retira) el último movimiento registrado.
        public Movimiento DeshacerUltimo()
        {
            if (pilaMovimientos.Count == 0)
                return null;

            return pilaMovimientos.Pop();
        }

        // Consulta el último movimiento sin retirarlo de la pila
        public Movimiento VerUltimo()
        {
            return pilaMovimientos.Count == 0 ? null : pilaMovimientos.Peek();
        }

        // Devuelve el historial completo en orden (del más reciente al más
        // antiguo), útil para un replay de la partida o para mostrar el registro.
        public IEnumerable<Movimiento> ObtenerHistorialCompleto()
        {
            return pilaMovimientos; // Stack<T> se enumera de cima (más reciente) a base.
        }

        public void Limpiar()
        {
            pilaMovimientos.Clear();
        }
    }
}
