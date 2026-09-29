using System;
using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Array 2D.
    // Representa el tablero de juego como una matriz de cartas (filas x columnas).
    // Un array 2D permite el acceso directo por coordenadas (fila, columna), lo cual
    // es ideal para verificar rápidamente el estado de una carta al hacer clic sobre ella
    // y para comparar dos cartas seleccionadas al momento de verificar un par.
    public class tablero
    {
        public carta[,] cartas { get; set; }
        public int filas { get; private set; }
        public int columnas { get; private set; }

        public tablero(int filas, int columnas)
        {
            if ((filas * columnas) % 2 != 0)
                throw new ArgumentException("El tablero debe tener una cantidad par de cartas.");

            this.filas = filas;
            this.columnas = columnas;
            cartas = new carta[filas, columnas];
        }

        // Acceso directo por coordenadas (característica principal del array 2D).
        public carta ObtenerCarta(int fila, int columna)
        {
            if (fila < 0 || fila >= filas || columna < 0 || columna >= columnas)
                throw new IndexOutOfRangeException("Coordenadas fuera del tablero.");

            return cartas[fila, columna];
        }

        public void ColocarCarta(int fila, int columna, carta c)
        {
            c.fila = fila;
            c.columna = columna;
            cartas[fila, columna] = c;
        }

        // Recorre el array 2D verificando si todas las cartas están emparejadas.
        public bool TableroCompleto()
        {
            foreach (carta c in cartas)
            {
                if (c == null || !c.estaEmparejada)
                    return false;
            }
            return true;
        }

        // Devuelve todas las cartas del tablero como lista plana
        public List<carta> ObtenerTodas()
        {
            var lista = new List<carta>();
            foreach (carta c in cartas)
                if (c != null)
                    lista.Add(c);
            return lista;
        }

        // Reinicia el estado de todas las cartas
        public void Reiniciar()
        {
            foreach (carta c in cartas)
            {
                if (c == null) continue;
                c.estaDescubierta = false;
                c.estaEmparejada = false;
            }
        }
    }
}
