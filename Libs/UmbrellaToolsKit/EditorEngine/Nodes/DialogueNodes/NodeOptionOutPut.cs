using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard.Extensions;
using UmbrellaToolsKit.Storage;

namespace UmbrellaToolsKit.EditorEngine.Nodes.DialogueNodes
{
    public class NodeOptionOutPut : NodeOutPut
    {
        public NodeOptionOutPut(Load storage, int id, string name, Vector2 position) : base(storage, id, name, position){}

        public override void Update() {}
        public override void Draw(ImDrawListPtr imDraw) 
        {
            DrawConnections(imDraw);
            DrawOutputPoint(imDraw);
            DrawNodeText(imDraw);
        }
    }
}
