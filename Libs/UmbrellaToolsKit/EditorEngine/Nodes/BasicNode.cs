using System;
using ImGuiNET;
using MonoGame.ImGui.Standard.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode : INode
    {
        private Vector2 _position = new Vector2(500, 100);

        public string Name { get; set; }

        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 MainSquareSize { get => (new Vector2(200, 50)); } //+ Vector2.UnitY * (NodesConnection.Count * ItemPedding); }
        public Vector2 SelectedNodeSize { get => MainSquareSize + Vector2.One * 4; }
        public Vector2 SelectedNodePosition { get => Position - Vector2.One * 2f; }
        public Vector2 TitleSize { get => new Vector2(200, 30); }

        public Color TitleColor = Color.Blue;

        public float ItemPedding = 30f;

        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool CanMoveNode = false;

        public static event Action<BasicNode> OnSelectNode;

        public BasicNode(string name, Vector2 position)
        {
            Name = name;
            Position = position;
        }

        public virtual void Update() => HandlerMoveNode();

        public void HandlerMoveNode()
        {
            var rectangle = new Rectangle(Position.ToPoint(), MainSquareSize.ToPoint());
            var mousePosition = Mouse.GetState().Position;
            var mousePoint = new Rectangle(mousePosition, new Point(1));

            if (rectangle.Contains(mousePoint))
                CanMoveNode = true;
            
            if (MouseHandler.ButtonLeftReleased) CanMoveNode = false;

            if (CanMoveNode) MoveNode();
        }

        public void MoveNode()
        {
            if (MouseHandler.ButtonLeftOneClick)
            {
                MousePosition = MouseHandler.Position;
                NodePositionOnClick = Position;
                OnSelectNode?.Invoke(this);
            }

            if (MouseHandler.ButtonLeftPressing)
            {
                var currentMousePosition = MouseHandler.Position;
                var direction = currentMousePosition - MousePosition;
                Position = NodePositionOnClick + direction;
            }
        }

        public virtual void Draw(ImDrawListPtr imDraw)
        {
            if (CanMoveNode)
                Primativas.Square.Draw(imDraw, SelectedNodePosition, SelectedNodeSize, Color.White);

            var titleTextPos = Position + Vector2.One * 8f;
            Primativas.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primativas.Square.Draw(imDraw, Position, TitleSize, TitleColor);
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);
        }

    }
}