using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class RecordDB
    {
        //Propiedades de la tabla RecordDB
        public int ID { get; set; }
        public int Partida_id { get; set; }
        public int Jugador_id { get; set; }
        public string Nivel { get; set; }
        public int Intentos { get; set; }
        public int Aciertos { get; set; }
        public int Fallos { get; set; }
        public int Tiempo_segundos { get; set; }
        public int Puntuacion { get; set; }
        public DateTime Fecha { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public RecordDB()
        {

        }

        //constructor
        public RecordDB(int id, int P_id, int J_id, string N, int I, int AC, int F, int T_S, int P, DateTime Fe)
        {
            ID = id;
            Partida_id = P_id;
            Jugador_id = J_id;
            Nivel = N;
            Intentos = I;
            Aciertos = AC;
            Fallos = F;
            Tiempo_segundos = T_S;
            Puntuacion = P;
            Fecha = Fe;
        }
    }
}
