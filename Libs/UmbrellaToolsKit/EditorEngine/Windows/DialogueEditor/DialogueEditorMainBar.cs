#if !RELEASE
using ImGuiNET;
#endif
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using Microsoft.Xna.Framework;
using System;

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
                if (ImGui.MenuItem("Add Start Node"))
                {
                    var node = new StartNode(_dialogueEditorWindow.Storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                if (ImGui.MenuItem("Add End Node"))
                {
                    var node = new EndNode(_dialogueEditorWindow.Storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                if (ImGui.MenuItem("Add Dialogue Node"))
                {
                    var node = new DialogueNode(_dialogueEditorWindow.Storage, DialogueData.GetNewNodeId(), "Dialogue Node", Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                if (ImGui.MenuItem("Add Sprite Node"))
                {
                    var node = new SpriteNode(_dialogueEditorWindow.Storage, DialogueData.GetNewNodeId(), "Sprite Node", Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                ImGui.EndMenu();
            }
#endif
        }
    }
}
