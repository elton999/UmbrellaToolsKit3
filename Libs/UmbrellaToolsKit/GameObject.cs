using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit
{
    public class GameObject : IGameObject
    {
        private IComponent _components;
        private bool _removeFromScene = false;
        private Vector2 _position = Vector2.Zero;
        private Scene _scene;

        public IComponent Components { get => _components; set => _components = value; }
        public bool RemoveFromScene { get => _removeFromScene; set => _removeFromScene = value; }
        public Vector2 Position { get => _position; set => _position = value; }

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
        public string Tag { get => tag; set => tag = value; }

        public ContentManager Content;
        public Scene Scene { get => _scene; set => _scene = value; }
        public dynamic Values;
        public List<Vector2> Nodes;

        public Vector2 InitialPosition;

        public Vector2 _bodySize;
        public float Density = 0f;
        public Vector2 TextureSize;

        public ScreenController _Screen;

        public SpriteSortMode SpriteSortMode = SpriteSortMode.Immediate;
        public SamplerState SamplerState = SamplerState.PointClamp;
        public BlendState BlendState = null;
        public Effect Effect = null;

        public float Radius;

        public virtual void Start() { }
        public virtual void OnVisible() { }
        public virtual void OnInvisible() { }

        public virtual void Update(GameTime gameTime)
        {
            if (Components != null) Components.Update(gameTime);
        }

        public virtual void UpdateData(GameTime gameTime)
        {
            if (Components != null) Components.UpdateData(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch, true);
            DrawSprite(spriteBatch);
            EndDraw(spriteBatch);
        }

        public virtual void DrawBeforeScene(SpriteBatch spriteBatch) { }

        public virtual void IsVisible() { }
        public virtual void IsNotvisible() { }
        public virtual void OnCollision(string tag = null) { }
        public virtual void OnCollisionOut(string tag) { }
        public virtual void OnTriggerIn(string tag) { }
        public virtual void OnTriggerOut(string tag) { }
        public virtual void OnMouseOver() { }
        public virtual void Destroy() => RemoveFromScene = true;

        public virtual Collision.Actor GetActor() => default(Collision.Actor);
        public virtual Collision.Solid GetSolid() => default(Collision.Solid);

        public virtual void restart() { }

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
                spriteBatch.Draw(Sprite, Position, Body.IsEmpty ? null : Body, SpriteColor * Transparent, Rotation, Origin, Scale, spriteEffect, 0);
        }

        public void BeginDraw(SpriteBatch spriteBatch, bool hasCamera = true)
        {
            spriteBatch.Begin(
                SpriteSortMode,
                BlendState,
                SamplerState,
                null,
                null,
                Effect,
                Scene.Camera != null && hasCamera ? Scene.Camera.TransformMatrix() : null
            );
        }

        public void EndDraw(SpriteBatch spriteBatch) => spriteBatch.End();

        public virtual void Dispose() => GC.SuppressFinalize(this);

        public T AddComponent<T>() where T : IComponent
        {
            var componentInstance = System.Activator.CreateInstance(typeof(T));
            IComponent component = (IComponent)componentInstance;

            if (_components != null) _components.Add((IComponent)component);
            else _components = (IComponent)component;

            component.Init(this);

            return (T)componentInstance;
        }

        public T GetComponent<T>() where T : IComponent
        {
            if (Components != null) return Components.GetComponent<T>();
            return default(T);
        }
    }
}