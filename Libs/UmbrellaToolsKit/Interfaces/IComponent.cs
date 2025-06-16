using System.ComponentModel;

namespace UmbrellaToolsKit.Interfaces
{
    public interface IComponent : Interfaces.IUpdatable, IUpdatableData, IAfterUpdatable
    {
        IComponent Next { get; set; }
        GameObject GameObject { get; set; }

        void Init(GameObject gameObject);
        void Start();

        void Add(IComponent component);
        void Remove(IComponent component);

        void Destroy();
        void OnDestroy();

        T GetComponent<T>() where T : IComponent;
    }
}