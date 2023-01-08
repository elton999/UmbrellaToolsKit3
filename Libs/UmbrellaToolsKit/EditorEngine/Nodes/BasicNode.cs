using ImGuiNET;
using MonoGame.ImGui.Standard.Extensions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using System;
using UmbrellaToolsKit.EditorEngine.Windows;

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

        public List<INodeInPutle> NodesConnection { get; set; }
        public float ItemPedding = 30f;

        public bool IsMouseClick = true;
        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool IsMouseOver = false;
        public bool IsConnecting { get => _isConnecting; }

        public BasicNode(string name, Vector2 position)
        {
            Name = name;
            NodesConnection = new List<INodeInPutle>();
            Position = position;
        }

        public void AddNodeConnection(INodeInPutle node)
        {
            if(!NodesConnection.Contains(node))
            {
                NodesConnection.Add(node);
                _isConnecting = false;
                IsMouseOver = false;
                IsMouseClick = true;
            }
        }

        public void Update()
        {
            CheckMouseOver();
            CheckMouseOverOutPut();

            if (IsMouseOver)
                Move();
        }

        public void CheckMouseOver()
        {
            var rectangle = new Rectangle(Position.ToPoint(), new Point(200,30));
            if (rectangle.Contains(new Rectangle(Mouse.GetState().Position, new Point(1, 1))) && !_isConnecting) 
                IsMouseOver = true;
            else
                IsMouseOver = IsMouseClick && IsMouseOver;
        }

        public void CheckMouseOverOutPut()
        {
            var outputVector = Mouse.GetState().Position.ToVector2();
            if ((outputVector - OutPosition).Length() >= 5.5f)
                return;

            _isConnecting = true;

            if (Mouse.GetState().LeftButton == ButtonState.Released)
                return;
           
            IsMouseOver = true;
            DialogueEditorWindow.StartConnnetingNodes(this);
        }

        public void Move()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !IsMouseClick)
            {
                IsMouseClick = true;
                MousePosition = Mouse.GetState().Position.ToVector2();
                NodePositionOnClick = Position;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
                IsMouseClick = false;

            if (IsMouseClick)
            {
                var direction = Mouse.GetState().Position.ToVector2() - MousePosition;
                Position = NodePositionOnClick + direction;
            }
        }

        public void Draw(ImDrawListPtr imDraw)
        {
            DrawConnections(imDraw);

            if (IsMouseOver)
                Primativas.Square.Draw(imDraw, SelectedNodePosition, SelectedNodeSize, Color.White);

            var titleTextPos = Position + Vector2.One * 8f;
            Primativas.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primativas.Square.Draw(imDraw, Position, TitleSize, TitleColor);
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);

            imDraw.AddCircleFilled(OutPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
            imDraw.AddCircleFilled(InPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
        }

        /*private void ShowItems(ImDrawListPtr imDraw)
        {
            int index = 0;
            foreach (var item in Items)
            {
                var itemPosition = Position + (ItemPedding / 2f * Vector2.UnitY);
                itemPosition += TitleSize * Vector2.UnitY;
                itemPosition += Vector2.UnitY * ItemPedding * index;
                itemPosition += Vector2.UnitX * 8f;

                imDraw.AddText(itemPosition.ToNumericVector2(), Color.White.PackedValue, item);
                index++;
            }
        }*/

        public void DrawConnections(ImDrawListPtr imDraw)
        {
            foreach(var outputNode in NodesConnection)
                Primativas.Line.Draw(imDraw, OutPosition, outputNode.InPosition);
        }
    }
}
