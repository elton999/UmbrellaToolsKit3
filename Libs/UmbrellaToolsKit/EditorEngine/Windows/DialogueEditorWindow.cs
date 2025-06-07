using System;
using System.Collections.Generic;
#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Nodes;
using UmbrellaToolsKit.EditorEngine.Nodes.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.EditorEngine.Fields;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class DialogueEditorWindow : IWindowEditable
    {
        private Storage.Load _storage;

        private VariableSettings _variableSettings;

        private GameManagement _gameManagement;
        public GameManagement GameManagement => _gameManagement;

        public Storage.Load Storage => _storage;

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
            BasicNode.OnDestroyNode += RemoveSelectedNode;
            DialogueEditorMainBar.OnAnyOpenFile += OpenFile;
            _storage = new Storage.Load();
        }

        public static void StartConnectingNodes(INodeOutPutle node) => OnStartConnecting?.Invoke(node);

        public void OpenFile(string filename)
        {
            _storage = new Storage.Load(filename);
            LoadNodes();
        }

        public void SetAsMainWindow()
        {
            BarEdtior.AdicionalBar = new DialogueEditorMainBar(this);
            EditorArea.OnDrawWindow += ShowWindow;
        }
        public void RemoveAsMainWindow()
        {
            BarEdtior.AdicionalBar = null;
            EditorArea.OnDrawWindow -= ShowWindow;
        }

        public void ShowWindow(GameTime gameTime)
        {
#if !RELEASE
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
            if(ImGui.Begin("Fields"))
            {
                ImGui.SetWindowFontScale(1.2f);
                ShowFieldsSettings();
            }
            ImGui.End();

            ImGui.SetNextWindowDockID(leftID, ImGuiCond.Once);
            if(ImGui.Begin("Item props"))
            {
                ImGui.SetWindowFontScale(1.2f);
                if (SelectedNode != null) ShowNodeInfo();
            }
            ImGui.End();

            ImGui.SetNextWindowDockID(rightID, ImGuiCond.Once);
            ImGui.Begin("Dialogue Editor");
            ImGui.SetWindowFontScale(1.2f);

            var drawList = ImGui.GetWindowDrawList();

            var windowPosition = ImGui.GetWindowPos();
            var windowSize = ImGui.GetWindowSize();

            DrawBackground(drawList, windowPosition, windowSize);

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
            Primitives.Square.Draw(drawList, displayPos, displaySize, new Color(0, 0, 0, 0.5f));
            drawList.AddText(displayPosText.ToNumericVector2(), Color.White.PackedValue, $"x: {direction.X}, y: {direction.Y}");

            Primitives.Square.Draw(drawList, displayPos + displaySize * Vector2.UnitX, displaySize, new Color(0, 0, 0, 0.5f));
            drawList.AddText((displayPosText + displaySize * Vector2.UnitX).ToNumericVector2(), Color.White.PackedValue, $"m x: {MouseHandler.Position.X}, y: {MouseHandler.Position.Y}");

            ImGui.End();
#endif
        }
#if !RELEASE
        private static void DrawBackground(ImDrawListPtr drawList, System.Numerics.Vector2 windowPosition, System.Numerics.Vector2 windowSize)
        {
            Primitives.Square.Draw(
                drawList,
                windowPosition.ToXnaVector2(),
                windowSize.ToXnaVector2(),
                Color.DarkGray
            );

            int lines = 10;
            int columns = 6;
            for (int lineCount = 0; lineCount < lines; lineCount++)
            {
                for (int columnCount = 0; columnCount < columns; columnCount++)
                {
                    if ((lineCount + columnCount) % 2 == 0)
                    {
                        Vector2 size = Vector2.One * 200;
                        Vector2 position = size * new Vector2(lineCount, columnCount);

                        Primitives.Square.Draw(drawList, position, size, Color.DimGray);
                    }
                }
            }
        }
#endif
        public void Save(string filename)
        {
            _storage ??= new Storage.Load(filename);
            _storage.SetFilename(filename);

            var variablesName = new List<string>();
            var variablesType = new List<float>();

            foreach (var variable in DialogueData.Fields.Variables)
            {
                variablesName.Add(variable.Value.Name);
                variablesType.Add((int)variable.Value.Type);
            }
            _storage.AddItemString($"Fields-Name", variablesName);
            _storage.AddItemFloat($"Fields-Type", variablesType);

            List<float> ids = new List<float>();
            foreach (var node in DialogueData.Nodes)
                ids.Add(node.Id);
            _storage.AddItemFloat("Ids", ids);

            OnSave?.Invoke();
        }

        public void ShowNodeInfo()
        {
#if !RELEASE
            bool treeNode = InspectorClass.DrawSeparator("Node Inspector");
            if (treeNode) SelectedNode.DrawInspector();
#endif
        }

        public void ShowFieldsSettings()
        {
#if !RELEASE
            _variableSettings ??= new VariableSettings("", VariableType.NONE);
            object variableValue = _variableSettings.Type;
            Field.DrawEnum("Variable Type", typeof(VariableType), ref variableValue);
            Field.DrawString("Variable Name", ref _variableSettings.Name);
            _variableSettings.Type = (VariableType)variableValue;

            if (Buttons.BlueButton("Add"))
            {
                if (DialogueData.Fields.AddVariable(_variableSettings.Name, (VariableType)variableValue))
                {
                    _variableSettings.Type = VariableType.NONE;
                    _variableSettings.Name = "";
                }
            }

            if (InspectorClass.DrawSeparator("Fields"))
            {
                foreach (var variable in DialogueData.Fields.Variables)
                {
                    object value = DialogueData.Fields.GetVariableType(variable.Key);

                    Field.DrawEnum(DialogueData.Fields.GetName(variable.Key), typeof(VariableType), ref value);
                    DialogueData.Fields.Variables[variable.Key].Name = variable.Value.Name;
                    DialogueData.Fields.Variables[variable.Key].Type = (VariableType)value;
                }

                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
            }
#endif
        }

        private void SelectNode(BasicNode node) => SelectedNode = node;
        private void RemoveSelectedNode(BasicNode node) => SelectedNode = SelectedNode == node ? null : node;
#if !RELEASE
        private void TraceLineConnection(ImDrawListPtr drawList)
        {
            Primitives.Line.Draw(
                drawList,
                NodeStartConnection.OutPosition,
                MouseHandler.Position
            );

            IsConnecting = MouseHandler.ButtonLeftPressed;

            if (!IsConnecting)
                NodeStartConnection.CancelConnection();

            foreach (var node in DialogueData.Nodes)
            {
                if (node is not INodeInPutle) continue;

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
#endif
        private void StartLineConnection(INodeOutPutle node)
        {
            NodeStartConnection = node;
            IsConnecting = true;
        }

        private void ClearNodes()
        {
            DialogueData.ClearNodes();
            SelectedNode = null;
        }

        private void LoadNodes()
        {
            ClearNodes();
            DialogueData.ClearNodes();

            var fieldsCount = _storage.getItemsString("Fields-Name").Count;
            for (int i = 0; i < fieldsCount; i++)
            {
                string name = _storage.getItemsString("Fields-Name")[i];
                VariableType type = (VariableType)_storage.getItemsFloat("Fields-Type")[i];
                DialogueData.Fields.AddVariable(name, type);
                Log.Write(name.ToString());
            }

            var nodeIds = _storage.getItemsFloat("Ids");
            foreach (float nodeId in nodeIds)
            {
                int id = (int)nodeId;
                if (DialogueData.LastNodeId < id)
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
