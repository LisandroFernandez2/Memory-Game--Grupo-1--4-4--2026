using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class Par_descubiertoDB
    {
        //Propiedades de la tabla Par_descubiertoDB
        public int ID { get; set; }
        public int Partida_id { get; set; }
        public int Id_valor { get; set; }
        public int Puntos { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public Par_descubiertoDB()
        {

        }

        //constructor
        public Par_descubiertoDB(int id, int P_id, int V_id, int P)
        {
            ID = id;
            Partida_id = P_id;
            Id_valor = V_id;
            Puntos = P;
        }
    }
}
