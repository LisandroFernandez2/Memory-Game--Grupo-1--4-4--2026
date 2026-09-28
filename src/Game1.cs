using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace MemoryGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Song Musica;
        private Texture2D BtnJugarNormal;
        private Texture2D BtnJugarPresionado;
        private Texture2D BtnSalirNormal;
        private Texture2D BtnSalirPresionado;
        private Rectangle RectanguloBotonJugar;
        private bool BotonJugarPresionado;
        private Rectangle RectanguloBotonSalir;
        private bool BotonSalirPresionado;
        public enum Pantalla
        {
            PantallaInicial,
            PantallaNiveles,
            PantallaJuego,
            PantallaVictoria,
            PantallaDerrota
        }
        public Pantalla pantallaActual = Pantalla.PantallaInicial;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.Title = "Memory Game";
        }

        protected override void Initialize()
        {

            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;


            _graphics.IsFullScreen = true;


            _graphics.ApplyChanges();
            base.Initialize();
        }
        public Texture2D TexturaDeFondoPantallaInicial;
        public Texture2D TexturaDeFondoPantallaSeleccionarNiveles;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TexturaDeFondoPantallaInicial = Content.Load<Texture2D>("bg_menu_bosque");
            TexturaDeFondoPantallaSeleccionarNiveles = Content.Load<Texture2D>("bg_seleccionar_nivel");
            BtnJugarNormal = Content.Load<Texture2D>("btn_jugar_normal");
            BtnJugarPresionado = Content.Load<Texture2D>("btn_jugar_presionado");

            BtnSalirNormal = Content.Load<Texture2D>("btn_salir_normal");
            BtnSalirPresionado = Content.Load<Texture2D>("btn_salir_presionado");
            Musica = Content.Load<Song>("musica");

            MediaPlayer.Play(Musica);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            MouseState mouse = Mouse.GetState();

            if (pantallaActual == Pantalla.PantallaInicial)
            {
                if (RectanguloBotonSalir.Contains(mouse.Position))
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        BotonSalirPresionado = true;
                    }
                    else if (BotonSalirPresionado)
                    {
                        BotonSalirPresionado = false;
                        Exit();
                    }
                }
                else
                {
                    BotonSalirPresionado = false;
                }
                if (RectanguloBotonJugar.Contains(mouse.Position))
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        BotonJugarPresionado = true;
                    }
                    else if (BotonJugarPresionado)
                    {
                        BotonJugarPresionado = false;
                        pantallaActual = Pantalla.PantallaNiveles;
                    }
                }
                else
                {
                    BotonJugarPresionado = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (pantallaActual == Pantalla.PantallaInicial)
            {
                _spriteBatch.Draw(
                    TexturaDeFondoPantallaInicial,
                    new Rectangle(
                        0,
                        0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height
                    ),
                    Color.White
                );

                int ancho = (int)(BtnJugarNormal.Width * 1.3f);
                int alto = (int)(BtnJugarNormal.Height * 1.3f);

                RectanguloBotonJugar = new Rectangle(
                    (GraphicsDevice.Viewport.Width - ancho) / 2,
                    670,
                    ancho,
                    alto
                );

                if (BotonJugarPresionado)
                {
                    _spriteBatch.Draw(
                        BtnJugarPresionado,
                        RectanguloBotonJugar,
                        Color.White
                    );
                }
                else
                {
                    _spriteBatch.Draw(
                        BtnJugarNormal,
                        RectanguloBotonJugar,
                        Color.White
                    );
                }

                int anchoSalir = (int)(BtnSalirNormal.Width * 1.3f);
                int altoSalir = (int)(BtnSalirNormal.Height * 1.3f);

                RectanguloBotonSalir = new Rectangle(
                    (GraphicsDevice.Viewport.Width - anchoSalir) / 2,
                    1600,
                    anchoSalir,
                    altoSalir
                );

                if (BotonSalirPresionado)
                {
                    _spriteBatch.Draw(
                        BtnSalirPresionado,
                        RectanguloBotonSalir,
                        Color.White
                    );
                }
                else
                {
                    _spriteBatch.Draw(
                        BtnSalirNormal,
                        RectanguloBotonSalir,
                        Color.White
                    );
                }
            }
            else if (pantallaActual == Pantalla.PantallaNiveles)
            {
                _spriteBatch.Draw(
                    TexturaDeFondoPantallaSeleccionarNiveles,
                    new Rectangle(
                        0,
                        0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height
                    ),
                    Color.White
                );
            }
            else if (pantallaActual == Pantalla.PantallaJuego)
            {

            }
            else if (pantallaActual == Pantalla.PantallaVictoria)
            {

            }
            else if (pantallaActual == Pantalla.PantallaDerrota)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}