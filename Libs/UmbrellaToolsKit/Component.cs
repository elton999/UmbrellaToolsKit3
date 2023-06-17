using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit
{
    public abstract class Component : IComponent
    {
        private IComponent _next;
        public IComponent Next { get => _next; set => _next = value; }
        public GameObject GameObject { get; set; }

        public void Init(GameObject gameObject)
        {
            GameObject = gameObject;
            Start();
        }

        public virtual void Start() { }

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

        public T GetComponent<T>() where T : IComponent
        {
            if (this is T t) return t;
            else if (Next != null) return Next.GetComponent<T>();
            else return default(T);
        }
    }
}