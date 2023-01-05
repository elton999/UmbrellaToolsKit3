using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Interfaces
{
    public interface IGameObject : IUpdatable, IUpdatableData, IDrawable, IDisposable
    {
        bool RemoveFromScene { get; set; }
        Vector2 Position { get; set; }
        IComponent Components { get; set; }
        string Tag { get; set; }    

        void OnVisible();
        void OnInvisible();
        void DrawBeforeScene(SpriteBatch spriteBatch);
        void Destroy();
    }
}