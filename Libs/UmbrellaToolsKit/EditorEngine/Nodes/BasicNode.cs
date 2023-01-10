using ImGuiNET;
using MonoGame.ImGui.Standard.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode : INodeInPutle, INodeOutPutle
    {
        private bool _isConnecting = false;

        public string Name { get; set; }

        public Vector2 Position = new Vector2(500, 100);
        public Vector2 MainSquareSize { get => (new Vector2(200, 50)); } //+ Vector2.UnitY * (NodesConnection.Count * ItemPedding); }
        public Vector2 SelectedNodeSize { get => MainSquareSize + Vector2.One * 4; }
        public Vector2 SelectedNodePosition { get => Position - Vector2.One * 2f; }
        public Vector2 TitleSize = new Vector2(200, 30);

        public Vector2 OutPosition { get => Position + TitleSize - (Vector2.UnitY * TitleSize.Y / 2f); }
        public Vector2 InPosition { get => OutPosition - MainSquareSize * Vector2.UnitX; }
        public Vector2 BezierFactor { get => new Vector2(0.5f, 0.1f); }

        public Color TitleColor = Color.Blue;

        public List<INodeInPutle> NodesConnectionIn { get; set; }
        public List<INodeOutPutle> NodesConnectionOut { get; set; }
        public float ItemPedding = 30f;

        public bool IsMouseLeftButtonPressing = true;
        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool CanMoveNode = false;
        public bool IsConnecting { get => _isConnecting; }

        public bool IsOverConnectorIn { get => isMouseOverPosition(InPosition); }
        public bool IsOverConnectorOutPut { get => isMouseOverPosition(OutPosition); }

        public BasicNode(string name, Vector2 position)
        {
            Name = name;
            NodesConnectionIn = new List<INodeInPutle>();
            NodesConnectionOut= new List<INodeOutPutle>();
            Position = position;
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

        public void CancelConnection()
        {
            _isConnecting = CanMoveNode = false;
            IsMouseLeftButtonPressing = true;
        }

        public void Update()
        {
            HandlerMoveNode();
            HandlerConnectionNodes();
        }

        public void HandlerMoveNode()
        {
            var rectangle = new Rectangle(Position.ToPoint(), MainSquareSize.ToPoint());
            var mousePosition = Mouse.GetState().Position;
            var mousePoint = new Rectangle(mousePosition, new Point(1));

            if (rectangle.Contains(mousePoint) && !_isConnecting) CanMoveNode = true;
            else CanMoveNode = IsMouseLeftButtonPressing && CanMoveNode;

            if (CanMoveNode) MoveNode();
        }

        public void HandlerConnectionNodes()
        {
            HandlerDesconnectionNodes();

            _isConnecting = false;
            if (!IsOverConnectorOutPut) return;

            _isConnecting = true;
            if (Mouse.GetState().LeftButton == ButtonState.Released) return;
           
            CanMoveNode = true;
            DialogueEditorWindow.StartConnnetingNodes(this);
        }

        public void HandlerDesconnectionNodes()
        {
            if (IsOverConnectorOutPut && Mouse.GetState().RightButton == ButtonState.Pressed)
                Desconnecting();
        }

        public void MoveNode()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !IsMouseLeftButtonPressing)
            {
                IsMouseLeftButtonPressing = true;
                MousePosition = Mouse.GetState().Position.ToVector2();
                NodePositionOnClick = Position;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
                IsMouseLeftButtonPressing = false;

            if (IsMouseLeftButtonPressing)
            {
                var currentMousePosition = Mouse.GetState().Position.ToVector2();
                var direction = currentMousePosition - MousePosition;
                Position = NodePositionOnClick + direction;
            }
        }

        public void Draw(ImDrawListPtr imDraw)
        {
            DrawConnections(imDraw);

            if (CanMoveNode)
                Primativas.Square.Draw(imDraw, SelectedNodePosition, SelectedNodeSize, Color.White);

            var titleTextPos = Position + Vector2.One * 8f;
            Primativas.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primativas.Square.Draw(imDraw, Position, TitleSize, TitleColor);
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);

            imDraw.AddCircleFilled(OutPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
            imDraw.AddCircleFilled(InPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
        }

        public void DrawConnections(ImDrawListPtr imDraw)
        {
            foreach(var outputNode in NodesConnectionIn)
                Primativas.Line.Draw(imDraw, OutPosition, outputNode.InPosition);
        }
        
        private bool isMouseOverPosition(Vector2 position) => (Mouse.GetState().Position.ToVector2() - position).Length() <= 5.0f;
    }
}
