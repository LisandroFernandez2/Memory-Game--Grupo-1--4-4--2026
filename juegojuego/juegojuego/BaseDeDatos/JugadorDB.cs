using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class JugadorDB
    {
        //Propiedades de la tabla JugadorDB
        public int ID { get; set; }
        public string Nombre { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public JugadorDB()
        {

        }

        //constructor
        public JugadorDB(int id, string N)
        {
            ID = id;
            Nombre = N;
        }
    }
}
