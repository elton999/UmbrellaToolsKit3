#if !RELEASE
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace UmbrellaToolsKit.EditorEngine.Fields
{
    public class Buttons
    {
        static Texture2D texture;
        public static bool ColorButton(string text, System.Numerics.Vector2 size, System.Numerics.Vector4 color)
        {
            bool clicked = false;
#if !RELEASE
            ImGui.PushStyleColor(ImGuiCol.Button, color);
            clicked = ImGui.Button(text, size);
            ImGui.PopStyleColor();
#endif
            return clicked;
        }

        public static bool RedButton(string text) => RedButton(text, System.Numerics.Vector2.Zero);

        public static bool RedButton(string text, System.Numerics.Vector2 size)
        {
#if !RELEASE
            var redColor = new System.Numerics.Vector4(1, 0, 0, 1);
#endif
            return ColorButton(text, size, redColor);
        }

        public static bool BlueButton(string text) => BlueButton(text, System.Numerics.Vector2.Zero);
        
        public static bool BlueButtonLarge(string text)
        {
#if !RELEASE
            return BlueButton(text, new System.Numerics.Vector2(ImGui.GetWindowSize().X, 0.0f));
#endif      
        }

        public static bool BlueButton(string text, System.Numerics.Vector2 size)
        {
             bool clicked = false;
#if !RELEASE
            clicked = ImGui.Button(text, size);
#endif
            return clicked;
        }
    }
}
