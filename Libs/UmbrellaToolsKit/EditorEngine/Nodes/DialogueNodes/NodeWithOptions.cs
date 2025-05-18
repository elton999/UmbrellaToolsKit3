#if !RELEASE
using ImGuiNET;
#endif
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Fields;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows;
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

#if !RELEASE
        public override void Draw(ImDrawListPtr imDraw)
        {
            base.Draw(imDraw);
            for (int i = 0; i < NodeOptions.Count; i++)
            {
                var option = NodeOptions[i];
                int optionNumber = i + 1;

                Vector2 nodePosition = Position + _optionSize * (Vector2.UnitY * optionNumber);

                option.Position = nodePosition;
            }
        }
#endif

        public override void OnSave()
        {
            base.OnSave();
            _storage.SetString($"Nodes-Object-{Id}", typeof(NodeWithOptions).Namespace + "." + typeof(NodeWithOptions).Name);
        }

        public override void Load()
        {
            base.Load();
            var nodes = DialogueData.Nodes.FindAll(x => x.ParentNode != null && x.ParentNode.Id == this.Id);
            foreach (var node in nodes)
                AddNodeOption(node);
        }

#if !RELEASE
        private string _currentVariableFormName;
        public override void DrawInspector()
        {
            base.DrawInspector();
            for (int i = 0; i < NodeOptions.Count; i++)
            {
                if (ImGui.CollapsingHeader($"Option - {NodeOptions[i].Id}"))
                {
                    ImGui.Indent();
                    string stringValue = NodeOptions[i].Name;
                    Fields.Field.DrawString("Name option", ref stringValue);
                    NodeOptions[i].Name = stringValue;

                    NodeOptions[i].DrawInspector();
                    ImGui.Unindent();
                }
            }

            if (ImGui.Button("Add a new Option"))
                CreateAnOption<NodeOptionOutPut>();

            var options =  DialogueData.Fields.GetAllVariablesNames();

            if (options.Length <= 0) return; 
            bool treeNode = InspectorClass.DrawSeparator("Settings");
            if (treeNode)
            {
                ImGui.Indent();

                foreach (var field in VariableFields)
                {
                    switch (field.GetType())
                    {
                        case VariableType.INT:
                            Field.DrawInt(DialogueData.Fields.Variables[field.Id].Name, ref field.IntValue);
                            break;
                        case VariableType.FLOAT:
                            Field.DrawFloat(DialogueData.Fields.Variables[field.Id].Name, ref field.FloatValue);
                            break;
                        case VariableType.STRING:
                            Field.DrawString(DialogueData.Fields.Variables[field.Id].Name, ref field.StringValue);
                            break;
                    }
                }

                Field.DrawStringOptions("Variables", ref _currentVariableFormName, options);
                if(Buttons.BlueButton("Add field"))
                {
                    int id = DialogueData.Fields.GetIdByName(_currentVariableFormName);
                    VariableFields.Add(new VariableFields() { Id = id });
                }

                ImGui.Unindent();
            }
        }
#endif

        public override void OnDelete()
        {
            base.OnDelete();
            _storage.DeleteNode($"Nodes-Object-{Id}");

            foreach (var node in NodesConnectionOut)
                node.NodesConnectionIn.Remove(this);
            NodesConnectionOut.Clear();

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
