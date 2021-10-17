using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Ogmo
{
    public class Tile : GameObject
    {
        public String tag;
        public Boolean BrickColor = false;
        public Point size;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.BrickColor)
            {
                for (var x = 0; x < this.size.X; x++)
                {
                    for (var y = 0; y < this.size.X; y++)
                    {
                        spriteBatch.Draw(
                            this.Sprite,
                            new Vector2(this.Position.X + (8 * x), this.Position.Y + (8 * y)),
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
            else
                this.DrawSprite(spriteBatch);
        }
    }
}
