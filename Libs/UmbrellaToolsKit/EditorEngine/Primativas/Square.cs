using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;

namespace UmbrellaToolsKit.EditorEngine.Primativas
{
    public class Square
    {
        public static void Draw(ImDrawListPtr imDraw, Vector2 position, Vector2 size, Color color)
        {
            imDraw.AddQuadFilled(
                position.ToNumericVector2(),
                (position + size * Vector2.UnitY).ToNumericVector2(),
                (position + size * Vector2.One).ToNumericVector2(),
                (position + size * Vector2.UnitX).ToNumericVector2(),
                color.PackedValue
            );
        }
    }
}
