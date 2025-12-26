#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui;
#endif
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolsKit.EditorEngine.Windows.Interfaces;

namespace UmbrellaToolsKit.EditorEngine
{
    public class EditorMain
    {
#if !RELEASE
        public ImGuiRenderer _imGUIRenderer;
#endif
        private BarEditor _mainBarEditor;
        private EditorArea _editorArea;

        private GameManagement _gameManagement;
        private Game _game;

        //windows
        private IWindowEditable _sceneEditor;
        private IWindowEditable _dialogueEditor;
        private IWindowEditable _gameSettingsEditor;
        private bool _showEditor = false;
        private bool _showEditorKeyUp = true;

        public GameManagement GameManagement => _gameManagement;
#if !RELEASE
        public ImGuiRenderer ImGuiRenderer => _imGUIRenderer;
#endif
        public static event Action OnDrawOverLayer;

        public EditorMain(Game game, GameManagement gameManagement)
        {
            _game = game;
            _gameManagement = gameManagement;
#if !RELEASE
            _imGUIRenderer = new ImGuiRenderer(game).Initialize().RebuildFontAtlas();
#endif
            _mainBarEditor = new BarEditor();
            _editorArea = new EditorArea();

            _sceneEditor = new Windows.SceneEditorWindow(_gameManagement);
            _dialogueEditor = new Windows.DialogueEditorWindow(_gameManagement);
            _gameSettingsEditor = new Windows.GameSettingsWindow(_gameManagement, this);
        }

        public void Draw(GameTime gameTime)
        {
#if !RELEASE
            if (Keyboard.GetState().IsKeyDown(Keys.F12) && !_showEditorKeyUp)
            {
                _showEditor = !_showEditor;
                _showEditorKeyUp = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.F12))
                _showEditorKeyUp = false;
            if (!_showEditor) return;

            _imGUIRenderer.BeginLayout(gameTime);

            var ImGuiIO = ImGui.GetIO();
            ImGuiIO.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            _mainBarEditor.Draw(gameTime);
            OnDrawOverLayer?.Invoke();
            _editorArea.Draw(gameTime);
            _imGUIRenderer.EndLayout();

#endif
        }
    }
}
