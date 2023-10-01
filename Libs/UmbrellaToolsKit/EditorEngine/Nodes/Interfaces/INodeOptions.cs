using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INodeOptions<T>
    {
        List<T> NodeOptions { get; }
        void CreateAnOption<Node>() where Node : BasicNode;
        void AddNodeOption(T node);
    }
}
