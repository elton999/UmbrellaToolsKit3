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
            int _row = (int)(((Scene.Camera.Position.X - Origin.X) - Scene.Camera.Origin.X) / Scene.CellSize);
            int _rowWidth = (int)(((Scene.Camera.Position.X - Origin.X) - Scene.Camera.Origin.X + Scene.Sizes.X) / Scene.CellSize) + 1;
            int _column = (int)((Scene.Camera.Position.Y - Origin.Y - Scene.Camera.Origin.Y) / Scene.CellSize);
            int _columnHeight = (int)((Scene.Camera.Position.Y - Origin.Y - Scene.Camera.Origin.Y + Scene.Sizes.Y) / Scene.CellSize) + 1;

            _row = _row < 0 ? 0 : _row;
            _column = _column < 0 ? 0 : _column;
            _rowWidth = _rowWidth >= tiles[0].Count() ? tiles[0].Count() : _rowWidth;
            _columnHeight = _columnHeight >= tiles.Count() ? tiles.Count() : _columnHeight;

            for (int x = _column; x < _columnHeight; x++)
            {
                for (int y = _row; y < _rowWidth; y++)
                {
                    if (tiles[x][y][0] != -1)
                    {
                        Body = new Rectangle(tiles[x][y][0], tiles[x][y][1], Scene.CellSize, Scene.CellSize);
                        Position = new Vector2((y * Scene.CellSize) + (Origin.X * 2), x * Scene.CellSize + (Origin.Y * 2));
                        DrawSprite(spriteBatch);
                    }
                }
            }
            EndDraw(spriteBatch);
        }
    }
}