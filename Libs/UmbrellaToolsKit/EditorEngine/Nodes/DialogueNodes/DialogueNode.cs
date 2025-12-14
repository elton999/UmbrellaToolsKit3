using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class DialogueNode : NodeWithOptions
    {
        public DialogueNode(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
        }
    }
}