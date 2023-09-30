using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INodeOptions
    {
        List<INodeOutPutle> NodeOptions { get; }
        void CreateAnOption();
        void AddNodeOption(INodeOutPutle node);
    }
}
