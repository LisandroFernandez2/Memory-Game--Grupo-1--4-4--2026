namespace juegojuego
{
    // AQUI SE AGREGAN LAS TEMATICAS DEL JUEGO (imagenes y musica de las cartas).
    // OJO: las REGLAS (Nivel Cronometro, Clasico, Infinito, etc.) se agregan en modosdejuego/CatalogoModosJuego.cs
    //
    // Para agregar una tematica nueva (ej: "Undertale"):
    //   1) Crear la carpeta Assets/modos/undertale/ con:
    //          caras/       las imagenes de las cartas (300x392, png o jpg)
    //          dorsos/      una o mas imagenes de dorso
    //          (opcional)   fondo.jpg  menu.wav  partida.wav  -> si no estan se usan los de Assets/comun/
    //   2) Agregar UNA linea a la lista de abajo:
    //          new Modo("Undertale", "undertale"),
    //   3) Compilar y listo: aparece en el menu de niveles (se cambia con las flechas o con el mouse).
    //
    // Que niveles se pueden jugar con cada tematica:
    //   Los niveles los define cada modo de juego. Si la tematica tiene pocas caras, los niveles que necesitan
    //   mas caras de las que hay no se muestran solos.
    //   (Muy Facil necesita 8 caras, Facil 10, Normal 18, Dificil 28, Muy Dificil 36, Imposible 45)
    //
    // Siempre tiene que haber al menos una tematica. La primera es la que aparece al abrir el juego.
    // Con una sola tematica el menu de niveles se ve sin selector de tematica.
    public static class CatalogoModos
    {
        public static readonly Modo[] Todos =
        {
            // Todas las tematicas mezcladas (como era el antiguo)
            new Modo("Mezcla", "mezcla"),

        };
    }
}
