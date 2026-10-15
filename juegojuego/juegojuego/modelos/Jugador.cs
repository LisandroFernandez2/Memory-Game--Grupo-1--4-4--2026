namespace juegojuego
{
    // Un jugador de la partida: su nombre y cuantos pares acerto y cuantos intentos fallo.
    // En el juego solitario hay un solo Jugador; los modos multijugador crean varios (ver Partida.Jugadores)
    public class Jugador
    {
        // Nombre que se muestra en pantalla (sin tildes: la fuente no las tiene)
        public string Nombre { get; set; }
        // Pares que encontro este jugador
        public int Aciertos { get; set; }
        // Intentos en los que este jugador no acerto
        public int Fallos { get; set; }

        // Constructor
        // Recibe: nombre del jugador
        // Devuelve: nada crea al jugador con aciertos y fallos en 0
        public Jugador(string nombre)
        {
            Nombre = nombre;
        }
    }
}
