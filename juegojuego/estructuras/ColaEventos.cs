using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Cola (Queue)
    // Actúa como buffer de eventos de volteo de cartas y de procesamiento de
    // animaciones. Cada vez que el jugador hace clic sobre una carta (o el juego
    // necesita ocultarla tras un fallo) se encola un evento; en el bucle de
    // actualización (Update) se van desencolando en el mismo orden en que
    // ocurrieron, garantizando que las animaciones se muestren cronológicamente.
    public class ColaEventos
    {
        private readonly Queue<EventoVolteo> colaEventos = new Queue<EventoVolteo>();

        public int Pendientes => colaEventos.Count;

        // Agrega un evento al final de la cola
        public void Encolar(EventoVolteo evento)
        {
            colaEventos.Enqueue(evento);
        }

        // Retira y devuelve el evento más antiguo (el primero en llegar)
        public EventoVolteo ProcesarSiguiente()
        {
            return colaEventos.Count == 0 ? null : colaEventos.Dequeue();
        }

        public bool HayEventosPendientes()
        {
            return colaEventos.Count > 0;
        }

        public void Limpiar()
        {
            colaEventos.Clear();
        }
    }
}
