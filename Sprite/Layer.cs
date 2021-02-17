using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolKit.Sprite
{
    class Layer : GameObject
    {
        public List<List<List<int>>> tiles;


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            int _row = (int)((this.Scene.Camera.Position.X - this.Scene.Camera.Origin.X) / 8);
            int _rowWidth = (int)((this.Scene.Camera.Position.X - this.Scene.Camera.Origin.X + this.Scene.Sizes.X) / 8) + 1;
            int _column = (int)((this.Scene.Camera.Position.Y - this.Scene.Camera.Origin.Y) / 8);
            int _columnHeight = (int)((this.Scene.Camera.Position.Y - this.Scene.Camera.Origin.Y + this.Scene.Sizes.Y) / 8) + 1;

            _row = _row < 0 ? 0 : _row;
            _column = _column < 0 ? 0 : _column;
            _rowWidth = _rowWidth > this.tiles.Count() ? this.tiles.Count() : _rowWidth;
            _columnHeight = _columnHeight > this.tiles[0].Count() ? this.tiles[0].Count() : _columnHeight;

            for (int x = _column; x < _columnHeight; x++)
            {
                for(int y = _row; y < _rowWidth; y++)
                {
                    if (this.tiles[x][y][0] != -1)
                    {
                        this.Body = new Rectangle(this.tiles[x][y][0]*8, this.tiles[x][y][1]*8, 8, 8);
                        this.Position = new Vector2(y*8, x*8);
                        this.DrawSprite(spriteBatch);
                    }
                }
            }
        }
    }
}
