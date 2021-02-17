using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolKit.Tiled
{
    public class Tile
    {
        public Rectangle TilePosition { get; set; }
        public Rectangle TileCollision { get; set; }
        public bool Collision { get; set; }
        public string Type;

        public Tile(Rectangle TilePosition, Rectangle TileCollision, bool Collision, string TagName)
        {
            this.TilePosition = TilePosition;
            this.TileCollision = TileCollision;
            this.Collision = Collision;
            this.Type = TagName;
        }
    }
}
