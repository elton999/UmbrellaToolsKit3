using ImGuiNET;
using System;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;

namespace UmbrellaToolsKit.EditorEngine.Nodes
{
    public class BasicNode
    {
        public static void Draw(ImDrawListPtr imDraw, string name, Vector2 position)
        {
            var mainSquareSize = new Vector2(200, 100);
            var titleSize = new Vector2(200, 30);
            var titleTextPos = position + Vector2.One * 8f;
            Primativas.Square.Draw(imDraw, position, mainSquareSize, Color.Black);
            Primativas.Square.Draw(imDraw, position, titleSize, Color.Blue);
            imDraw.AddText(titleTextPos.ToNumericVector2(), Color.White.PackedValue, name);
        }
    }
}
