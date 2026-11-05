using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
namespace juegojuego;

// Clase que une una imagen textura con su posicion en pantalla
public class Sprite
{
    // La imagen que se dibuja
    public Texture2D textura;
    // Posicion x y en pantalla en pixeles
    public Vector2 position;

    // Constructor
    public Sprite(Texture2D textura, Vector2 position)
    {
        this.textura = textura;
        this.position = position;

    }

}