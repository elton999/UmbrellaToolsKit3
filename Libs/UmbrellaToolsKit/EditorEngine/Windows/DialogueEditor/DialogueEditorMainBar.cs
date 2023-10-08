using ImGuiNET;
using UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor
{
    public class DialogueEditorMainBar : IBarEditor
    {
        public DialogueEditorMainBar( Storage.Load storage) { _storage = storage; }

        private Storage.Load _storage;
        private const string _dialogueJsonPath = @"Content/Dialogue1.dn";
        private const string _dialogueSettingPath = @"Content/Dialogue1.Umbrella";

        public void Draw()
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Open..."))
                {
                }

                if (ImGui.MenuItem("Save"))
                {
                    List<float> ids = new List<float>();
                    foreach (var node in DialogueData.Nodes)
                        ids.Add(node.Id);
                    _storage.AddItemFloat("Ids", ids);

                    DialogueEditorWindow.Save();
                }

                if (ImGui.MenuItem("Export Json"))
                    DialogueJsonExport.Export(_dialogueJsonPath);

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
