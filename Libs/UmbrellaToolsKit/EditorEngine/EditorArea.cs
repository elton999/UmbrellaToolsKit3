using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard;

namespace UmbrellaToolsKit.EditorEngine
{
    public class EditorArea
    {
        public static event Action<GameTime> OnDrawWindow;
        public System.Numerics.Vector2 Position = new System.Numerics.Vector2(0, 20);

        public void Draw(GameTime gameTime)
        {
            ImGui.Begin("main window", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
                ImGui.SetWindowPos(Position);
                ImGui.SetWindowSize(ImGui.GetMainViewport().Size - Position);
                OnDrawWindow?.Invoke(gameTime);
            ImGui.End();
        }
    }
}
