using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class CartaDB
    {
        //Propiedades de la tabla CartaDB
        public int ID {  get; set; } 
        public int partida_id { get; set; }  
        public int fila { get; set; } 
        public int columna { get; set; } 
        public int id_valor { get; set; }
        public int esta_descubierta { get; set; }   
        public int esta_emparejada { get; set; }

        //un constructor para que lo lea Microsoft.EntityFrameworkCore 
        public CartaDB()
        {

        }

        //constructor
        public CartaDB(int id, int P_id, int V_id, int F, int C, int ED, int EE)
        {
            ID = id;
            partida_id = P_id;
            id_valor = V_id;
            fila = F;
            columna = C;
            esta_descubierta = ED;
            esta_emparejada = EE;
        }
    }
}
