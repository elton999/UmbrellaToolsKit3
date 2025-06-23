using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Interfaces
{
    public interface ISprite : ITexture
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public Rectangle GetRectangle();
    }
}
