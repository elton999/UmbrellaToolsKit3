#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine.Primitives
{
    public class Square
    {
 #if !RELEASE
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
#endif
    }
}
