using System;
using ImGuiNET;
using System.Collections.Generic;
using MonoGame.ImGui.Standard.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode : INode
    {
        protected Load _storage;
        private int _index = 0;

        private Vector2 _position;
        private string _name;

        public int Id
        {
            get => _index;
            set
            {
                int itemsIdNum = _storage.getItemsFloat("Id").Count;

                if (_index + 1 > itemsIdNum)
                    _storage.AddItemFloat("Id", value);
                else
                    _storage.getItemsFloat("Id")[_index] = value;
                _index = value;
            }
        }

        public string Name
        { 
            get => _name;
            set
            {
                _storage.AddItemString($"name-{_index}", new List<string>() { value });
                _name = value;
            }
        }

        public Vector2 Position 
        {
            get => _position;
            set
            {
                _storage.AddItemFloat($"position-{_index}-vector-x", new List<float>() { value.X });
                _storage.AddItemFloat($"position-{_index}-vector-y", new List<float>() { value.Y });
                _position = value;
            }
        }
        public Vector2 MainSquareSize { get => (new Vector2(200, 50)); }
        public Vector2 SelectedNodeSize { get => MainSquareSize + Vector2.One * 4; }
        public Vector2 SelectedNodePosition { get => Position - Vector2.One * 2f; }
        public Vector2 TitleSize { get => new Vector2(200, 30); }

        public Color TitleColor = Color.Blue;

        public float ItemPedding = 30f;

        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool CanMoveNode = false;

        public static event Action<BasicNode> OnSelectNode;

        public BasicNode(Load storage, int id, string name, Vector2 position)
        {
            _storage = storage;
            _index= id;
            Id = id;
            Name = name;
            Position = position;
            _storage.Save();
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