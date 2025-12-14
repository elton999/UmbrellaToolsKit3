using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class DialogueNode : NodeWithOptions
    {
        protected override string _className => typeof(DialogueNode).Namespace + "." + typeof(DialogueNode).Name;

        public DialogueNode(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        { }

    }
}