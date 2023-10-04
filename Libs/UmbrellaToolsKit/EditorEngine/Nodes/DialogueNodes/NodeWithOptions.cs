using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class NodeWithOptions : NodeInPut, INodeOptions<BasicNode>
    {
        private List<BasicNode> _nodeOptions;
        private Vector2 _optionSize => Vector2.UnitY * 30;

        public NodeWithOptions(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            _nodeOptions = new List<BasicNode>();
            UpdateBodyNodeSize();
        }

        public List<BasicNode> NodeOptions { get => _nodeOptions; }

        public void AddNodeOption(BasicNode node)
        {
            if (_nodeOptions.Contains(node)) return;
            _nodeOptions.Add(node);
        }

        public void CreateAnOption<Node>() where Node : BasicNode
        {
            int id = DialogueData.GetNewNodeId();
            var node = (Node)Activator.CreateInstance(typeof(Node), _storage, id, $"option - {id}", Vector2.Zero);
            node.ParentNode = this;

            AddNodeOption(node);
            DialogueData.AddNode(node);
        }

        public override void Update()
        {
            base.Update();
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

                option.Position = nodePosition;
            }
        }

        public override void OnSave()
        {
            base.OnSave();
            _storage.AddItemString($"Nodes-Object-{Id}", new List<string>() { typeof(NodeWithOptions).Namespace + "." + typeof(NodeWithOptions).Name });
        }

        public override void DrawInspector()
        {
            base.DrawInspector();
            for (int i = 0; i < NodeOptions.Count; i++)
            {
                string optionName = $"Option ({i + 1}) #ID{NodeOptions[i].Id}";
                NodeOptions[i].Name = optionName;
                if (ImGui.CollapsingHeader(optionName))
                {
                    ImGui.Indent();
                    NodeOptions[i].DrawInspector();
                    ImGui.Unindent();
                }
            }
            if (ImGui.Button("Add a new Option"))
                CreateAnOption<NodeOptionOutPut>();
        }

        public override void OnDelete()
        {
            foreach (var node in NodesConnectionOut)
                node.NodesConnectionIn.Remove(this);
            NodesConnectionOut.Clear();

            DialogueData.RemoveNode(this);
            var nodeOptions = new List<BasicNode>(NodeOptions);
            foreach (var node in nodeOptions)
                node.OnDelete();
        }

        private void UpdateBodyNodeSize()
        {
            _mainSquareSize = new Vector2(200, 30);
            _mainSquareSize += Vector2.UnitY * NodeOptions.Count * _mainSquareSize.Y;
        }
    }
}
