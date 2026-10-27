using System;

namespace juegojuego
{
    // Representa un intento del jugador: las dos cartas volteadas y si coincidieron
    // Es el elemento que se apila en el historial de movimientos y el que se encola
    // como evento de volteo para las animaciones
    // Clase que describe UN intento: donde estaban las 2 cartas elegidas si acertaste y a que hora
    public class Movimiento
    {
        // Fila de la primera carta elegida
        public int Fila1 { get; set; }
        // Columna de la primera carta elegida
        public int Columna1 { get; set; }
        // Fila de la segunda carta elegida
        public int Fila2 { get; set; }
        // Columna de la segunda carta elegida
        public int Columna2 { get; set; }
        // true si las dos cartas formaron par false si fue fallo
        public bool Acierto { get; set; }
        // Fecha y hora en que se hizo el intento
        public DateTime Momento { get; set; }

        // Constructor
        // Recibe: fila1 columna1 fila2 columna2 posicion de las dos cartas y acierto true/false
        // Devuelve: nada crea el movimiento La hora se guarda sola con DateTime Now
        public Movimiento(int fila1, int columna1, int fila2, int columna2, bool acierto)
        {
            Fila1 = fila1;
            Columna1 = columna1;
            Fila2 = fila2;
            Columna2 = columna2;
            Acierto = acierto;
            Momento = DateTime.Now;
        }

        // Recibe: nada
        // Devuelve: texto listo para mostrar ej: [12:30:05] 0 1 - 2 3 => ACIERTO
        public override string ToString()
        {
            // Elige la palabra segun si fue acierto o fallo
            string resultado = Acierto ? "ACIERTO" : "FALLO";
            return $"[{Momento:HH:mm:ss}] ({Fila1},{Columna1}) - ({Fila2},{Columna2}) => {resultado}";
        }
    }
}
