using System;
using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.EditorEngine.Nodes;
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using UmbrellaToolsKit.Input;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class DialogueEditorWindow : IWindowEditable
    {
        private const string _dialogueSettingPath = @"Content/Dialogue1.Umbrella";
        private const string _dialogueJsonPath = @"Content/Dialogue1.dn";
        private Storage.Load _storage;

        private GameManagement _gameManagement;
        public GameManagement GameManagement { get => _gameManagement; }

        public BasicNode SelectedNode;

        public INodeOutPutle NodeStartConnection;
        public bool IsConnecting = false;

        public Vector2 ClickPosition = Vector2.Zero;
        public Vector2 LastDirection = Vector2.One;

        public static event Action<INodeOutPutle> OnStartConnecting;
        public static event Action OnSave;

        public DialogueEditorWindow(GameManagement gameManagement)
        {
            _gameManagement = gameManagement;

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

            if (ImGui.Button("Save"))
            {
                List<float> ids = new List<float>();
                foreach (var node in DialogueData.Nodes)
                    ids.Add(node.Id);
                _storage.AddItemFloat("Ids", ids);

                OnSave?.Invoke();
            }
                
            if (ImGui.Button("Add Start Node")) AddNodeStart();
            if (ImGui.Button("Add End Node")) AddNodeEnd();
            if (ImGui.Button("Add Node")) AddNode();
            if (SelectedNode != null) ShowNodeInfo();
            if (ImGui.Button("Export")) DialogueJsonExport.Export(_dialogueJsonPath);

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

            int lines = 10;
            int columns = 10;
            for (int i = 0; i < lines; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    if((i+j) % 2 == 0)
                    {
                        Vector2 size = Vector2.One * 200;
                        Vector2 position = size * (new Vector2(i, j));

                        Primativas.Square.Draw(drawList, position, size, Color.DimGray);
                    }
                }
            }

            var direction = Vector2.Zero;

            if (MouseHandler.ButtonMiddleOneClick)
                ClickPosition = MouseHandler.Position;

            if (MouseHandler.ButtonMiddlePressing)
                direction = ClickPosition - MouseHandler.Position;

            var cachedNodes = new List<BasicNode>(DialogueData.Nodes);
            foreach (var node in cachedNodes)
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

            var displayPos = windowPosition.ToXnaVector2() + Vector2.One * 30;
            var displaySize = new Vector2(150, 30);
            var displayPosText = displayPos + Vector2.One * 8;
            Primativas.Square.Draw(drawList, displayPos, displaySize, new Color(0,0,0,0.5f));
            drawList.AddText(displayPosText.ToNumericVector2(), Color.White.PackedValue, $"x: {direction.X}, y: {direction.Y}");

            Primativas.Square.Draw(drawList, displayPos + displaySize * Vector2.UnitX, displaySize, new Color(0, 0, 0, 0.5f));
            drawList.AddText((displayPosText + displaySize * Vector2.UnitX).ToNumericVector2(), Color.White.PackedValue, $"m x: {MouseHandler.Position.X}, y: {MouseHandler.Position.Y}");

            ImGui.End();
        }

        public void ShowNodeInfo()
        {
            bool treeNode = InspectorClass.DrawSeparator("Node Inspector");
            if (treeNode) SelectedNode.DrawInspector();
            if (treeNode) ImGui.Unindent();
        }

        private void SelectNode(BasicNode node) => SelectedNode = node;

        private void AddNode()
        {
            var node = new NodeWithOptions(_storage, DialogueData.GetNewNodeId(), "new node", Vector2.One * 500f);
            DialogueData.AddNode(node);
            _storage.Save();
        }

        private void AddNodeStart()
        {
            var node = new StartNode(_storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f);
            DialogueData.AddNode(node);
            _storage.Save();
        }

        private void AddNodeEnd()
        {
            var node = new EndNode(_storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f);
            DialogueData.AddNode(node);
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

            foreach (var node in DialogueData.Nodes)
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
            var nodeIds = _storage.getItemsFloat("Ids");
            foreach (float nodeId in nodeIds)
            {
                int id = (int)nodeId;
                if(DialogueData.LastNodeId < id)
                    DialogueData.LastNodeId = id;
            }
            DialogueData.LastNodeId++;

            foreach (float id in nodeIds)
            {
                string nodeType = _storage.getItemsString($"Nodes-Object-{(int)id}")[0];

                Type type = Type.GetType(nodeType);
                object[] args = new object[] { _storage, (int)id, "name", Vector2.Zero };
                var node = (BasicNode)Activator.CreateInstance(type, args);
                DialogueData.AddNode(node);
            }

            foreach (var node in DialogueData.Nodes)
                node.Load();

            foreach (var node in DialogueData.Nodes)
                node.Load();
        }
    }
}
