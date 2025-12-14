using Microsoft.Xna.Framework;
using System.Collections.Generic;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class EndNode : NodeInPut
    {
        public EndNode(Load storage, int id, string name, Vector2 position) : base(storage, id, "End", position)
        {
            TitleColor = Color.Red;
            _titleSize = new Vector2(100, 30);
            _mainSquareSize = new Vector2(100, 30);
        }

        protected override string _className => typeof(EndNode).Namespace + "." + typeof(EndNode).Name;
    }
}
