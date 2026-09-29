using System;

namespace juegojuego
{
    // Registro de una partida jugada: jugador, fecha, nivel, intentos, tiempo
    // y puntuación final. Es la unidad que se guarda en la tabla de récords
    // y la que se persiste en disco (texto/binario) para llevar el historial.
    [Serializable]
    public class Puntaje : IComparable<Puntaje>
    {
        public string Jugador { get; set; }
        public DateTime Fecha { get; set; }
        public string Nivel { get; set; }
        public int Intentos { get; set; }
        public int Aciertos { get; set; }
        public int Fallos { get; set; }
        public TimeSpan Tiempo { get; set; }
        public int Puntuacion { get; set; }

        public Puntaje(string jugador, DateTime fecha, string nivel, int intentos, int aciertos, int fallos, TimeSpan tiempo, int puntuacion)
        {
            Jugador = jugador;
            Fecha = fecha;
            Nivel = nivel;
            Intentos = intentos;
            Aciertos = aciertos;
            Fallos = fallos;
            Tiempo = tiempo;
            Puntuacion = puntuacion;
        }

        // Orden natural: de mayor a menor puntuación (para el ranking)
        public int CompareTo(Puntaje otro)
        {
            if (otro == null) return 1;
            return otro.Puntuacion.CompareTo(this.Puntuacion);
        }

        public override string ToString()
        {
            return $"{Jugador,-12} | {Nivel,-8} | Intentos:{Intentos,3} | Tiempo:{Tiempo:mm\\:ss} | Puntos:{Puntuacion,5} | {Fecha:dd/MM/yyyy HH:mm}";
        }
    }
}
