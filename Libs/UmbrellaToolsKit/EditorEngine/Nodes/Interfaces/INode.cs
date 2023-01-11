using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INode
    {
        string Name { get; set; }

        Vector2 Position { get; }
        Vector2 MainSquareSize { get; }
        Vector2 SelectedNodeSize { get; }
        Vector2 SelectedNodePosition { get; }
        Vector2 TitleSize { get; }
    }
}
