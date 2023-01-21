using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INodeOutPutle
    {
        INode Node { get; }
        Vector2 OutPosition { get; }
        List<INodeInPutle> NodesConnectionIn { get; set; }
        
        bool IsConnecting { get; }
        bool IsOverConnectorOutPut { get; }

        void AddNodeConnection(INodeInPutle node);
        void Desconnecting();
        void CancelConnection();
    }
}
