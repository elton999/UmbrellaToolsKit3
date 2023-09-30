using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.EditorEngine.Nodes;
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
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

        public List<BasicNode> Nodes;
        public INodeOutPutle NodeStartConnection;
        public bool IsConnecting = false;

        public Vector2 ClickPosition = Vector2.Zero;
        public Vector2 LastDirection = Vector2.One;

        public static event Action<INodeOutPutle> OnStartConnecting;

        public DialogueEditorWindow(GameManagement gameManagement)
        {
            _gameManagement = gameManagement;

            Nodes = new List<BasicNode>();

            BarEdtior.OnSwitchEditorWindow += RemoveAsMainWindow;
            BarEdtior.OnOpenDialogueEditor += SetAsMainWindow;

            OnStartConnecting += StartLineConnection;
            BasicNode.OnSelectNode += SelectNode;

            _storage = new Storage.Load(_dialogueSettingPath);
            LoadNodes();
        }

        public static void StartConnectingNodes(INodeOutPutle node) => OnStartConnecting?.Invoke(node);

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

            if (ImGui.Button("Save")) _storage.Save();
            if (ImGui.Button("Add Start Node")) AddNodeStart();
            if (ImGui.Button("Add End Node")) AddNodeEnd();
            if (ImGui.Button("Add Node")) AddNode();
            if (SelectedNode != null) ShowNodeInfo();

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

            var direction = Vector2.Zero;

            if (MouseHandler.ButtonMiddleOneClick)
                ClickPosition = MouseHandler.Position;

            if (MouseHandler.ButtonMiddlePressing)
                direction = ClickPosition - MouseHandler.Position;

            foreach (var node in Nodes)
            {
                if (direction.Length() > 0)
                    node.Position = node.Position - direction;

                if (direction.Length() == 0 && LastDirection.Length() > 0)
                    node.Position = node.Position - LastDirection;

                if (!IsConnecting)
                    node.Update();

                if (IsConnecting)
                    TraceLineConnection(drawList);

                node.Draw(drawList);

                if (direction.Length() > 0)
                    node.Position = node.Position + direction;
            }

            LastDirection = direction;

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
            _storage.Save();
        }

        private void AddNodeStart()
        {
            var node = new StartNode(_storage, _idsCount, Vector2.One * 500f);
            _idsCount++;
            Nodes.Add(node);
            _storage.Save();
        }

        private void AddNodeEnd()
        {
            var node = new EndNode(_storage, _idsCount, Vector2.One * 500f);
            _idsCount++;
            Nodes.Add(node);
            _storage.Save();
        }

        private void TraceLineConnection(ImDrawListPtr drawList)
        {
            Primativas.Line.Draw(
                drawList,
                NodeStartConnection.OutPosition,
                MouseHandler.Position
            );

            IsConnecting = MouseHandler.ButtonLeftPressed;

            if (!IsConnecting)
                NodeStartConnection.CancelConnection();

            foreach (var node in Nodes)
            {
                if (node is INodeInPutle)
                {
                    INodeInPutle nodeInPutle = (INodeInPutle)node;
                    float distance = (nodeInPutle.InPosition - MouseHandler.Position).Length();
                    if (distance <= 5f)
                    {
                        NodeStartConnection.AddNodeConnection(nodeInPutle);
                        IsConnecting = false;
                        return;
                    }
                }
            }
        }

        private void StartLineConnection(INodeOutPutle node)
        {
            NodeStartConnection = node;
            IsConnecting = true;
        }

        private void LoadNodes()
        {
            foreach (var nodeId in _storage.getItemsFloat("Id"))
            {
                int id = (int)nodeId;
                string name = _storage.getItemsString($"name-{id}")[0];
                var position = Vector2.Zero;
                position.X = _storage.getItemsFloat($"position-{id}-vector-x")[0];
                position.Y = _storage.getItemsFloat($"position-{id}-vector-y")[0];

                var node = new NodeInPutAndOutPut(_storage, id, name, position);
                Nodes.Add(node);
            }

            foreach (var node in Nodes)
            {
                var nodesConnections = _storage.getItemsFloat($"Nodes-Connection-In-{node.Id}");
                foreach (var connection in nodesConnections)
                {
                    //node.AddNodeConnection(Nodes[(int)connection]);
                }
            }
        }
    }
}
