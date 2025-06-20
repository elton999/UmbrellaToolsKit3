﻿#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui;
#endif
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using MonoGame.ImGui.Extensions;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class SceneEditorWindow : IWindowEditable
    {
        private GameManagement _gameManagement;
        private System.IntPtr _bufferSceneID;

        public GameManagement GameManagement => _gameManagement;
        public GameObject GameObjectSelected;
        public List<string> Logs = new List<string>();
#if !RELEASE
        public ImGuiRenderer ImGuiRenderer => _gameManagement.Editor.ImGuiRenderer;
#endif

        public SceneEditorWindow(GameManagement gameManagement)
        {
            BarEdtior.OnSwitchEditorWindow += RemoveAsMainWindow;
            BarEdtior.OnOpenMainEditor += SetAsMainWindow;
            _gameManagement = gameManagement;
            Log.OnLog += Logs.Add;
            Log.OnLog += System.Console.WriteLine;
        }

        public void SetAsMainWindow()
        {
#if !RELEASE
            EditorArea.OnDrawWindow += ShowWindow;
#endif
        }

        public void RemoveAsMainWindow()
        {
#if !RELEASE
            EditorArea.OnDrawWindow -= ShowWindow;
#endif
        }

        public void ShowWindow(GameTime gameTime)
        {
#if !RELEASE
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

            ShowGameObjectProprieties(right);
            ShowSceneLayers(left);
            ShowEditorView(iddock);
            ShowConsole(bottom);
#endif
        }
#if !RELEASE
        private System.Numerics.Vector2 _windowPosition;
        private System.Numerics.Vector2 _windowSize;

        private void ShowEditorView(uint iddock)
        {
            ImGui.SetNextWindowDockID(iddock, ImGuiCond.Once);
            ImGui.Begin("Game (Edit Mode) (Playing)");
            ImGui.SetWindowFontScale(1.2f);

            RenderEditorView();

            _windowPosition = ImGui.GetWindowPos();
            _windowSize = ImGui.GetWindowSize();
            _windowPosition += new System.Numerics.Vector2(0, 40f);

            ImGui.End();
        }

        public void RenderEditorView()
        {
            var sceneRendered = _gameManagement.SceneManagement.MainScene.Sizes;
            var backBufferSize = new Vector2(sceneRendered.X, sceneRendered.Y);

            float xScale = _windowSize.X / backBufferSize.X;
            float yScale = _windowSize.Y / backBufferSize.Y;
            float backBuffer_scale = _windowSize.X > _windowSize.Y ? xScale : yScale;

            float backBuffer_Position_x = _windowPosition.X;
            float backBuffer_Position_y = _windowPosition.Y;

            if (_bufferSceneID.ToInt32() == 0)
                _bufferSceneID = ImGuiRenderer.BindTexture(_gameManagement.SceneManagement.MainScene.SceneRendered);
            ImGui.Image(_bufferSceneID, _gameManagement.SceneManagement.MainScene.SceneRendered.Bounds.Size.ToVector2().ToNumericVector2() * backBuffer_scale);
        }

        private void ShowConsole(uint bottom)
        {
            ImGui.SetNextWindowDockID(bottom, ImGuiCond.Once);
            ImGui.Begin("Console");
            ImGui.SetWindowFontScale(1.2f);

            if (ImGui.Button("Clear"))
                Logs.Clear();

            ImGui.BeginChild("Console Text");
            foreach (string line in Logs)
                ImGui.TextUnformatted(line);
            ImGui.EndChild();

            ImGui.End();
        }

        private void ShowSceneLayers(uint left)
        {
            ImGui.SetNextWindowDockID(left, ImGuiCond.Once);
            ImGui.Begin("Layers");
            ImGui.SetWindowFontScale(1.2f);
            if (ImGui.TreeNode("Foreground"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Foreground);
            if (ImGui.TreeNode("Player"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Players);
            if (ImGui.TreeNode("Enemies"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Enemies);
            if (ImGui.TreeNode("Middleground"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Middleground);
            if (ImGui.TreeNode("Background"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.Backgrounds);
            if (ImGui.TreeNode("UI"))
                ShowGameObjectFromLayer(_gameManagement.SceneManagement.MainScene.UI);
            ImGui.End();
        }

        private void ShowGameObjectFromLayer(List<GameObject> layer)
        {
            if (layer.Count == 0)
                ImGui.Text("---- Is empty ----");
            foreach (var gameObject in layer)
                if (ImGui.Selectable(gameObject.tag))
                    GameObjectSelected = gameObject;
            ImGui.Unindent();
        }

        private void ShowGameObjectProprieties(uint right)
        {
            ImGui.SetNextWindowDockID(right, ImGuiCond.Once);
            ImGui.Begin("Game Object props");
            if (GameObjectSelected != null)
            {
                ImGui.SetWindowFontScale(1.5f);
                ImGui.Text(GameObjectSelected.tag);
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
                ImGui.SetWindowFontScale(1.2f);

                InspectorClass.DrawAllFields(GameObjectSelected);

                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
                DrawComponents(GameObjectSelected.Components);
            }
            ImGui.End();
        }

        private void DrawComponents(UmbrellaToolsKit.Interfaces.IComponent component)
        {
            if (component == null) return;

            if (ImGui.CollapsingHeader(component.GetType().Name))
            {
                ImGui.Indent();
                InspectorClass.DrawAllFields(component);
                ImGui.Unindent();
            }

            DrawComponents(component.Next);
        }
#endif
    }
}
