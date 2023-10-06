using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.Storage;
namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class NodeOutPut : BasicNode, INodeOutPutle
    {
        public NodeOutPut(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            NodesConnectionIn = new List<INodeInPutle>();
        }

        private bool _isConnecting = false;

        public INode Node { get => this; }

        public Vector2 OutPosition { get => Position + TitleSize - (Vector2.UnitY * TitleSize.Y / 2f); }

        public List<INodeInPutle> NodesConnectionIn { get; set; }

        public bool IsConnecting => _isConnecting;

        public bool IsOverConnectorOutPut { get => isMouseOverPosition(OutPosition); }

        public override void Update()
        {
            base.Update();
            HandlerConnectionNodes();
        }

        public override void Draw(ImDrawListPtr imDraw)
        {
            DrawConnections(imDraw);
            base.Draw(imDraw);
            DrawOutputPoint(imDraw);
        }

        public override void OnSave()
        {
            base.OnSave();
            SaveConnections();
            _storage.SetString($"Nodes-Object-{Id}", typeof(NodeOutPut).Namespace + "." + typeof(NodeOutPut).Name);
        }

        public override void Load()
        {
            base.Load();
            var connections = _storage.getItemsFloat($"Nodes-Connection-In-{Id}");
            foreach (var connection in connections)
            {
                var node = DialogueData.Nodes.FindAll(x => x.Id == (int)connection)[0] as INodeInPutle;
                AddNodeConnection(node);
            }
        }

        protected void DrawOutputPoint(ImDrawListPtr imDraw)
        {
            imDraw.AddCircleFilled(OutPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
        }

        public void HandlerConnectionNodes()
        {
            HandlerDisconnectionNodes();

            _isConnecting = false;
            if (!IsOverConnectorOutPut) return;

            _isConnecting = true;
            if (MouseHandler.ButtonLeftReleased) return;

            CanMoveNode = true;
            DialogueEditorWindow.StartConnectingNodes(this);
        }

        public void DrawConnections(ImDrawListPtr imDraw)
        {
            foreach (var outputNode in NodesConnectionIn)
                Primativas.Line.Draw(imDraw, OutPosition, outputNode.InPosition);
        }

        public void HandlerDisconnectionNodes()
        {
            if (IsOverConnectorOutPut && MouseHandler.ButtonRightPressed)
                Disconnecting();
        }

        public void AddNodeConnection(INodeInPutle node)
        {
            if (NodesConnectionIn.Contains(node)) return;

            NodesConnectionIn.Add(node);
            node.AddNodeConnection(this);
            CancelConnection();
        }
        public void Disconnecting()
        {
            NodesConnectionIn.Clear();
        }

        public void CancelConnection() => _isConnecting = CanMoveNode = false;

        private bool isMouseOverPosition(Vector2 position) => (MouseHandler.Position - position).Length() <= 5.0f;

        private void SaveConnections()
        {
            var nodesConnectionInList = new List<float>();
            foreach (var nodeIn in NodesConnectionIn)
                nodesConnectionInList.Add(nodeIn.Node.Id);

            _storage.AddItemFloat($"Nodes-Connection-In-{Id}", nodesConnectionInList);
        }
    }
}
