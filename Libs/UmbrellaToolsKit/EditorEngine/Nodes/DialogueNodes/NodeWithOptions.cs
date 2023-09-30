using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class NodeWithOptions : NodeInPut, INodeOptions
    {
        private List<INodeOutPutle> _nodeOptions;
        private Vector2 _optionSize => Vector2.UnitY * 30;

        public NodeWithOptions(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            _nodeOptions = new List<INodeOutPutle>();
            UpdateBodyNodeSize();
            KeyBoardHandler.AddInput(Microsoft.Xna.Framework.Input.Keys.K);
        }

        public List<INodeOutPutle> NodeOptions { get => _nodeOptions; }

        public void AddNodeOption(INodeOutPutle node)
        {
            if (_nodeOptions.Contains(node)) return;
            _nodeOptions.Add(node);
        }

        public void CreateAnOption()
        {
            int id = DialogueData.GetNewNodeId();
            var node = new NodeOptionOutPut(_storage, id, $"option - {id}", Vector2.Zero) 
            { ParentNode = this };

            AddNodeOption(node);
            DialogueData.AddNode(node);
        }

        public override void Update()
        {
            base.Update();
            if (KeyBoardHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
                CreateAnOption();
            UpdateBodyNodeSize();
        }

        public override void Draw(ImDrawListPtr imDraw)
        {
            base.Draw(imDraw);
            for(int i = 0; i < NodeOptions.Count; i++)
            {
                var option = NodeOptions[i];
                int optionNumber = i + 1;

                Vector2 nodePosition = Position + _optionSize * (Vector2.UnitY * optionNumber);

                option.Node.Position = nodePosition;
            }
        }

        private void UpdateBodyNodeSize()
        {
            _mainSquareSize = new Vector2(200, 30);
            _mainSquareSize += Vector2.UnitY * NodeOptions.Count * _mainSquareSize.Y;
        }
    }
}
