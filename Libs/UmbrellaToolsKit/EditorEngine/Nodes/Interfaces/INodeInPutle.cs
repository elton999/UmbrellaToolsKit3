using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INodeInPutle
    {
        INode Node { get; }
        Vector2 InPosition { get; }
        List<INodeOutPutle> NodesConnectionOut { get; set; }
        bool IsOverConnectorIn { get; }

        void AddNodeConnection(INodeOutPutle node);
    }
}
