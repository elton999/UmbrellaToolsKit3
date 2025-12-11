using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using UmbrellaToolsKit.Utils;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class GameSettingsWindow : IWindowEditable
    {
        private GameManagement _gameManagement;
        private IEnumerable<Type> _allSettingsData;

        private object _currentObject;
        private string _currentPathFile;
        private bool _canShowPropertyEditor = false;

        private string _projectPath => _buildPath + "/../../../../Project";
        private string _buildPath => Environment.CurrentDirectory;
        private Type _currentType;

        private EditorMain _editorMain;

        public GameManagement GameManagement => _gameManagement;
        public IEnumerable<Type> AllSettingsData
        {
            get
            {
                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                Assembly projectAssembly = Assembly.GetEntryAssembly();
                Type gameSettingsType = typeof(GameSettingsPropertyAttribute);

                List<Type> types = AttributesHelper.GetTypesWithAttribute(currentAssembly, gameSettingsType).ToList();
                types.AddRange(AttributesHelper.GetTypesWithAttribute(projectAssembly, gameSettingsType));

                return types;
            }
        }

        public GameSettingsWindow(GameManagement gameManagement, EditorMain editorMain)
        {
            _gameManagement = gameManagement;
            _allSettingsData = AllSettingsData;

            BarEdtior.OnSwitchEditorWindow += RemoveAsMainWindow;
            BarEdtior.OnOpenGameSettingsEditor += SetAsMainWindow;
            _editorMain = editorMain;
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
            uint leftID = ImGui.GetID("MainLeft");
            uint rightID = ImGui.GetID("MainRight");

            var dockSize = new System.Numerics.Vector2(0, 0);

            ImGui.BeginChild("left", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.15f, 0));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(leftID, dockSize);
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("right", new System.Numerics.Vector2(ImGui.GetMainViewport().Size.X * 0.85f, 0));
            ImGui.SetWindowFontScale(1.2f);
            ImGui.DockSpace(rightID, dockSize);
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.SetNextWindowDockID(rightID, ImGuiCond.Once);
            ImGui.Begin("Game Settings Editor");
            ImGui.SetWindowFontScale(1.2f);
            ShowSettingProperty();
            ImGui.End();

            ImGui.SetNextWindowDockID(leftID, ImGuiCond.Once);
            ImGui.Begin("All Game Settings Data");
            ImGui.SetWindowFontScale(1.2f);
            ShowSettingsList();
            ImGui.End();
#endif
        }

#if !RELEASE
        private void ShowSettingsList()
        {
            foreach (var type in _allSettingsData)
            {
                if (ImGui.Selectable(AttributesHelper.FormatName(type.Name), type == _currentType))
                {
                    _currentType = type;
                    _currentObject = GetInstanceByType(type);
                }
            }
        }

        private void ShowSettingProperty()
        {
            if (!_canShowPropertyEditor) return;
            if (ImGui.Button("Save"))
            {
                SaveFile(_projectPath + _currentPathFile, _currentObject);
                SaveFile(_buildPath + _currentPathFile, _currentObject);
            }
            ImGui.BeginChild("Game Settings Fields");
            ((GameSettingsProperty)_currentObject).DrawFields(_editorMain);
            ImGui.EndChild();
        }

        private object GetInstanceByType(Type type)
        {
            _canShowPropertyEditor = false;
            bool hasPropertyAttribute = type.HasPropertyAttribute(typeof(GameSettingsPropertyAttribute));

            if (hasPropertyAttribute)
            {
                var propertyAttribute = type.GetCustomAttributesData();
                var arguments = propertyAttribute[0].ConstructorArguments;
                string nameFile = (string)arguments[0].Value;
                string pathFile = (string)arguments[1].Value;

                pathFile += nameFile;
                _currentPathFile = pathFile;
                _canShowPropertyEditor = true;

                if (!File.Exists(_buildPath + pathFile + GameSettingsProperty.SaveIntegration.Extension))
                {
                    var instance = Activator.CreateInstance(type);
                    _currentObject = instance;
                    SaveFile(_projectPath + pathFile, instance);
                    SaveFile(_buildPath + pathFile, instance);
                    return instance;
                }
                return GameSettingsProperty.GetProperty(_buildPath + pathFile, type);
            }

            return default;
        }

        private static void SaveFile(string pathFile, object instance)
        {
            var timer = new Utils.Timer();
            timer.Begin();
            GameSettingsProperty.SaveIntegration.Set(instance);
            GameSettingsProperty.SaveIntegration.Save(pathFile);
            timer.End();

            Log.Write($"[{instance.GetType().Name}] saving: {pathFile}, {timer.GetTotalSeconds()}");
        }
#endif
    }
}
