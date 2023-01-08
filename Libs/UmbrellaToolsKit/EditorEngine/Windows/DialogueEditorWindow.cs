using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class DialogueEditorWindow : IWindowEditable
    {
        private GameManagement _gameManagement;
        public GameManagement GameManagement { get => _gameManagement; }

        public List<Nodes.BasicNode> Nodes;

        public DialogueEditorWindow(GameManagement gameManagement)
        {
            _gameManagement = gameManagement;

            Nodes = new List<Nodes.BasicNode>();
            Nodes.Add(new Nodes.BasicNode("basic node", new Vector2(500, 200)));
            Nodes.Add(new Nodes.BasicNode("basic node2", new Vector2(700, 200)));

            Nodes[0].OutNode = Nodes[1];

            BarEdtior.OnSwichEditorWindow += RemoveAsMainWindow;
            BarEdtior.OnOpenDialogueEditor += SetAsMainWindow;
        }

        public void SetAsMainWindow() => EditorArea.OnDrawWindow += ShowWindow;

        public void RemoveAsMainWindow() => EditorArea.OnDrawWindow -= ShowWindow;

        public void ShowWindow(GameTime gameTime)
        {
            uint leftID = ImGui.GetID("MainLeft");
            uint rightID = ImGui.GetID("MainRight");

            var dockSize = new System.Numerics.Vector2(0, 0);

            ImGui.BeginChild("left", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.2f, 0));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(leftID, dockSize);
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("right", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.8f, 0));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(rightID, dockSize);
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.SetNextWindowDockID(leftID, ImGuiCond.Once);
            ImGui.Begin("Item props");
            ImGui.SetWindowFontScale(1.2f);
            ImGui.End();

            ImGui.SetNextWindowDockID(rightID, ImGuiCond.Once);
            ImGui.Begin("Dialogue Editor");
            ImGui.SetWindowFontScale(1.2f);

            var drawList = ImGui.GetWindowDrawList();

            var windowPosition = ImGui.GetWindowPos();
            var windowSize = ImGui.GetWindowSize();

            Primativas.Square.Draw(
                drawList, 
                windowPosition.ToXnaVector2(), 
                windowSize.ToXnaVector2(), 
                Color.DarkSlateGray
            );

            foreach(var node in Nodes)
            {
                node.Update();
                node.Draw(drawList);
            }

            ImGui.End();
        }
    }
}
