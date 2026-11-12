using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    public class TematicaDB
    {
       //Propiedades de la tabla TematicaDB
       public int ID { get; set; } 
       public string Nombre { get; set; }  
       public string Carpeta { get; set; }

       //un constructor para que lo lea Microsoft.EntityFrameworkCore
       public TematicaDB()
       {

       }

       //constructor
       public TematicaDB(int id, string N , string C)
       {
            ID = id;
            Nombre = N;
            Carpeta = C;
       }
    }
}
