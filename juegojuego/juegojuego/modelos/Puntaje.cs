using System;

namespace juegojuego
{
    // Registro de una partida jugada: jugador, fecha, nivel, intentos, tiempo
    // y puntuación final. Es la unidad que se guarda en la tabla de récords
    // y la que se persiste en disco (texto/binario) para llevar el historial.
    // Clase que guarda los datos de UNA partida terminada
    // Implementa IComparable para poder ordenar los puntajes mas puntos primero
    [Serializable]
    public class Puntaje : IComparable<Puntaje>
    {
        // Nombre del jugador
        public string Jugador { get; set; }
        // Fecha y hora en que termino la partida
        public DateTime Fecha { get; set; }
        // Nombre de la dificultad jugada
        public string Nivel { get; set; }
        // Cantidad total de intentos
        public int Intentos { get; set; }
        // Cantidad de pares acertados
        public int Aciertos { get; set; }
        // Cantidad de intentos fallidos
        public int Fallos { get; set; }
        // Tiempo total que tardo en terminar
        public TimeSpan Tiempo { get; set; }
        // Puntos finales
        public int Puntuacion { get; set; }

        // Constructor
        // Recibe: jugador fecha nivel intentos aciertos fallos tiempo y puntuacion
        // Devuelve: nada crea el puntaje Solo guarda los valores
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
        // Recibe: otro el Puntaje con el que se compara
        // Devuelve: un numero que indica quien va primero negativo = este positivo = el otro 0 = empate
        // El que tiene mas puntos va primero
        public int CompareTo(Puntaje otro)
        {
            // Cualquier puntaje va antes que null
            if (otro == null) return 1;
            // Se compara al reves otro contra este para que ordene de mayor a menor
            return otro.Puntuacion.CompareTo(this.Puntuacion);
        }

        // Recibe: nada
        // Devuelve: una linea de texto con jugador nivel intentos tiempo puntos y fecha
        public override string ToString()
        {
            return $"{Jugador,-12} | {Nivel,-8} | Intentos:{Intentos,3} | Tiempo:{Tiempo:mm\\:ss} | Puntos:{Puntuacion,5} | {Fecha:dd/MM/yyyy HH:mm}";
        }
    }
}
