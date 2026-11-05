using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Cola (Queue)
    // Actúa como buffer de eventos de volteo de cartas y de procesamiento de
    // animaciones. Cada vez que el jugador hace clic sobre una carta (o el juego
    // necesita ocultarla tras un fallo) se encola un evento; en el bucle de
    // actualización (Update) se van desencolando en el mismo orden en que
    // ocurrieron, garantizando que las animaciones se muestren cronológicamente.
    // Clase que guarda eventos en orden de llegada: el primero que entra es el primero que sale cola FIFO
    public class ColaEventos
    {
        // La cola real donde se guardan los eventos
        private readonly Queue<EventoVolteo> colaEventos = new Queue<EventoVolteo>();

        // Propiedad de solo lectura Devuelve: cuantos eventos estan esperando en la cola
        public int Pendientes => colaEventos.Count;

        // Agrega un evento al final de la cola
        // Recibe: evento el EventoVolteo a guardar
        // Devuelve: nada
        // Como funciona: lo agrega al final de la cola
        public void Encolar(EventoVolteo evento)
        {
            colaEventos.Enqueue(evento);
        }

        // Retira y devuelve el evento más antiguo (el primero en llegar)
        // Recibe: nada
        // Devuelve: el evento mas viejo y lo saca de la cola o null si no hay ninguno
        public EventoVolteo ProcesarSiguiente()
        {
            // Si la cola esta vacia devuelve null; si no saca el primero con Dequeue
            return colaEventos.Count == 0 ? null : colaEventos.Dequeue();
        }

        // Recibe: nada
        // Devuelve: true si quedan eventos en la cola false si esta vacia
        public bool HayEventosPendientes()
        {
            return colaEventos.Count > 0;
        }

        // Recibe: nada Devuelve: nada
        // Borra todos los eventos de la cola
        public void Limpiar()
        {
            colaEventos.Clear();
        }
    }
}
