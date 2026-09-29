using System;

namespace juegojuego
{
    // Representa un intento del jugador: las dos cartas volteadas y si coincidieron
    // Es el elemento que se apila en el historial de movimientos y el que se encola
    // como evento de volteo para las animaciones
    public class Movimiento
    {
        public int Fila1 { get; set; }
        public int Columna1 { get; set; }
        public int Fila2 { get; set; }
        public int Columna2 { get; set; }
        public bool Acierto { get; set; }
        public DateTime Momento { get; set; }

        public Movimiento(int fila1, int columna1, int fila2, int columna2, bool acierto)
        {
            Fila1 = fila1;
            Columna1 = columna1;
            Fila2 = fila2;
            Columna2 = columna2;
            Acierto = acierto;
            Momento = DateTime.Now;
        }

        public override string ToString()
        {
            string resultado = Acierto ? "ACIERTO" : "FALLO";
            return $"[{Momento:HH:mm:ss}] ({Fila1},{Columna1}) - ({Fila2},{Columna2}) => {resultado}";
        }
    }
}
