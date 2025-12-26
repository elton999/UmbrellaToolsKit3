using System;
using System.Collections.Generic;
using System.Reflection;
#if !RELEASE
using ImGuiNET;
#endif
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using UmbrellaToolsKit.Utils;

namespace UmbrellaToolsKit.EditorEngine
{
    public class BarEditor
    {
        private string projectName = Assembly.GetCallingAssembly().GetName().Name;
        private bool isShowingImguiDemo = false;
        private string[] nodesTypes;

        public static event Action OnOpenMainEditor;
        public static event Action OnOpenDialogueEditor;
        public static event Action OnOpenGameSettingsEditor;

        public static event Action OnSwitchEditorWindow;

        public static IBarEditor AdditionalBar;

        public BarEditor()
        {
#if !RELEASE
            IEnumerable<Type> types = AttributesHelper.GetTypesWithAttribute(typeof(NodeImplementationAttribute));
            var nodesTypesName = new List<string>();
            foreach (var type in types)
            {
                string nodeTypeName = (string)AttributesHelper.GetConstructorArgumentsValue(type, "name");
                nodesTypesName.AddIfNew(nodeTypeName);
            }
            nodesTypes = nodesTypesName.ToArray();
#endif
        }

        public void Draw(GameTime gameTime)
        {
#if !RELEASE
            double frameRate = 1d / gameTime.ElapsedGameTime.TotalSeconds;

            if (ImGui.BeginMainMenuBar())
            {
                if (AdditionalBar is IBarEditor)
                    AdditionalBar.Draw();

                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Main Editor"))
                    {
                        OnSwitchEditorWindow?.Invoke();
                        OnOpenMainEditor?.Invoke();
                    }

                    foreach (string nodeTypeName in nodesTypes)
                    {
                        if (ImGui.MenuItem($"{nodeTypeName} Editor"))
                        {
                            OnSwitchEditorWindow?.Invoke();
                            OnOpenDialogueEditor?.Invoke();
                        }
                    }

                    if (ImGui.MenuItem("GameSettings Editor"))
                    {
                        OnSwitchEditorWindow?.Invoke();
                        OnOpenGameSettingsEditor?.Invoke();
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Help"))
                {
                    if (ImGui.MenuItem("Imgui helper"))
                        isShowingImguiDemo = true;

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

            if (isShowingImguiDemo)
                ImGui.ShowDemoWindow();
#endif
        }
    }
}
