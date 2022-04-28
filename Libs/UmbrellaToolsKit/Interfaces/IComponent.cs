namespace UmbrellaToolsKit.Interfaces
{
    public interface IComponent : Interfaces.IUpdatable, IUpdatableData
    {
        IComponent Next { get; set; }

        void Add(IComponent component);
        void Remove(IComponent component);
    }
}