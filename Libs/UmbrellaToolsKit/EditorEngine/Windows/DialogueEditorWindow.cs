using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.EditorEngine.Nodes;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using UmbrellaToolsKit.Input;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class DialogueEditorWindow : IWindowEditable
    {
        private const string _dialogueSettingPath = @"Content/Dialogue1.Umbrella";
        private Storage.Load _storage;
        private int _idsCount = 0;

        private GameManagement _gameManagement;
        public GameManagement GameManagement { get => _gameManagement; }

        public BasicNode SelectedNode;

        public List<NodeInPutAndOutPut> Nodes;
        public Nodes.Interfaces.INodeOutPutle NodeStartConnection;
        public bool IsConnecting = false;

        public static event Action<Nodes.Interfaces.INodeOutPutle> OnStartConnecting;

        public DialogueEditorWindow(GameManagement gameManagement)
        {
            _gameManagement = gameManagement;

            Nodes = new List<NodeInPutAndOutPut>();

            BarEdtior.OnSwichEditorWindow += RemoveAsMainWindow;
            BarEdtior.OnOpenDialogueEditor += SetAsMainWindow;

            OnStartConnecting += StartLineConnection;
            BasicNode.OnSelectNode += SelectNode;

            _storage= new Storage.Load(_dialogueSettingPath);
        }

        public static void StartConnnetingNodes(Nodes.Interfaces.INodeOutPutle node) => OnStartConnecting?.Invoke(node);

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
            
            if(ImGui.Button("Save")) _storage.Save();
            if (ImGui.Button("Add Node")) AddNode();
            if(SelectedNode != null) ShowNodeInfo();
            
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
                Color.DarkGray
            );

            foreach(var node in Nodes)
            {
                if (!IsConnecting)
                    node.Update();

                if (IsConnecting)
                    TraceLineConnection(drawList);

                node.Draw(drawList);
            }

            ImGui.End();
        }

        public void ShowNodeInfo()
        {
            string nameNodeValue = SelectedNode.Name;
            Fields.Field.DrawString("Node Name", ref nameNodeValue);
            Fields.Field.DrawLongText("Node text", ref nameNodeValue);
            SelectedNode.Name = nameNodeValue;
        }

        private void SelectNode(BasicNode node) => SelectedNode = node;

        private void AddNode()
        {
            var node = new NodeInPutAndOutPut(_storage, _idsCount, "new node", Vector2.One * 500f);
            _idsCount++;
            Nodes.Add(node);
        }

        private void TraceLineConnection(ImDrawListPtr drawList)
        {
            Primativas.Line.Draw(
                drawList,
                NodeStartConnection.OutPosition,
                MouseHandler.Position
            );

            IsConnecting = MouseHandler.ButtonLeftPressed;

            if(!IsConnecting)
                NodeStartConnection.CancelConnection();
            
            foreach(var node in Nodes)
            {
                float distance = (node.InPosition - MouseHandler.Position).Length();
                if (distance <= 5f)
                {
                    NodeStartConnection.AddNodeConnection(node);
                    IsConnecting = false;
                    return;
                }
            }
        }

        private void StartLineConnection(Nodes.Interfaces.INodeOutPutle node)
        {
            NodeStartConnection = node;
            IsConnecting = true;
        }
    }
}
