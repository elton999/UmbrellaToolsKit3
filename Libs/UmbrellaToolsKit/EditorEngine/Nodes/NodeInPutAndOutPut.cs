using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.Input;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class NodeInPutAndOutPut : BasicNode,  INodeInPutle, INodeOutPutle
    {
        private bool _isConnecting = false;

        public List<INodeInPutle> NodesConnectionIn { get; set; }
        public List<INodeOutPutle> NodesConnectionOut { get; set; }

        public Vector2 OutPosition { get => Position + TitleSize - (Vector2.UnitY * TitleSize.Y / 2f); }
        public Vector2 InPosition { get => OutPosition - MainSquareSize * Vector2.UnitX; }
        public Vector2 BezierFactor { get => new Vector2(0.5f, 0.1f); }

        public bool IsConnecting { get => _isConnecting; }

        public bool IsOverConnectorIn { get => isMouseOverPosition(InPosition); }
        public bool IsOverConnectorOutPut { get => isMouseOverPosition(OutPosition); }

        public NodeInPutAndOutPut(string name, Vector2 position) : base(name, position)
        {
            NodesConnectionIn = new List<INodeInPutle>();
            NodesConnectionOut = new List<INodeOutPutle>();
        }

        public virtual void Update()
        {
            base.Update();
            HandlerConnectionNodes();
        }

        public virtual void Draw(ImDrawListPtr imDraw)
        {
            DrawConnections(imDraw);
            base.Draw(imDraw);
            imDraw.AddCircleFilled(OutPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
            imDraw.AddCircleFilled(InPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
        }

        public void DrawConnections(ImDrawListPtr imDraw)
        {
            foreach (var outputNode in NodesConnectionIn)
                Primativas.Line.Draw(imDraw, OutPosition, outputNode.InPosition);
        }

        public void HandlerConnectionNodes()
        {
            HandlerDesconnectionNodes();

            _isConnecting = false;
            if (!IsOverConnectorOutPut) return;

            _isConnecting = true;
            if (MouseHandler.ButtonLeftReleased) return;

            CanMoveNode = true;
            DialogueEditorWindow.StartConnnetingNodes(this);
        }

        public void HandlerDesconnectionNodes()
        {
            if (IsOverConnectorOutPut && MouseHandler.ButtonRightPressed)
                Desconnecting();
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

        public void Desconnecting()
        {
            NodesConnectionOut.Clear();
            NodesConnectionIn.Clear();
        }

        public void CancelConnection() => _isConnecting = CanMoveNode = false;

        private bool isMouseOverPosition(Vector2 position) => (MouseHandler.Position - position).Length() <= 5.0f;
    }
}
