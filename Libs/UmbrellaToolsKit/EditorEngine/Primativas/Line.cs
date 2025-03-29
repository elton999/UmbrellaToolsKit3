using System;
using System.Collections.Generic;
using System.Text;
#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine.Primativas
{
    public class Line
    {
        public static Vector2 BezierFactor { get => new Vector2(0.5f, 0.1f); }

#if !RELEASE
        public static void Draw(ImDrawListPtr imDraw, Vector2 start, Vector2 end)
        {
            float lenght = (start - end).Length();
            var p2 = start + BezierFactor * lenght;
            var p3 = end - BezierFactor * lenght;

            imDraw.AddBezierCubic(
                start.ToNumericVector2(),
                p2.ToNumericVector2(),
                p3.ToNumericVector2(),
                end.ToNumericVector2(),
                Color.Yellow.PackedValue, 2f);
        }
#endif
    }
}
