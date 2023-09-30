﻿using System;
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
        protected Vector2 _titleSize = new Vector2(200, 30);
        protected Vector2 _mainSquareSize = new Vector2(200, 30);
        protected bool _isDragableNode = true;
        private int _index = 0;
        private INode _parentNode;

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
                _position = value;
                //_storage.AddItemFloat($"position-{_index}-vector-x", new List<float>() { value.X });
                //_storage.AddItemFloat($"position-{_index}-vector-y", new List<float>() { value.Y });
            }
        }
        public Vector2 MainSquareSize { get => _mainSquareSize; }
        public Vector2 SelectedNodeSize { get => MainSquareSize + Vector2.One * 4; }
        public Vector2 SelectedNodePosition { get => Position - Vector2.One * 2f; }
        public Vector2 TitleSize { get => _titleSize; }
        public bool IsDragbleNode { get => _isDragableNode; set => _isDragableNode = value; }
        public INode ParentNode { get => _parentNode; set => _parentNode = value; }

        public Color TitleColor = Color.Blue;

        public float ItemPedding = 30f;

        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool CanMoveNode = false;

        public static event Action<BasicNode> OnSelectNode;
        public static BasicNode currentSelectedNode;

        public BasicNode(Load storage, int id, string name, Vector2 position)
        {
            _storage = storage;
            _index= id;
            Id = id;
            Name = name;
            Position = position;
        }

        public virtual void Update() => HandlerMoveNode();

        public void HandlerMoveNode()
        {
            if (!IsDragbleNode) return;

            var rectangle = new Rectangle(Position.ToPoint(), MainSquareSize.ToPoint());
            var mousePosition = Mouse.GetState().Position;
            var mousePoint = new Rectangle(mousePosition, new Point(1));

            if (rectangle.Contains(mousePoint)) CanMoveNode = true;
            if (MouseHandler.ButtonLeftReleased) CanMoveNode = false;

            if (CanMoveNode) MoveNode();
            else if (currentSelectedNode == this) currentSelectedNode = null;
        }

        public void MoveNode()
        {
            if (currentSelectedNode != this && currentSelectedNode != null) return;

            if (MouseHandler.ButtonLeftOneClick)
            {
                MousePosition = MouseHandler.Position;
                NodePositionOnClick = Position;
                currentSelectedNode = this;
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
            DrawSelectionArea(imDraw);
            DrawNodeSquare(imDraw);
            DrawNodeText(imDraw);
        }

        protected void DrawNodeText(ImDrawListPtr imDraw)
        {
            var titleTextPos = Position + Vector2.One * 8f;
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);
        }

        protected void DrawNodeSquare(ImDrawListPtr imDraw)
        {
            Primativas.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primativas.Square.Draw(imDraw, Position, TitleSize, TitleColor);
        }

        protected void DrawSelectionArea(ImDrawListPtr imDraw)
        {
            if (CanMoveNode)
                Primativas.Square.Draw(imDraw, SelectedNodePosition, SelectedNodeSize, Color.White);
        }
    }
}