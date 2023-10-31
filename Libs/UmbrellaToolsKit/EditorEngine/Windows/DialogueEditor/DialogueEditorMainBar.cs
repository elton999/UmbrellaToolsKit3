using ImGuiNET;
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Eto.Forms;
using System;

namespace UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor
{
    public class DialogueEditorMainBar : IBarEditor
    {
        public static event Action<string> OnAnyOpenFile;

        public DialogueEditorMainBar( Storage.Load storage) { _storage = storage; }

        private Storage.Load _storage;
        private string _dialogueJsonPath;
        private string _dialogueSettingPath;

        public void Draw()
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Open..."))
                {
                    var openFileDialog = OpenFileDialogue.OpenFileDialog("Open file", "Dialogue Nodes", ".Umbrella");
                    if(OpenFileDialogue.SaveFileDialog(openFileDialog))
                    {
                        _dialogueSettingPath = openFileDialog.FileName;
                        OnAnyOpenFile?.Invoke(_dialogueSettingPath);
                    }
                }

                if (ImGui.MenuItem("Save"))
                {
                    List<float> ids = new List<float>();
                    foreach (var node in DialogueData.Nodes)
                        ids.Add(node.Id);
                    _storage.AddItemFloat("Ids", ids);

                    DialogueEditorWindow.Save();
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
                    var node = new StartNode(_storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                if (ImGui.MenuItem("Add End Node"))
                {
                    var node = new EndNode(_storage, DialogueData.GetNewNodeId(), null, Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                if (ImGui.MenuItem("Add Nove With Options"))
                {
                    var node = new NodeWithOptions(_storage, DialogueData.GetNewNodeId(), "new node", Vector2.One * 500f);
                    DialogueData.AddNode(node);
                }

                ImGui.EndMenu();
            }
        }
    }
}
