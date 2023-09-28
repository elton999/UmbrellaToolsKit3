using ImGuiNET;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Attributes;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class SceneEditorWindow
    {
        private GameManagement _gameManagement;
        public GameObject GameObjectSelected;
        public List<string> Logs = new List<string>();

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

            ShowGameObjectProprieties(right);
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
            var sceneRendered = _gameManagement.SceneManagement.MainScene.Sizes;
            var backBufferSize = new Vector2(sceneRendered.X, sceneRendered.Y);

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

                DrawAllFields(GameObjectSelected);

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
                DrawAllFields(component);
                ImGui.Unindent();
            }

            DrawComponents(component.Next);
        }

        private void DrawAllFields(object obj)
        {
            var type = obj.GetType();
            var fieldsCategories = new Dictionary<string, List<FieldInfo>>();
            var fieldsWithoutCategories = new List<FieldInfo>();

            foreach (FieldInfo fInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (var attr in fInfo.CustomAttributes)
                {
                    if (attr.AttributeType == typeof(CategoryAttribute))
                    {
                        string categoryName = (string)attr.ConstructorArguments[0].Value;

                        if (!fieldsCategories.ContainsKey(categoryName))
                            fieldsCategories.Add(categoryName, new List<FieldInfo>());

                        fieldsCategories[categoryName].Add(fInfo);

                    }
                    if (attr.AttributeType == typeof(ShowEditorAttribute))
                        fieldsWithoutCategories.Add(fInfo);
                }
            }

            foreach (var category in fieldsCategories)
            {
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
                bool treeNode = ImGui.TreeNodeEx(category.Key, ImGuiTreeNodeFlags.DefaultOpen);

                foreach (var field in category.Value)
                {
                    if (fieldsWithoutCategories.Contains(field))
                    {
                        fieldsWithoutCategories.Remove(field);
                        if (treeNode) DrawField(field, obj);
                    }
                }
                if (treeNode) ImGui.Unindent();
            }

            foreach (var field in fieldsWithoutCategories)
                DrawField(field, obj);
        }

        private void DrawField(FieldInfo fInfo, object prop)
        {
            switch (fInfo.FieldType.ToString())
            {
                case "Microsoft.Xna.Framework.Vector2":
                    var vector2 = (Vector2)fInfo.GetValue(prop);
                    Fields.Field.DrawVector(fInfo.Name, ref vector2);
                    fInfo.SetValue(prop, vector2);
                    break;
                case "Microsoft.Xna.Framework.Vector3":
                    var vector3 = (Vector3)fInfo.GetValue(prop);
                    Fields.Field.DrawVector(fInfo.Name, ref vector3);
                    fInfo.SetValue(prop, vector3);
                    break;
                case "System.Single":
                    var floatValue = (float)fInfo.GetValue(prop);
                    Fields.Field.DrawFloat(fInfo.Name, ref floatValue);
                    fInfo.SetValue(prop, floatValue);
                    break;
                case "System.String":
                    var stringValue = (string)fInfo.GetValue(prop);
                    Fields.Field.DrawString(fInfo.Name, ref stringValue);
                    fInfo.SetValue(prop, stringValue);
                    break;
                case "System.Boolean":
                    var boolValue = (bool)fInfo.GetValue(prop);
                    Fields.Field.DrawBoolean(fInfo.Name, ref boolValue);
                    fInfo.SetValue(prop, boolValue);
                    break;
            }
        }
    }
}
