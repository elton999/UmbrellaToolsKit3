using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Sprite
{
    public class BoxSprite : GameObject
    {
        public Rectangle LeftTopBoxSprite;
        public Rectangle RightTopBoxSprite;
        public Rectangle LeftBottomBoxSprite;
        public Rectangle RightBottomBoxSprite;

        public Rectangle TopBoxSprite;
        public Rectangle BottomBoxSprite;
        public Rectangle LeftBoxSprite;
        public Rectangle RightBoxSprite;

        public Rectangle ContentBoxSprite;
        public Vector2 ContentSize;


        private void DrawRectanglePart(SpriteBatch spriteBatch, Vector2 _Position, Rectangle _Rectangle)
        {
            spriteBatch.Draw(
                        this.Sprite,
                        _Position,
                        _Rectangle,
                        this.SpriteColor * this.Transparent,
                        this.Rotation,
                        this.Origin,
                        this.Scale,
                        this.spriteEffect,
                        1f
                        );
        }

        public void DrawRectangle(SpriteBatch spriteBatch)
        {
            float xx = ContentSize.X / ContentBoxSprite.Size.X;
            float yy = ContentSize.Y / ContentBoxSprite.Size.Y;
            // content
            for (int x = 0; x < xx; x++)
            {

                // Top Line
                this.DrawRectanglePart(
                       spriteBatch,
                       new Vector2(this.Position.X + this.LeftTopBoxSprite.Size.X + (x * this.TopBoxSprite.Size.X), this.Position.Y),
                       this.TopBoxSprite
                       );

                // Bottom Line
                this.DrawRectanglePart(
                       spriteBatch,
                       new Vector2(this.Position.X + this.ContentBoxSprite.Size.X + (x * this.BottomBoxSprite.Size.X), this.Position.Y + this.ContentBoxSprite.Size.Y + (yy * this.ContentBoxSprite.Size.Y)),
                       this.BottomBoxSprite
                       );


                for (int y = 0; y < yy; y++)
                {
                    this.DrawRectanglePart(
                        spriteBatch,
                        new Vector2(this.Position.X + this.LeftTopBoxSprite.Size.X + (x * this.ContentBoxSprite.Size.X), this.Position.Y + this.LeftTopBoxSprite.Size.Y + (y * this.ContentBoxSprite.Size.Y)),
                        this.ContentBoxSprite
                        );

                    if (x == 0)
                    {
                        // Left Line
                        this.DrawRectanglePart(
                               spriteBatch,
                               new Vector2(this.Position.X, this.Position.Y + this.LeftTopBoxSprite.Size.Y + (y * this.ContentBoxSprite.Size.Y)),
                               this.LeftBoxSprite
                               );

                        // Right Line
                        this.DrawRectanglePart(
                               spriteBatch,
                               new Vector2(this.Position.X + this.LeftTopBoxSprite.Size.X + (xx * this.ContentBoxSprite.Size.X), this.Position.Y + this.RightTopBoxSprite.Size.Y + (y * this.ContentBoxSprite.Size.Y)),
                               this.RightBoxSprite
                               );
                    }
                }
            }

            // left Top
            this.DrawRectanglePart(
                       spriteBatch,
                       new Vector2(this.Position.X, this.Position.Y),
                       this.LeftTopBoxSprite
                       );

            // right Top
            this.DrawRectanglePart(
                       spriteBatch,
                       new Vector2(this.Position.X + this.ContentBoxSprite.Size.X + (xx * this.ContentBoxSprite.Size.X), this.Position.Y),
                       this.RightTopBoxSprite
                       );

            // left Bottom
            this.DrawRectanglePart(
                       spriteBatch,
                       new Vector2(this.Position.X, this.Position.Y + this.ContentBoxSprite.Size.Y + (yy * this.ContentBoxSprite.Size.Y)),
                       this.LeftBottomBoxSprite
                       );

            // right Bottom
            this.DrawRectanglePart(
                       spriteBatch,
                       new Vector2(this.Position.X + this.ContentBoxSprite.Size.X + (xx * this.ContentBoxSprite.Size.X), this.Position.Y + this.ContentBoxSprite.Size.Y + (yy * this.ContentBoxSprite.Size.Y)),
                       this.RightBottomBoxSprite
                       );
        }

       }
}
