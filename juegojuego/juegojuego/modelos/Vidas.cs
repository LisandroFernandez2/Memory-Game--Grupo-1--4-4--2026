using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.modelos
{
    public class Vidas
    {
        public int vidas { get; set; }

        // Se pierde cuando no queda ninguna vida
        public bool SeAgotoLasVidas() => vidas <= 0;
    }
}
