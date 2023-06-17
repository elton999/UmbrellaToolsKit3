using System;

namespace UmbrellaToolsKit.Interfaces
{
    public interface IComponent : Interfaces.IUpdatable, IUpdatableData
    {
        IComponent Next { get; set; }
        GameObject GameObject { get; set; }

        void Init(GameObject gameObject);
        void Start();

        void Add(IComponent component);
        void Remove(IComponent component);

        T GetComponent<T>() where T : IComponent;
    }
}