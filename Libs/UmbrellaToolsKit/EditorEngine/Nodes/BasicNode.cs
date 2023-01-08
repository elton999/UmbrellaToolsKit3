using ImGuiNET;
using System;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode
    {
        public string Name = "Node";
        public Vector2 Position = new Vector2(500, 100);
        public Vector2 MainSquareSize { get => (new Vector2(200, 50)) + Vector2.UnitY * (Items.Count * ItemPedding); }
        public Vector2 TitleSize = new Vector2(200, 30);

        public Vector2 OutPosition { get => Position + TitleSize - (Vector2.UnitY * TitleSize.Y / 2f); }
        public Vector2 InPosition { get => OutPosition - MainSquareSize * Vector2.UnitX; }

        public Color TitleColor = Color.Blue;

        public List<string> Items;
        public float ItemPedding = 30f;

        public bool IsMouseClick = true;
        public Vector2 MousePosition;
        public Vector2 NodePositionOnClick;

        public bool IsMouseOver = false;

        public BasicNode OutNode = null;

        public BasicNode(string name, Vector2 position)
        {
            Name = name;
            Items = new List<string>();
            Items.Add("Option 1");
            Items.Add("Option 2");

            Position = position;
        }

        public void Update()
        {
            CheckMouseOver();
            if (IsMouseOver)
                Move();
        }

        public void CheckMouseOver()
        {
            var rectangle = new Rectangle(Position.ToPoint(), new Point(200,30));
            if (rectangle.Contains(new Rectangle(Mouse.GetState().Position, new Point(1, 1))))
                IsMouseOver = true;
            else if(IsMouseClick && IsMouseOver)
                IsMouseOver = true;
            else
                IsMouseOver = false;
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
            if(IsMouseOver)
            {
                var selectNodePosition = Position - Vector2.One * 2f;
                var selectNodeSize = new Vector2(204, 54);
                Primativas.Square.Draw(imDraw, selectNodePosition, selectNodeSize, Color.White);
            }

            var titleTextPos = Position + Vector2.One * 8f;
            Primativas.Square.Draw(imDraw, Position, MainSquareSize, Color.Black);

            Primativas.Square.Draw(imDraw, Position, TitleSize, TitleColor);
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, Name);

            int index = 0;
            foreach(var item in Items)
            {
                var itemPosition = Position + (ItemPedding / 2f * Vector2.UnitY);
                itemPosition += TitleSize * Vector2.UnitY;
                itemPosition += Vector2.UnitY * ItemPedding * index;
                itemPosition += Vector2.UnitX * 8f;

                imDraw.AddText(itemPosition.ToNumericVector2(), Color.White.PackedValue, item);
                index++;
            }

            imDraw.AddCircleFilled(OutPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);
            imDraw.AddCircleFilled(InPosition.ToNumericVector2(), 5f, Color.Yellow.PackedValue);

            DrawLineConnect(imDraw);
        }

        public void DrawLineConnect(ImDrawListPtr imDraw)
        {
            if (OutNode == null) return;

            imDraw.AddLine(OutPosition.ToNumericVector2(), OutNode.InPosition.ToNumericVector2(), Color.Yellow.PackedValue, 2f);
        }
    }
}
