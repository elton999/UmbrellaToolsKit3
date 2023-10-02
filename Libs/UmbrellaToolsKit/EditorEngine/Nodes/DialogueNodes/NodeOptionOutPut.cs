using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class NodeOptionOutPut : NodeOutPut
    {
        public NodeOptionOutPut(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            IsDragbleNode = false;
            storage.AddItemString($"Nodes-Object-{Id}", new List<string>() { typeof(NodeOptionOutPut).Namespace + "." + typeof(NodeOptionOutPut).Name });
        }

        public override void DrawInspector()
        {
            string content = Content;
            Fields.Field.DrawLongText("Content", ref content);
            Content = content;

            if(ImGui.Button("Delete Option")) OnDelete();
        }

        public override void OnDelete()
        {
            foreach (var node in NodesConnectionIn)
                node.NodesConnectionOut.Remove(this);
            if (ParentNode is NodeWithOptions)
                ((NodeWithOptions)ParentNode).NodeOptions.Remove(this);
            Disconnecting();
            DialogueData.RemoveNode(this);
        }

        public override void Draw(ImDrawListPtr imDraw) 
        {
            DrawConnections(imDraw);
            DrawOutputPoint(imDraw);
            DrawNodeText(imDraw);
        }
    }
}
