﻿#if !RELEASE
using ImGuiNET;
#endif
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class NodeOptionOutPut : NodeOutPut
    {
        public NodeOptionOutPut(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position)
        {
            IsDragbleNode = false;
        }

#if !RELEASE
        public override void DrawInspector()
        {
            string content = Content;
            Fields.Field.DrawLongText("Content", ref content);
            Content = content;
            if (ImGui.Button("Delete Option")) OnDelete();
        }

#endif
        public override void OnDelete()
        {
            base.OnDelete();
            _storage.DeleteNode($"Nodes-Object-{Id}");

            foreach (var node in NodesConnectionIn)
                node.NodesConnectionOut.Remove(this);
            if (ParentNode is NodeWithOptions)
                ((NodeWithOptions)ParentNode).NodeOptions.Remove(this);
            Disconnecting();
        }

        public override void OnSave()
        {
            base.OnSave();
            _storage.SetString($"Nodes-Object-{Id}", typeof(NodeOptionOutPut).Namespace + "." + typeof(NodeOptionOutPut).Name);
        }
#if !RELEASE
        public override void Draw(ImDrawListPtr imDraw)
        {
            DrawConnections(imDraw);
            DrawOutputPoint(imDraw);
            DrawNodeText(imDraw);
        }
#endif
    }
}
