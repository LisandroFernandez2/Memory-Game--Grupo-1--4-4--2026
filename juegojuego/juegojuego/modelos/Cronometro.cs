using System;
using System.Diagnostics;

namespace juegojuego
{
    // Temporizador que mide el tiempo que tarda el jugador en resolver el
    // tablero. Envuelve un Stopwatch para poder iniciarlo, pausarlo y
    // consultarlo fácilmente desde el bucle de Update de MonoGame.
    // Clase que mide cuanto tiempo pasa desde que arranca la partida
    public class Cronometro
    {
        // El Stopwatch cronometro de NET que hace el trabajo real
        private readonly Stopwatch reloj = new Stopwatch();

        // Recibe: nada Devuelve: nada
        // Empieza a contar desde cero
        public void Iniciar() => reloj.Restart();
        // Recibe: nada Devuelve: nada
        // Frena el conteo sin perder el tiempo acumulado
        public void Pausar() => reloj.Stop();
        // Recibe: nada Devuelve: nada
        // Sigue contando desde donde se pauso
        public void Reanudar() => reloj.Start();
        // Recibe: nada Devuelve: nada
        // Vuelve el tiempo a cero y lo deja frenado
        public void Reiniciar() => reloj.Reset();

        // Devuelve: el tiempo acumulado como TimeSpan permite sacar minutos segundos etc
        public TimeSpan TiempoTranscurrido => reloj.Elapsed;
        // Devuelve: el tiempo acumulado en segundos enteros int
        public int SegundosTranscurridos => (int)reloj.Elapsed.TotalSeconds;

        // Recibe: limiteSegundos int
        // Devuelve: true si ya paso ese tiempo false si no
        // Si el limite es 0 o menor nunca se agota siempre false
        public bool SeAgotoElTiempo(int limiteSegundos)
        {
            return limiteSegundos > 0 && SegundosTranscurridos >= limiteSegundos;
        }
    }
}
