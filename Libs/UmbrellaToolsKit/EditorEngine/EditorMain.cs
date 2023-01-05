using ImGuiNET;
using MonoGame.ImGui.Standard;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine
{
    public class EditorMain
    {
        private ImGUIRenderer _imGUIRenderer;
        private BarEdtior _mainBarEditor;

        public EditorMain(Game game)
        {
            _imGUIRenderer = new ImGUIRenderer(game).Initialize().RebuildFontAtlas();
            _mainBarEditor = new BarEdtior();
        }

        public void Draw(GameTime gameTime)
        {
            _imGUIRenderer.BeginLayout(gameTime);
            _mainBarEditor.Draw(gameTime);
            _imGUIRenderer.EndLayout();
        }
    }
}
