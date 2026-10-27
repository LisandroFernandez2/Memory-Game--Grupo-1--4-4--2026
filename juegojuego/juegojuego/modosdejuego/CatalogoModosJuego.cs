using System.Linq;

namespace juegojuego
{
    // AQUI SE AGREGAN LOS MODOS DE JUEGO (las REGLAS: Nivel Cronometro, Clasico, Infinito, etc.)
    // OJO: las tematicas (imagenes y musica de las cartas) se agregan en modelos/CatalogoModos.cs
    //
    // El orden de la lista es el orden en que salen en la pantalla de modos (se elige con el mouse o con las teclas 1 a 9).
    // Los modos con Disponible = false se ven en la lista con el cartel PROXIMAMENTE y no se pueden elegir.
    //
    // Para agregar un modo nuevo:
    //   1) Crear la clase en esta carpeta (copiar ModoClasico.cs que es la mas simple y cambiarle el nombre)
    //   2) Agregar UNA linea a la lista de abajo:   new ModoMiModo(),
    //   3) Compilar y listo: aparece en la pantalla de modos.
    //
    // Siempre tiene que haber al menos un modo y el primero tiene que estar Disponible.
    public static class CatalogoModosJuego
    {
        public static readonly ModoJuego[] Todos =
        {
            // El juego de siempre: elegir dificultad y ganar antes de que se acabe el tiempo
            new ModoNivelCronometro(),

            // Modos preparados pero todavia sin terminar (Disponible = false)
            new ModoClasico(),
            new ModoInfinito(),
            new ModoMultijugador(),
            new ModoCooperativo(),
            new ModoContrarreloj(),
            new ModoSupervivencia(),
        };

        // Cuantos modos se pueden jugar de verdad ahora (sirve para saber si hay que aclarar el modo en los records)
        public static int CantidadDisponibles => Todos.Count(m => m.Disponible);
    }
}
