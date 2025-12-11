namespace UmbrellaToolsKit.Interfaces
{
    public interface IGUID <T>
    {
        public string Id { get; }

        public T GetItemByID();
    }
}
