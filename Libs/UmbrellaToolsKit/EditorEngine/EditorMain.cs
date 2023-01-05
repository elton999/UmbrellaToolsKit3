using ImGuiNET;
using MonoGame.ImGui.Standard;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine
{
    public class EditorMain
    {
        private ImGUIRenderer _imGUIRenderer;
        private BarEdtior _mainBarEditor;
        private EditorArea _editorArea;

        private GameManagement _gameManagement;

        //windows
        private Windows.SceneEditorWindow _sceneEditor;
        private Game _game;

        public EditorMain(Game game, GameManagement gameManagement)
        {
            _game = game;
            _gameManagement = gameManagement;   
            
            _imGUIRenderer = new ImGUIRenderer(game).Initialize().RebuildFontAtlas();
            _mainBarEditor = new BarEdtior();
            _editorArea = new EditorArea();

            _sceneEditor = new Windows.SceneEditorWindow(_gameManagement);
        }

        public void Draw(GameTime gameTime)
        {
            _imGUIRenderer.BeginLayout(gameTime);

            var ImGuiIO = ImGui.GetIO();
            ImGuiIO.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            _mainBarEditor.Draw(gameTime);
            _editorArea.Draw(gameTime);
            _imGUIRenderer.EndLayout();
        }
    }
}
