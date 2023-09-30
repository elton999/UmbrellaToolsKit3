using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class StartNode : NodeOutPut
    {
        public StartNode(Load storage, int id, Vector2 position) : base(storage, id, "start", position)
        {
            TitleColor = Color.Red;
            _titleSize = new Vector2(100, 30);
            _mainSquareSize = new Vector2(100, 30);
        }
    }
}
