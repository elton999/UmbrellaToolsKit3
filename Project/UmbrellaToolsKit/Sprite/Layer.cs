using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Sprite
{
    class Layer : GameObject
    {
        public List<List<List<int>>> tiles;


        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            int _row = (int)(((this.Scene.Camera.Position.X - this.Origin.X) - this.Scene.Camera.Origin.X) / this.Scene.CellSize);
            int _rowWidth = (int)(((this.Scene.Camera.Position.X - this.Origin.X) - this.Scene.Camera.Origin.X + this.Scene.Sizes.X) / this.Scene.CellSize) + 1;
            int _column = (int)((this.Scene.Camera.Position.Y - this.Origin.Y - this.Scene.Camera.Origin.Y) / this.Scene.CellSize);
            int _columnHeight = (int)((this.Scene.Camera.Position.Y - this.Origin.Y - this.Scene.Camera.Origin.Y + this.Scene.Sizes.Y) / this.Scene.CellSize) + 1;

            _row = _row < 0 ? 0 : _row;
            _column = _column < 0 ? 0 : _column;
            _rowWidth = _rowWidth >= this.tiles[0].Count() ? this.tiles[0].Count() : _rowWidth;
            _columnHeight = _columnHeight >= this.tiles.Count() ? this.tiles.Count() : _columnHeight;

            for (int x = _column; x < _columnHeight; x++)
            {
                for (int y = _row; y < _rowWidth; y++)
                {
                    if (this.tiles[x][y][0] != -1)
                    {
                        this.Body = new Rectangle(this.tiles[x][y][0] * this.Scene.CellSize, this.tiles[x][y][1] * this.Scene.CellSize, this.Scene.CellSize, this.Scene.CellSize);
                        this.Position = new Vector2((y * this.Scene.CellSize) + (this.Origin.X * 2), x * this.Scene.CellSize + (this.Origin.Y * 2));
                        DrawSprite(spriteBatch);
                    }
                }
            }
            EndDraw(spriteBatch);
        }
    }
}
