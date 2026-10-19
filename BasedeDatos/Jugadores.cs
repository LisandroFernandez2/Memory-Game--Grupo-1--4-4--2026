using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class Jugadores
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public Jugadores()
        {

        }

        public Jugadores(int id, string N, string A)
        {
            ID = id;
            Nombre = N;
            Apellido = A;
        }
    }
}
