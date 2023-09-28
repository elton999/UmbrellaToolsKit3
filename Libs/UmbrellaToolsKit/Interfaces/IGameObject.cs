using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Interfaces
{
    public interface IGameObject : IUpdatable, IUpdatableData, IDrawable, IDisposable
    {
        bool RemoveFromScene { get; set; }
        IComponent Components { get; set; }
        Scene Scene { get; set; }
        string Tag { get; set; }

        void Start();
        void OnVisible();
        void OnInvisible();
        void DrawBeforeScene(SpriteBatch spriteBatch);
        void Destroy();

        T AddComponent<T>() where T : IComponent;

        T GetComponent<T>() where T : IComponent;
    }
}