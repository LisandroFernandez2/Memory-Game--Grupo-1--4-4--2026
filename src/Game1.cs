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
        private Texture2D BtnRetrocederNormal;
        private Texture2D BtnRetrocederPresionado;
        private Rectangle RectanguloBotonRetroceder;
        private bool BotonRetrocederPresionado;
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
            
            BtnRetrocederNormal = Content.Load<Texture2D>("btn_retroceder_normal");
            BtnRetrocederPresionado = Content.Load<Texture2D>("btn_retroceder_presionado");
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
            if (pantallaActual == Pantalla.PantallaNiveles)
            {
                if (mouse.LeftButton == ButtonState.Pressed &&
                    RectanguloBotonRetroceder.Contains(mouse.Position))
                {
                    pantallaActual = Pantalla.PantallaInicial;
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

                int anchoBoton = (int)(GraphicsDevice.Viewport.Width * 0.20f);

                int altoJugar = (int)(
                    BtnJugarNormal.Height *
                    ((float)anchoBoton / BtnJugarNormal.Width)
                );

                int altoSalir = (int)(
                    BtnSalirNormal.Height *
                    ((float)anchoBoton / BtnSalirNormal.Width)
                );

                int xJugar = (GraphicsDevice.Viewport.Width - anchoBoton) / 2;
                int xSalir = (GraphicsDevice.Viewport.Width - anchoBoton) / 2;

                int yJugar = (int)(GraphicsDevice.Viewport.Height * 0.40f);
                int ySalir = (int)(GraphicsDevice.Viewport.Height * 0.78f);

                RectanguloBotonJugar = new Rectangle(
                    xJugar,
                    yJugar,
                    anchoBoton,
                    altoJugar
                );

                RectanguloBotonSalir = new Rectangle(
                    xSalir,
                    ySalir,
                    anchoBoton,
                    altoSalir
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
                int anchoRetroceder = (int)(GraphicsDevice.Viewport.Width * 0.10f);

                int altoRetroceder = (int)(
                    BtnRetrocederNormal.Height *
                    ((float)anchoRetroceder / BtnRetrocederNormal.Width)
                );

                RectanguloBotonRetroceder = new Rectangle(
                    GraphicsDevice.Viewport.Width - anchoRetroceder - 40,
                    40,
                   anchoRetroceder,
                   altoRetroceder
                );

                if (BotonRetrocederPresionado)
                {
                    _spriteBatch.Draw(
                        BtnRetrocederPresionado,
                        RectanguloBotonRetroceder,
                        Color.White
                    );
                }
                else
                {
                    _spriteBatch.Draw(
                        BtnRetrocederNormal,
                        RectanguloBotonRetroceder,
                        Color.White
                    );
                }
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