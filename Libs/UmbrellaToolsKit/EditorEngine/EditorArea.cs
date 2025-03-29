using System;
#if !RELEASE
using ImGuiNET;
#endif
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine
{
    public class EditorArea
    {
        public static event Action<GameTime> OnDrawWindow;
#if !RELEASE
        public System.Numerics.Vector2 Position = new System.Numerics.Vector2(0, 20);
#endif

        public void Draw(GameTime gameTime)
        {
#if !RELEASE
            ImGui.Begin("main window", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
            ImGui.SetWindowPos(Position);
            ImGui.SetWindowSize(ImGui.GetMainViewport().Size - Position);
            OnDrawWindow?.Invoke(gameTime);
            ImGui.End();
#endif
        }
    }
}
