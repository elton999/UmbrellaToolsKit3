using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Ogmo
{
    public class TilemapLayer : GameObject
    {
        private List<Tile> Items = new List<Tile>();

        public void addGameObject(Tile gameObject){
		    this.Items.Add(gameObject);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach(Tile tile in Items)
               tile.Draw(spriteBatch);
        }
    }
}
