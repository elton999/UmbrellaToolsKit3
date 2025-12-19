using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    [NodeImplementation("DialogueNodes")]
    public class StartNode : NodeOutPut
    {
        public StartNode(Load storage, int id, string name, Vector2 position) : base(storage, id, "start", position)
        {
            TitleColor = Color.Red;
            _titleSize = new Vector2(100, 30);
            _mainSquareSize = new Vector2(100, 30);
        }

        protected override string _className => typeof(StartNode).Namespace + "." + typeof(StartNode).Name;
    }
}
