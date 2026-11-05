namespace juegojuego.BasedeDatos
{
    // Tabla "tematica": imagenes y musica de las cartas (la clase Modo del juego, ver CatalogoModos.cs).
    public class TematicaDB
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        // Carpeta dentro de Assets/modos/
        public string Carpeta { get; set; } = "";
    }
}
