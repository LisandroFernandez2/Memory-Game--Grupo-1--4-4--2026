using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace juegojuego
{
    // Carga imagenes y musica desde el disco.
    // Se acuerda de lo que ya cargo: si dos modos usan el mismo fondo o la misma musica,
    // se carga una sola vez (los WAV pesan mucho) y la musica no se corta al cambiar de modo.
    public class Recursos
    {
        // Volumen de la musica
        private const float VolumenMusica = 0.5f; // 0.0 a 1.0

        // Placa de video: hace falta para crear texturas
        private readonly GraphicsDevice _graphicsDevice;
        // Lo ya cargado: ruta del archivo -> textura
        private readonly Dictionary<string, Texture2D> _texturas = new Dictionary<string, Texture2D>();
        // Lo ya cargado: ruta del archivo -> musica lista para reproducir en loop (null si no se pudo cargar)
        private readonly Dictionary<string, SoundEffectInstance> _musicas = new Dictionary<string, SoundEffectInstance>();
        // Los audios cargados, guardados para liberarlos al cerrar el juego
        private readonly List<SoundEffect> _sonidos = new List<SoundEffect>();

        // Constructor
        // Recibe: graphicsDevice la placa de video del juego
        public Recursos(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        // Carga una imagen desde disco y la convierte a alfa premultiplicado
        // Recibe: ruta archivo de imagen
        // Devuelve: la textura Texture2D lista para dibujar
        public Texture2D Textura(string ruta)
        {
            // Si ya se cargo antes devuelve la misma
            if (_texturas.TryGetValue(ruta, out Texture2D guardada))
                return guardada;

            Texture2D tex;
            using (FileStream fs = File.OpenRead(ruta))
                // Lee la imagen del archivo
                tex = Texture2D.FromStream(_graphicsDevice, fs);

            // Copia todos los pixeles a un arreglo
            Color[] datos = new Color[tex.Width * tex.Height];
            tex.GetData(datos);
            // Multiplica R G y B por la transparencia alfa para que los bordes transparentes se vean bien
            for (int i = 0; i < datos.Length; i++)
            {
                Color c = datos[i];
                datos[i] = new Color(c.R * c.A / 255, c.G * c.A / 255, c.B * c.A / 255, (int)c.A);
            }
            // Devuelve los pixeles ya corregidos a la textura
            tex.SetData(datos);

            _texturas[ruta] = tex;
            return tex;
        }

        // Carga un WAV como musica que se repite sin parar
        // Recibe: ruta archivo wav
        // Devuelve: la musica lista para reproducir o null si el archivo no existe o no hay placa de sonido
        // (en ese caso el juego sigue andando, solo que sin musica)
        public SoundEffectInstance Musica(string ruta)
        {
            // Si ya se cargo antes devuelve la misma
            if (_musicas.TryGetValue(ruta, out SoundEffectInstance guardada))
                return guardada;

            SoundEffectInstance instancia = null;
            try
            {
                if (File.Exists(ruta))
                {
                    SoundEffect sonido;
                    using (FileStream fs = File.OpenRead(ruta))
                        // Lee el wav
                        sonido = SoundEffect.FromStream(fs);
                    _sonidos.Add(sonido);

                    instancia = sonido.CreateInstance();
                    // Se repite sin parar
                    instancia.IsLooped = true;
                    instancia.Volume = VolumenMusica;
                }
            }
            catch (Exception) { /* sin audio disponible */ }

            _musicas[ruta] = instancia;
            return instancia;
        }

        // Recibe: nada Devuelve: nada
        // Libera la musica y los sonidos de la memoria Se llama al cerrar el juego
        public void Liberar()
        {
            foreach (SoundEffectInstance musica in _musicas.Values)
                musica?.Dispose();
            foreach (SoundEffect sonido in _sonidos)
                sonido.Dispose();
        }
    }
}
