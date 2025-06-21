using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGui;
using MonoGame.ImGui.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.Input;

namespace UmbrellaToolsKit.EditorEngine.Windows.GameSettings
{
    [GameSettingsProperty(nameof(AtlasGameSettings), "/Content/")]
    public class AtlasGameSettings : GameSettingsProperty
    {
        [System.Serializable]
        public class SpriteBody
        {
            [ShowEditor] public string Name;
            [ShowEditor] public Vector2 Position;
            [ShowEditor] public Vector2 Size;
        }

        [System.Serializable]
        public class SpriteAtlas
        {
            private Texture2D _texture;
            private System.IntPtr _bufferID;
            [ShowEditor] public string Path;
            [ShowEditor] public List<SpriteBody> Sprites = new();
            public bool HasAreadyLoad => _texture != null;

            public void SetTexture(Texture2D texture) => _texture = texture;

            public void SetBuffer(ImGuiRenderer imGuiRenderer)
            {
                if (_bufferID.ToInt32() > 0) return;
                _bufferID = imGuiRenderer.BindTexture(_texture);
            }

            public System.IntPtr GetTextureBuffer() => _bufferID;

            public Texture2D GetTexture() => _texture;
        }

        private SpriteAtlas _currentSprite;
        private float _zoom = 1.0f;
        private const float _zoomFactor = 0.3f;
        private float _currentZoomFactor = 1.0f;
        private float _previousScrollValue = MouseHandler.ScrollValue;

        [ShowEditor] public List<SpriteAtlas> Atlas = new();

        public override void DrawFields(EditorMain editorMain)
        {
            uint idSpriteList = ImGui.GetID("SpriteList");
            uint idSpriteView = ImGui.GetID("SpriteView");
            uint idSpriteBody = ImGui.GetID("SpriteBody");

            ImGui.BeginChild("spriteLeft", new System.Numerics.Vector2(ImGui.GetWindowWidth() * 0.2f, 0));
            ImGui.DockSpace(idSpriteList, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("spriteMiddle", new System.Numerics.Vector2(ImGui.GetWindowWidth() * 0.6f, 0));
            ImGui.DockSpace(idSpriteView, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("spriteRight", new System.Numerics.Vector2(ImGui.GetWindowWidth() * 0.2f, 0));
            ImGui.DockSpace(idSpriteBody, new System.Numerics.Vector2(0, 0));
            ImGui.EndChild();

            ImGui.SetNextWindowDockID(idSpriteList, ImGuiCond.Once);
            ImGui.Begin("Sprite List");
            {
                foreach (var sprite in Atlas)
                {
                    if (!sprite.HasAreadyLoad)
                    {
                        sprite.SetTexture(editorMain.GameManagement.Game.Content.Load<Texture2D>(sprite.Path));
                        sprite.SetBuffer(editorMain.ImGuiRenderer);
                    }

                    if (ImGui.Selectable(sprite.Path, _currentSprite == sprite, ImGuiSelectableFlags.None, new System.Numerics.Vector2(0, 30.0f)))
                    {
                        _currentSprite = sprite;
                    }
                    ImGui.Separator();
                }

                ImGui.SetWindowFontScale(1.2f);
                if (Fields.Buttons.BlueButtonLarge("add Sprite"))
                {
                    LoadingWindow.Begin();
                    var openFileDialog = OpenFileDialogue.OpenFileDialog("Import Image", "Sprite", ".xnb");
                    if (OpenFileDialogue.SaveFileDialog(openFileDialog))
                    {
                        var sprite = new SpriteAtlas()
                        {
                            Path = openFileDialog.FileName.Replace(".xnb", "").Replace(Directory.GetCurrentDirectory() + @"\Content\", "")
                        };
                        sprite.SetTexture(editorMain.GameManagement.Game.Content.Load<Texture2D>(sprite.Path));
                        sprite.SetBuffer(editorMain.ImGuiRenderer);
                        Atlas.Add(sprite);
                    }
                    LoadingWindow.End();
                }
            }
            ImGui.End();

            ImGui.SetNextWindowDockID(idSpriteView, ImGuiCond.Once);
            ImGui.Begin("Sprite View", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoMouseInputs);
            {
                if (MouseHandler.ScrollValue > _previousScrollValue)
                    _currentZoomFactor += _zoomFactor;
                if (MouseHandler.ScrollValue < _previousScrollValue)
                    _currentZoomFactor -= _zoomFactor;

                _currentZoomFactor = Math.Max(0.1f, _currentZoomFactor);
                _previousScrollValue = MouseHandler.ScrollValue;

                var drawList = ImGui.GetWindowDrawList();
                var windowPosition = ImGui.GetWindowPos();
                var windowSize = ImGui.GetWindowSize();

                Primitives.Square.Draw(
                    drawList,
                    windowPosition.ToXnaVector2(),
                    windowSize.ToXnaVector2(),
                    Color.Black
                );

                if (_currentSprite != null)
                {
                    var size = new System.Numerics.Vector2(_currentSprite.GetTexture().Width, _currentSprite.GetTexture().Height);
                    var position = windowPosition + windowSize* 0.5f - size * _currentZoomFactor * 0.5f;

                    Primitives.Square.Draw(
                        drawList,
                        position.ToXnaVector2(),
                        size.ToXnaVector2() * _currentZoomFactor,
                        Color.Fuchsia
                    );

                    ImGui.SetCursorPos(windowSize * 0.5f - size * _currentZoomFactor * 0.5f);
                    ImGui.Image(_currentSprite.GetTextureBuffer(), size * _currentZoomFactor);
                }

            }
            ImGui.End();
        }
    }
}
