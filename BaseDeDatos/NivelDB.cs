using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BaseDeDatos
{
    public class NivelDB
    {
        //Propiedades de la tabla NivelDB
        public int ID { get; set; }
        public int Modo_juego_id { get; set; }
        public string Nombre { get; set; }
        public int Filas { get; set; }
        public int Columnas { get; set; }
        public int Pares { get; set; }
        public int? Tiempo_limite { get; set; }
        public int? Vidas { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public NivelDB()
        {

        }

        //constructor
        public NivelDB(int id, int M_id, string N, int F, int C, int P, int? T_L, int? V)
        {
            ID = id;
            Modo_juego_id = M_id;
            Nombre = N;
            Filas = F;
            Columnas = C;
            Pares = P;
            Tiempo_limite = T_L;
            Vidas = V;
        }
    }
}
