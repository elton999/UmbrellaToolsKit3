using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.TileMap
{
    public class TileMapLayer : GameObject
    {
        private List<Tile> Items = new List<Tile>();

        public void addGameObject(Tile gameObject) => Items.Add(gameObject);

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Tile tile in Items)
                tile.Draw(spriteBatch);
        }
    }
}
