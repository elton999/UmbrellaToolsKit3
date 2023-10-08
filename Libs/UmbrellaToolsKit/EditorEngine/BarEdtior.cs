using System;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;

namespace UmbrellaToolsKit.EditorEngine
{
    public class BarEdtior
    {
        private string projectName = Assembly.GetCallingAssembly().GetName().Name;
        
        public static event Action OnOpenMainEditor;
        public static event Action OnOpenDialogueEditor;

        public static event Action OnSwitchEditorWindow;

        public static IBarEditor AdicionalBar;

        public void Draw(GameTime gameTime)
        {
            double frameRate = 1d / gameTime.ElapsedGameTime.TotalSeconds;

            if (ImGui.BeginMainMenuBar())
            {
                if (AdicionalBar is IBarEditor)
                    AdicionalBar.Draw();

                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Main Editor"))
                    {
                        OnSwitchEditorWindow?.Invoke();
                        OnOpenMainEditor?.Invoke();
                    }

                    if (ImGui.MenuItem("Dialogue Editor"))
                    {
                        OnSwitchEditorWindow?.Invoke();
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
