using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine.Windows.Interfaces
{
    public interface IWindowEditable
    {
        public GameManagement GameManagement { get; }
        void SetAsMainWindow();
        void RemoveAsMainWindow();

        void ShowWindow(GameTime gameTime);
    }
}
