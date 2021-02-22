using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit.UI;

namespace UmbrellaToolKit
{
    public class GameObject
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin = Vector2.Zero;
        public Point size;
        public float Scale = 1;
        public Rectangle Body;
        public float Rotation = 0;
        public Color SpriteColor = Color.White;
        public SpriteEffects spriteEffect = SpriteEffects.None;
        public Texture2D Sprite;
        public float Transparent = 1f;

        public ContentManager Content;
        public Scene Scene;
        public bool RemoveFromScene = false;

        public virtual void Start () { }
        public virtual void OnVisible () { }
        public virtual void OnInvisible () { }
        public virtual void Update (GameTime gameTime) { }
        public virtual void UpdateData(GameTime gameTime) { }
        public virtual void Draw (SpriteBatch spriteBatch) { }



        public Vector2 _bodySize;
        public float Density = 0f;
        public Vector2 TextureSize;

        public MouseManager _Mouse;
        public ScreemController _Screem;


        public float Radius;

        public virtual void OnCollision(string tag = null) { }
        public virtual void OnCollisionOut(string tag) { }
        public virtual void OnTriggerIn(string tag) { }
        public virtual void OnTriggerOut(string tag) { }
        public virtual void OnMouseOver() { }
        public virtual void Destroy() { }

        

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            if(this.Sprite != null)
            {
                if (this.Body.IsEmpty)
                {
                    spriteBatch.Draw(this.Sprite, this.Position, null, this.SpriteColor * this.Transparent, this.Rotation, this.Origin, this.Scale, this.spriteEffect, 0);
                }
                else
                {
                    spriteBatch.Draw(this.Sprite, this.Position, this.Body, this.SpriteColor * this.Transparent, this.Rotation, this.Origin, this.Scale, this.spriteEffect, 0);
                }
               
            }
        }
    }
}
