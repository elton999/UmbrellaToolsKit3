using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard;

namespace UmbrellaToolsKit.EditorEngine
{
    public class BarEdtior
    {
        private string projectName = Assembly.GetCallingAssembly().GetName().Name;
        
        public static event Action OnOpenMainEditor;
        public static event Action OnOpenDialogueEditor;

        public static event Action OnSwichEditorWindow;

        public void Draw(GameTime gameTime)
        {
            double frameRate = 1d / gameTime.ElapsedGameTime.TotalSeconds;

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Main Editor"))
                    {
                        OnSwichEditorWindow?.Invoke();
                        OnOpenMainEditor?.Invoke();
                    }

                    if (ImGui.MenuItem("Dialogue Editor"))
                    {
                        OnSwichEditorWindow?.Invoke();
                        OnOpenDialogueEditor?.Invoke();
                    }

                    ImGui.EndMenu();
                }
                ImGui.BeginTable("##positionBar", 4);
                ImGui.TableNextColumn();
                ImGui.TableNextColumn();
                ImGui.TableNextColumn();
                ImGui.Text(projectName);
                ImGui.TableNextColumn();
                ImGui.Text($"FPS: {(int)frameRate}");
                ImGui.EndTable();
            }
            ImGui.SetWindowFontScale(1.2f);
            ImGui.EndMainMenuBar();
        }
    }
}
