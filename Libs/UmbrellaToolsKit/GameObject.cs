using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.UI;

namespace UmbrellaToolsKit
{
    public class GameObject : IDisposable
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
        public string tag = "gameObject";

        public ContentManager Content;
        public Scene Scene;
        public Dictionary<string, string> Values;
        public List<Vector2> Nodes;
        public bool RemoveFromScene = false;

        public Vector2 InitialPosition;
        public static readonly Random getRandom = new Random();

        public virtual void Start() { }
        public virtual void OnVisible() { }
        public virtual void OnInvisible() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void UpdateData(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch, true);
            DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
        }

        public virtual void DrawBeforeScene(SpriteBatch spriteBatch) { }

        public Vector2 _bodySize;
        public float Density = 0f;
        public Vector2 TextureSize;

        public MouseManager _Mouse;
        public ScreenController _Screen;

        public SpriteSortMode SpriteSortMode = SpriteSortMode.Immediate;
        public SamplerState SamplerState = SamplerState.PointClamp;
        public BlendState BlendState = null;
        public Effect Effect = null;

        public float Radius;

        public virtual void IsVisible() { }
        public virtual void IsNotvisible() { }
        public virtual void OnCollision(string tag = null) { }
        public virtual void OnCollisionOut(string tag) { }
        public virtual void OnTriggerIn(string tag) { }
        public virtual void OnTriggerOut(string tag) { }
        public virtual void OnMouseOver() { }
        public virtual void Destroy()
        {
            this.RemoveFromScene = true;
        }

        public float lerp(float min, float max, float value)
        {
            return min + (max - min) * value;
        }

        public float LinearTween(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        public float EaseInQuad(float t, float b, float c, float d)
        {
            return c * (t /= d) * t + b;
        }

        public float EaseOutQuad(float t, float b, float c, float d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        public virtual void restart() { }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            if (this.Sprite != null)
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

        public void BeginDraw(SpriteBatch spriteBatch, bool hasCamera = true)
        {
            spriteBatch.Begin(
                this.SpriteSortMode,
                this.BlendState,
                this.SamplerState,
                null,
                null,
                this.Effect,
                this.Scene.Camera != null && hasCamera ? this.Scene.Camera.TransformMatrix() : null
            );
        }

        public void EndDraw(SpriteBatch spriteBatch) => spriteBatch.End();

        public virtual void Dispose() => GC.SuppressFinalize(this);
    }
}
