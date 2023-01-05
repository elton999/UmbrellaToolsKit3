using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using MonoGame.ImGui.Standard;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine
{
    public class EditorMain
    {
        private ImGUIRenderer _imGUIRenderer;

        public EditorMain(Game game)
        {
            _imGUIRenderer = new ImGUIRenderer(game).Initialize().RebuildFontAtlas();
        }

        public void Draw(GameTime gameTime)
        {
            _imGUIRenderer.BeginLayout(gameTime);
            ImGui.Begin("basic window");
            ImGui.End();
            _imGUIRenderer.EndLayout();
        }
    }
}
