using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
	internal class Modo_JuegoDB
	{
        //Propiedades de la tabla Modo_JuegoDB
        public int ID { get; set; } 
		public string nombre { get; set; }  
		public string descripcion { get; set; } 
		public int disponible { get; set; } 
		public int cantidad_jugadores { get; set; }
		public int guarda_records { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore
        public Modo_JuegoDB()
		{

		}

        //constructor
        public Modo_JuegoDB(int id, string N, string D, string Dis, int CJ, int GR)
		{
			ID = id;
			nombre = N;
			descripcion = D;
			disponible = Dis;
			cantidad_jugadores = CJ;
			guarda_records = GR;
		}
	}
}