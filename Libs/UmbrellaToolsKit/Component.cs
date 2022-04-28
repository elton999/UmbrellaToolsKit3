using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit
{
    public abstract class Component : IComponent
    {
        private IComponent _next;
        public IComponent Next { get => _next; set => _next = value; }

        public virtual void Update(GameTime gameTime) => Next?.Update(gameTime);
        public virtual void UpdateData(GameTime gameTime) => Next?.UpdateData(gameTime);

        public void Add(IComponent component)
        {
            if (Next != null)
                Next.Add(component);
            else
                Next = component;
        }

        public void Remove(IComponent component)
        {
            if (component != Next)
                Next.Remove(component);
            else
                Next = Next.Next;
        }
    }
}