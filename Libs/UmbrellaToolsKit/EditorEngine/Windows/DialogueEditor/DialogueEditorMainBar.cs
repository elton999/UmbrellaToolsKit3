#if !RELEASE
using ImGuiNET;
#endif
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using Microsoft.Xna.Framework;
using System;
using UmbrellaToolsKit.EditorEngine.Attributes;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Nodes;

namespace UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor
{
    public class DialogueEditorMainBar : IBarEditor
    {
        public static event Action<string> OnAnyOpenFile;

        private string _dialogueJsonPath;
        private string _dialogueSettingPath;

        private DialogueEditorWindow _dialogueEditorWindow;

        public DialogueEditorMainBar(DialogueEditorWindow dialogueEditorWindow) => _dialogueEditorWindow = dialogueEditorWindow;

        public void Draw()
        {
#if !RELEASE
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Open..."))
                {
                    var openFileDialog = OpenFileDialogue.OpenFileDialog("Open file", "Dialogue Nodes", ".Umbrella");
                    if (OpenFileDialogue.SaveFileDialog(openFileDialog))
                    {
                        _dialogueSettingPath = openFileDialog.FileName;
                        OnAnyOpenFile?.Invoke(_dialogueSettingPath);
                    }
                }

                if (ImGui.MenuItem("Save"))
                {
                    if (_dialogueSettingPath is null)
                    {
                        var saveFileDialog = ExportDialogue.SaveFileDialog("Save", "Umbrella Tools Kit Dialogue Nodes file", ".Umbrella");

                        if (ExportDialogue.ShowSaveDialog(saveFileDialog))
                        {
                            _dialogueSettingPath = saveFileDialog.FileName;
                            _dialogueEditorWindow.Save(_dialogueSettingPath);
                        }

                        return;
                    }
                    _dialogueEditorWindow.Save(_dialogueSettingPath);
                }

                if (ImGui.MenuItem("Export DN file"))
                {
                    var saveFileDialog = ExportDialogue.SaveFileDialog("Export to ...", "Umbrella Tools Kit Dialogue Nodes file", ".dn");

                    if (ExportDialogue.ShowSaveDialog(saveFileDialog))
                    {
                        _dialogueJsonPath = saveFileDialog.FileName;
                        DialogueJsonExport.Export(_dialogueJsonPath);
                    }
                }

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Nodes"))
            {
                IEnumerable<Type> types = AttributesHelper.GetTypesWithAttribute(typeof(NodeImplementationAttribute));

                foreach (var nodeClassType in types)
                {
                    var propertyAttribute = nodeClassType.GetCustomAttributesData();
                    var arguments = propertyAttribute[0].ConstructorArguments;
                    string nodeTypeName = (string)arguments[0].Value;

                    if (nodeTypeName != "DialogueNodes") continue;

                    if (ImGui.MenuItem($"Add {AttributesHelper.FormatName(nodeClassType.Name)}"))
                    {
                        var node = (BasicNode)Activator.CreateInstance(nodeClassType, new object[] { _dialogueEditorWindow.Storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f });
                        DialogueData.AddNode(node);
                    }

                }

                ImGui.EndMenu();
            }
#endif
        }
    }
}
