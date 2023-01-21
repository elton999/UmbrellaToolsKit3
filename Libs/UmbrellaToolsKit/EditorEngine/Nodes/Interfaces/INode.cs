using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine.Nodes.Interfaces
{
    public interface INode
    {
        int Id { get; set; }
        string Name { get; set; }

        Vector2 Position { get; set; }
        Vector2 MainSquareSize { get; }
        Vector2 SelectedNodeSize { get; }
        Vector2 SelectedNodePosition { get; }
        Vector2 TitleSize { get; }
    }
}
