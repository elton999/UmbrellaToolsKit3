using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INodeOutPutle
    {
        Vector2 OutPosition { get; }
        List<INodeInPutle> NodesConnection { get; set; }
        bool IsConnecting { get; }

        void AddNodeConnection(INodeInPutle node);
    }
}
