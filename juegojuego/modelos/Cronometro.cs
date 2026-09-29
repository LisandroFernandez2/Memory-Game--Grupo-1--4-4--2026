using System;
using System.Diagnostics;

namespace juegojuego
{
    // Temporizador que mide el tiempo que tarda el jugador en resolver el
    // tablero. Envuelve un Stopwatch para poder iniciarlo, pausarlo y
    // consultarlo fácilmente desde el bucle de Update de MonoGame.
    public class Cronometro
    {
        private readonly Stopwatch reloj = new Stopwatch();

        public void Iniciar() => reloj.Restart();
        public void Pausar() => reloj.Stop();
        public void Reanudar() => reloj.Start();
        public void Reiniciar() => reloj.Reset();

        public TimeSpan TiempoTranscurrido => reloj.Elapsed;
        public int SegundosTranscurridos => (int)reloj.Elapsed.TotalSeconds;

        public bool SeAgotoElTiempo(int limiteSegundos)
        {
            return limiteSegundos > 0 && SegundosTranscurridos >= limiteSegundos;
        }
    }
}
