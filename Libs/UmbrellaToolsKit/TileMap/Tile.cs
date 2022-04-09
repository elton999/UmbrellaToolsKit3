using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.TileMap
{
    public class Tile : GameObject
    {
        public Boolean BrickColor = false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.BrickColor)
                DrawBrick(spriteBatch);
            else
                this.DrawSprite(spriteBatch);
        }

        private void DrawBrick(SpriteBatch spriteBatch)
        {
            for (var x = 0; x < this.size.X; x++)
            {
                for (var y = 0; y < this.size.X; y++)
                {
                    var brickSize = Vector2.Multiply(new Vector2(x, y), 8);
                    spriteBatch.Draw(
                        this.Sprite,
                        Vector2.Add(Position, brickSize),
                        this.Body,
                        this.SpriteColor * this.Transparent,
                        this.Rotation,
                        this.Origin,
                        this.Scale,
                        this.spriteEffect, 0
                    );
                }
            }
        }
    }
}
