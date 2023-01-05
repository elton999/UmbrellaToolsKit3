using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ImGuiNET;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class SceneEditorWindow
    {
        private GameManagement _gameManagement;

        public SceneEditorWindow(GameManagement gameManagement)
        {
            BarEdtior.OnSwichEditorWindow += RemoveAsMainWindow;
            BarEdtior.OnOpenMainEditor += SetAsMainWindow;
            _gameManagement = gameManagement;
        }

        public void SetAsMainWindow() => EditorArea.OnDrawWindow += Draw;

        public void RemoveAsMainWindow() => EditorArea.OnDrawWindow -= Draw;

        public void Draw(GameTime gameTime)
        {
            uint iddock = ImGui.GetID("MainDocking");
            uint left = ImGui.GetID("MainLeft");
            uint right = ImGui.GetID("MainRight");
            uint bottom = ImGui.GetID("MainBottom");

            ImGui.BeginChild("left", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.2f, 0));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(left, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("middle", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.6f, 0));
            ImGui.BeginChild("top", new System.Numerics.Vector2(0, ImGui.GetMainViewport().Size.Y * 0.7f));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(iddock, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();

            ImGui.BeginChild("bottom", new System.Numerics.Vector2(0, ImGui.GetMainViewport().Size.Y * 0.3f));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(bottom, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();
            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("right", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.2f, 0));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(right, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.End();

            ShowGameObjectProprietes(right);
            ShowSceneLayers(left);
            ShowEditorView(iddock);
            ShowConsole(bottom);
        }

        private void ShowEditorView(uint iddock)
        {
            ImGui.SetNextWindowDockID(iddock, ImGuiCond.Once);
            ImGui.Begin("Game (Edit Mode) (Paused)");
            ImGui.SetWindowFontScale(1.2f);
            ImGui.End();
        }

        private void ShowConsole(uint bottom)
        {
            ImGui.SetNextWindowDockID(bottom, ImGuiCond.Once);
            ImGui.Begin("Console");
            ImGui.SetWindowFontScale(1.2f);
            ImGui.End();
        }

        private void ShowSceneLayers(uint left)
        {
            ImGui.SetNextWindowDockID(left, ImGuiCond.Once);
            ImGui.Begin("Layers");
            ImGui.SetWindowFontScale(1.2f);
            if (ImGui.TreeNode("Forenground")) { }
            if (ImGui.TreeNode("Player")) { }
            if (ImGui.TreeNode("Enimies")) { }
            if (ImGui.TreeNode("Middleground")) { }
            if (ImGui.TreeNode("Background")) { }
            ImGui.End();
        }

        private void ShowGameObjectProprietes(uint right)
        {
            ImGui.SetNextWindowDockID(right, ImGuiCond.Once);
            ImGui.Begin("Game Object props");
            ImGui.SetWindowFontScale(1.2f);
            if (ImGui.TreeNode("Position"))
            {
                ImGui.TableNextColumn();
                System.Numerics.Vector3 vetor = new System.Numerics.Vector3(0, 0, 0);
                if (ImGui.BeginTable("##position", 3))
                {
                    ImGui.TableNextColumn();
                    ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(1, 0, 0, 0.5f));
                    ImGui.InputFloat("x", ref vetor.X);
                    ImGui.TableNextColumn();
                    ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0, 1, 0, 0.5f));
                    ImGui.InputFloat("y", ref vetor.Y);
                    ImGui.TableNextColumn();
                    ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0, 0, 1, 0.5f));
                    ImGui.InputFloat("z", ref vetor.Z);
                    ImGui.EndTable();
                }
                ImGui.Unindent();
            }
            ImGui.End();
        }
    }
}
