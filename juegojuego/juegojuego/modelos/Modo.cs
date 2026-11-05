using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace juegojuego
{
    // Un MODO es una TEMATICA completa del juego: sus imagenes y su musica.
    // (No confundir con ModoJuego, que son las REGLAS: Nivel Cronometro, Clasico, Infinito, etc.
    //  Los niveles ya no se definen aca: los ofrece cada ModoJuego y se muestran solo los que alcanzan con las caras de la tematica)
    //
    // Cada modo vive en su propia carpeta: Assets/modos/<carpeta>/
    //     caras/        imagenes de las cartas (obligatorio)
    //                   cuantas mas haya, mas niveles se pueden jugar: un nivel necesita tantas caras como pares tiene
    //                   (Muy Facil necesita 8 caras, Facil 10, Normal 18, Dificil 28, Muy Dificil 36, Imposible 45)
    //     dorsos/       imagenes del dorso de las cartas (obligatorio) en cada partida se elige una al azar
    //     fondo.jpg     fondo de pantalla (opcional)
    //     menu.wav      musica del menu (opcional)
    //     partida.wav   musica de la partida (opcional)
    // Lo opcional que no este en la carpeta del modo se toma de Assets/comun/
    //
    // Los modos se registran en CatalogoModos.cs
    public class Modo
    {
        // Nombre que se muestra en el menu (sin tildes: la fuente no las tiene)
        public string Nombre { get; }
        // Nombre de la carpeta dentro de Assets/modos/
        public string Carpeta { get; }

        // Fondo de pantalla
        public Texture2D Fondo { get; set; }
        // Imagenes de las caras de las cartas
        public List<Texture2D> Caras { get; } = new List<Texture2D>();
        // Imagenes del dorso de las cartas
        public List<Texture2D> Dorsos { get; } = new List<Texture2D>();
        // Musica del menu puede ser null si no hay audio
        public SoundEffectInstance MusicaMenu { get; set; }
        // Musica de la partida puede ser null si no hay audio
        public SoundEffectInstance MusicaPartida { get; set; }

        // true cuando ya se leyeron las imagenes y la musica del disco
        private bool _cargado;

        // Constructor
        // Recibe: nombre texto del menu y carpeta nombre de la carpeta en Assets/modos
        // Devuelve: nada Solo guarda los datos: las imagenes se cargan despues con Cargar
        public Modo(string nombre, string carpeta)
        {
            Nombre = nombre;
            Carpeta = carpeta;
        }

        // Lee del disco las imagenes y la musica del modo. Si ya estaba cargado no hace nada.
        // Recibe: recursos el cargador de imagenes y musica Devuelve: nada
        // Falla con un mensaje claro si falta la carpeta, las caras o los dorsos
        public void Cargar(Recursos recursos)
        {
            if (_cargado) return;

            string carpetaModo = Path.Combine(AppContext.BaseDirectory, "Assets", "modos", Carpeta);
            string carpetaComun = Path.Combine(AppContext.BaseDirectory, "Assets", "comun");


            // Caras y dorsos: todas las imagenes de cada carpeta
            foreach (string archivo in ImagenesEn(Path.Combine(carpetaModo, "caras")))
                Caras.Add(recursos.Textura(archivo));
            foreach (string archivo in ImagenesEn(Path.Combine(carpetaModo, "dorsos")))
                Dorsos.Add(recursos.Textura(archivo));


            // Fondo y musica: los del modo si los tiene, si no los de Assets/comun
            Fondo = recursos.Textura(ArchivoDelModo(carpetaModo, carpetaComun, "fondo.jpg"));
            MusicaMenu = recursos.Musica(ArchivoDelModo(carpetaModo, carpetaComun, "menu.wav"));
            MusicaPartida = recursos.Musica(ArchivoDelModo(carpetaModo, carpetaComun, "partida.wav"));

            _cargado = true;
        }

        // Recibe: carpeta ruta de la carpeta con imagenes
        // Devuelve: las rutas de todas las imagenes png o jpg de esa carpeta (vacio si la carpeta no existe)
        private static List<string> ImagenesEn(string carpeta)
        {
            List<string> imagenes = new List<string>();
            if (!Directory.Exists(carpeta)) return imagenes;

            foreach (string archivo in Directory.GetFiles(carpeta))
            {
                string extension = Path.GetExtension(archivo).ToLowerInvariant();
                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    imagenes.Add(archivo);
            }
            imagenes.Sort(StringComparer.OrdinalIgnoreCase);
            return imagenes;
        }

        // Recibe: carpetaModo carpetaComun y nombreArchivo ej: "fondo.jpg"
        // Devuelve: la ruta del archivo dentro de la carpeta del modo si existe; si no, la de la carpeta comun
        private static string ArchivoDelModo(string carpetaModo, string carpetaComun, string nombreArchivo)
        {
            string propio = Path.Combine(carpetaModo, nombreArchivo);
            return File.Exists(propio) ? propio : Path.Combine(carpetaComun, nombreArchivo);
        }
    }
}
