using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ImGuiNET;
using UmbrellaToolsKit.Interfaces;

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

        public void SetAsMainWindow()
        {
            EditorMain.OnDrawOverLayer += RenderEditorView;
            EditorArea.OnDrawWindow += Draw;
        }

        public void RemoveAsMainWindow() 
        {
            EditorMain.OnDrawOverLayer -= RenderEditorView;
            EditorArea.OnDrawWindow -= Draw;
        }

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

        private System.Numerics.Vector2 _windowPosition;
        private System.Numerics.Vector2 _windowSize;

        private void ShowEditorView(uint iddock)
        {
            ImGui.SetNextWindowDockID(iddock, ImGuiCond.Once);
            ImGui.Begin("Game (Edit Mode) (Playing)");
                ImGui.SetWindowFontScale(1.2f);

                _windowPosition = ImGui.GetWindowPos();
                _windowSize = ImGui.GetWindowSize();
                _windowPosition += new System.Numerics.Vector2(0, 40f);

            ImGui.End();
        }

        public void RenderEditorView()
        {
            var sceneRendered = _gameManagement.SceneManagement.MainScene.SceneRendered;
            var backBufferSize = new Vector2(sceneRendered.Width, sceneRendered.Height);

            float xScale = _windowSize.X / backBufferSize.X;
            float yScale = _windowSize.Y / backBufferSize.Y;
            float backBuffer_scale = _windowSize.X > _windowSize.Y ? xScale : yScale;

            float backBuffer_Position_x = _windowPosition.X;
            float backBuffer_Position_y = _windowPosition.Y;

            _gameManagement.SceneManagement.MainScene.DrawBuffer(
                _gameManagement.SpriteBatch,
                backBuffer_scale,
                backBuffer_Position_x,
                backBuffer_Position_y
            );
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
            if (ImGui.TreeNode("Forenground")) 
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Foreground);
            if (ImGui.TreeNode("Player"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Players);
            if (ImGui.TreeNode("Enimies"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Enemies);
            if (ImGui.TreeNode("Middleground"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Middleground);
            if (ImGui.TreeNode("Background"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Backgrounds);
            ImGui.End();
        }

        private void ShowGameObjectFromLayer(List<IGameObject> layer)
        {
            if(layer.Count == 0)
                ImGui.Text("---- Is empty ----");
            foreach (var gameObject in layer)
                ImGui.Selectable(gameObject.Tag);
            ImGui.TreePop();
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
