using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace UmbrellaToolsKit.UI
{
    public class MouseManager : GameObject
    {
        public bool Show = false;

        public override void Update(GameTime gameTime)
        {
            this.Position.X = Mouse.GetState().Position.X;
            this.Position.Y = Mouse.GetState().Position.Y;

            // this.CBody.SetTransform(this.Position, 0f);

           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(this.Sprite, this.Position, null, this.SpriteColor, this.Rotation, Vector2.Zero, this.Scale, this.spriteEffect, 0);
            if(this.Show) this.DrawSprite(spriteBatch);
        }

    }
}
