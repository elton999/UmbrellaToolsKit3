using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard;

namespace UmbrellaToolsKit.EditorEngine
{
    public class BarEdtior
    {
        public void Draw(GameTime gameTime)
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Open..", "Ctrl+O")) { /* Do stuff */ }
                    if (ImGui.MenuItem("Save", "Ctrl+S")) { /* Do stuff */ }
                    if (ImGui.MenuItem("Close", "Ctrl+W")) { /* Do stuff */  }
                    ImGui.EndMenu();
                }
                ImGui.BeginTable("##positionBar", 4);
                ImGui.TableNextColumn();
                ImGui.TableNextColumn();
                ImGui.TableNextColumn();
                ImGui.Text("project name");
                ImGui.TableNextColumn();
                ImGui.Text("FPS: 60");
                ImGui.EndTable();
            }
            ImGui.SetWindowFontScale(1.2f);
            ImGui.EndMainMenuBar();
        }
    }
}
