using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    public class PartidaDB
    {
        //Propiedades de la tabla PartidaDB
        public int ID { get; set; } 
        public int Jugador_id { get; set; }
        public int Nivel_id { get; set; }
        public int Tematica_id { get; set; }
        public DateTime Fecha { get; set; }
        public int Intentos { get; set; }
        public int Aciertos { get; set; }
        public int Fallos { get; set; }
        public int Tiempo_segundos { get; set; }
        public int Puntuacion { get; set; }
        public int Gano { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public PartidaDB()
        {

        }

        //constructor
        public PartidaDB(int id, int J_id, int N_id, int T_id, DateTime Fec, int I, int AC, int F, int T_S, int P, int G)
        {
            ID = id;
            Jugador_id = J_id;
            Nivel_id = N_id;
            Tematica_id = T_id;
            Fecha = Fec;
            Intentos = I;
            Aciertos = AC;
            Fallos = F;
            Tiempo_segundos = T_S;
            Puntuacion = P;
            Gano = G;
        }
    }
}
