using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Sprite
{
    class GameObjectLayer : GameObject
    {
        public List<List<List<int>>> tiles;

        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            int row = (int)(((Scene.Camera.Position.X - Origin.X) - Scene.Camera.Origin.X) / Scene.CellSize);
            int rowWidth = (int)(((Scene.Camera.Position.X - Origin.X) - Scene.Camera.Origin.X + Scene.Sizes.X) / Scene.CellSize) + 1;
            int column = (int)((Scene.Camera.Position.Y - Origin.Y - Scene.Camera.Origin.Y) / Scene.CellSize);
            int columnHeight = (int)((Scene.Camera.Position.Y - Origin.Y - Scene.Camera.Origin.Y + Scene.Sizes.Y) / Scene.CellSize) + 1;

            row = row < 0 ? 0 : row;
            column = column < 0 ? 0 : column;
            rowWidth = rowWidth >= tiles[0].Count() ? tiles[0].Count() : rowWidth;
            columnHeight = columnHeight >= tiles.Count() ? tiles.Count() : columnHeight;

            for (int x = column; x < columnHeight; x++)
            {
                for (int y = row; y < rowWidth; y++)
                {
                    if (tiles[x][y][0] != -1)
                    {
                        Body = new Rectangle(tiles[x][y][0] * Scene.CellSize, tiles[x][y][1] * Scene.CellSize, Scene.CellSize, Scene.CellSize);
                        Position = new Vector2(y * Scene.CellSize + Origin.X * 2, x * Scene.CellSize + Origin.Y * 2);
                        DrawSprite(spriteBatch);
                    }
                }
            }
            EndDraw(spriteBatch);
        }

        public override void Dispose()
        {
            tiles.Clear();
            base.Dispose();
        }
    }
}
