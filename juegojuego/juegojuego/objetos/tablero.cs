using System;
using System.Collections.Generic;

namespace juegojuego
{
    // ESTRUCTURA: Array 2D.
    // Representa el tablero de juego como una matriz de cartas (filas x columnas).
    // Un array 2D permite el acceso directo por coordenadas (fila, columna), lo cual
    // es ideal para verificar rápidamente el estado de una carta al hacer clic sobre ella
    // y para comparar dos cartas seleccionadas al momento de verificar un par.
    // Clase del tablero: una matriz de cartas de filas x columnas
    public class tablero
    {
        // La matriz 2D de cartas Se accede asi: cartas[fila columna]
        public carta[,] cartas { get; set; }
        // Cantidad de filas solo se fija en el constructor
        public int filas { get; set; }
        // Cantidad de columnas solo se fija en el constructor
        public int columnas { get; set; }

        // Constructor
        // Lanza un error si la cantidad total de casillas es impar porque no se podrian armar pares
        public tablero(int filas, int columnas)
        {
            // Si el total es impar sobraria una carta sin pareja
            if ((filas * columnas) % 2 != 0)
                throw new ArgumentException("El tablero debe tener una cantidad par de cartas.");

            this.filas = filas;
            this.columnas = columnas;
            cartas = new carta[filas, columnas];
        }

        // Acceso directo por coordenadas (característica principal del array 2D).
        // Recibe: fila y columna
        // Devuelve: la carta de esa posicion puede ser null si la casilla esta vacia
        // Lanza un error si las coordenadas estan fuera del tablero
        public carta ObtenerCarta(int fila, int columna)
        {
            if (fila < 0 || fila >= filas || columna < 0 || columna >= columnas)
                throw new IndexOutOfRangeException("Coordenadas fuera del tablero.");

            return cartas[fila, columna];
        }

        // Recibe: fila columna y c la carta a guardar
        // Devuelve: nada
        // Como funciona: guarda la carta en esa casilla y le anota su posicion
        public void ColocarCarta(int fila, int columna, carta c)
        {
            c.fila = fila;
            c.columna = columna;
            cartas[fila, columna] = c;
        }

        // Recorre el array 2D verificando si todas las cartas están emparejadas.
        // Recibe: nada
        // Devuelve: true si todas las cartas estan emparejadas se gano false si falta alguna
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
        // Recibe: nada
        // Devuelve: una lista simple con todas las cartas sin contar casillas vacias
        public List<carta> ObtenerTodas()
        {
            var lista = new List<carta>();
            foreach (carta c in cartas)
                if (c != null)
                    lista.Add(c);
            return lista;
        }
    }
}
