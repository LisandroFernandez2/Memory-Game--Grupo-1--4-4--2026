using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class MovimientoDB
    {
        //Propiedades de la tabla MovimientoDB
        public int ID { get; set; }
        public int partida_id { get; set; }
        public int fila1 { get; set; }
        public int columna1 { get; set; }
        public int fila2 { get; set; }
        public int columna2 { get; set; }
        public int acierto { get; set; }
        public DateTime momento { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public MovimientoDB()
        {

        }

        //constructor
        public MovimientoDB(int id, int P_id, int C1, int F1, int C2, int F2, int Ac, DateTime M)
        {
            ID = id;
            partida_id = P_id;
            fila1 = F1;
            columna1 = C1;
            fila2 = F2;
            columna2 = C2;
            acierto = Ac;
            momento = M;
        }
    }
}
