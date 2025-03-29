using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.Storage;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Input;
#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class NodeInPut : BasicNode, INodeInPutle
    {
        public NodeInPut(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            NodesConnectionOut = new List<INodeOutPutle>();
        }

        public INode Node => this;

        public Vector2 InPosition { get => Position + (Vector2.UnitY * TitleSize.Y / 2f); }

        public List<INodeOutPutle> NodesConnectionOut { get; set; }

        public bool IsOverConnectorIn { get => isMouseOverPosition(InPosition); }

        public override void Update()
        {
            base.Update();
        }

#if !RELEASE
        public override void Draw(ImDrawListPtr imDraw)
        {
            base.Draw(imDraw);
            imDraw.AddCircleFilled(InPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
        }
#endif

        public override void OnSave()
        {
            SaveConnections();
            base.OnSave();
        }

        public override void Load()
        {
            base.Load();
            var connections = _storage.getItemsFloat($"Nodes-Connection-Out-{Id}");
            foreach(var connection in connections)
            {
                var node = DialogueData.Nodes.FindAll(x => x.Id == (int)connection)[0] as INodeOutPutle;
                AddNodeConnection(node);
            }
        }

        public override void OnDelete()
        {
            base.OnDelete();
            _storage.DeleteNode($"Nodes-Connection-Out-{Id}");
        }
#if !RELEASE
        public void DrawConnections(ImDrawListPtr imDraw)
        {
            foreach (var outputNode in NodesConnectionOut)
                Primativas.Line.Draw(imDraw, InPosition, outputNode.OutPosition);
        }
#endif
        public void AddNodeConnection(INodeOutPutle node)
        {
            if (NodesConnectionOut.Contains(node)) return;
            NodesConnectionOut.Add(node);
        }

        private bool isMouseOverPosition(Vector2 position) => (MouseHandler.Position - position).Length() <= 5.0f;

        private void SaveConnections()
        {
            var nodesConnectionOutList = new List<float>();
            foreach (var nodeOut in NodesConnectionOut)
                nodesConnectionOutList.Add(nodeOut.Node.Id);

            _storage.AddItemFloat($"Nodes-Connection-Out-{Id}", nodesConnectionOutList);
        }
    }
}
