using System.Collections.Generic;
#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class NodeInPutAndOutPut : BasicNode, INodeInPutle, INodeOutPutle
    {
        private bool _isConnecting = false;

        public INode Node { get => this; }

        public List<INodeInPutle> NodesConnectionIn { get; set; }
        public List<INodeOutPutle> NodesConnectionOut { get; set; }

        public Vector2 OutPosition { get => Position + TitleSize - (Vector2.UnitY * TitleSize.Y / 2f); }
        public Vector2 InPosition { get => OutPosition - MainSquareSize * Vector2.UnitX; }
        public Vector2 BezierFactor { get => new Vector2(0.5f, 0.1f); }

        public bool IsConnecting { get => _isConnecting; }

        public bool IsOverConnectorIn { get => isMouseOverPosition(InPosition); }
        public bool IsOverConnectorOutPut { get => isMouseOverPosition(OutPosition); }

        public NodeInPutAndOutPut(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            NodesConnectionIn = new List<INodeInPutle>();
            NodesConnectionOut = new List<INodeOutPutle>();
        }

        public override void Update()
        {
            base.Update();
            HandlerConnectionNodes();
        }
#if !RELEASE
        public override void Draw(ImDrawListPtr imDraw)
        {
            DrawConnections(imDraw);
            base.Draw(imDraw);
            imDraw.AddCircleFilled(OutPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
            imDraw.AddCircleFilled(InPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
        }
#endif
        public override void OnSave()
        {
            base.OnSave();
            
            SaveConnections();
        }
#if !RELEASE
        public void DrawConnections(ImDrawListPtr imDraw)
        {
            foreach (var outputNode in NodesConnectionIn)
                Primitives.Line.Draw(imDraw, OutPosition, outputNode.InPosition);
        }
#endif
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

        public void HandlerDisconnectionNodes()
        {
            if (IsOverConnectorOutPut && MouseHandler.ButtonRightPressed)
                Disconnecting();
        }

        public override void OnDelete()
        {
            base.OnDelete();
            _storage.DeleteNode($"Nodes-Connection-In-{Id}");
            _storage.DeleteNode($"Nodes-Connection-Out-{Id}");
        }

        public void AddNodeConnection(INodeInPutle node)
        {
            if (NodesConnectionIn.Contains(node)) return;

            NodesConnectionIn.Add(node);
            node.AddNodeConnection(this);
            CancelConnection();
        }

        public void AddNodeConnection(INodeOutPutle node)
        {
            if (NodesConnectionOut.Contains(node)) return;
            NodesConnectionOut.Add(node);
        }

        public void Disconnecting()
        {
            NodesConnectionOut.Clear();
            NodesConnectionIn.Clear();
        }

        public void CancelConnection() => _isConnecting = CanMoveNode = false;

        private bool isMouseOverPosition(Vector2 position) => (MouseHandler.Position - position).Length() <= 5.0f;

        private void SaveConnections()
        {
            var nodesConnectionInList = new List<float>();
            foreach (var nodeIn in NodesConnectionIn)
                nodesConnectionInList.Add(nodeIn.Node.Id);

            var nodesConnectionOutList = new List<float>();
            foreach (var nodeOut in NodesConnectionOut)
                nodesConnectionOutList.Add(nodeOut.Node.Id);

            _storage.AddItemFloat($"Nodes-Connection-In-{Id}", nodesConnectionInList);
            _storage.AddItemFloat($"Nodes-Connection-Out-{Id}", nodesConnectionOutList);
        }
    }
}
