using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Extensions;
using System.Collections.Generic;
using System.IO;
using UmbrellaToolsKit.EditorEngine.Attributes;

namespace UmbrellaToolsKit.EditorEngine.Windows.GameSettings
{
    [GameSettingsProperty(nameof(InputGameSettings), "/Content/")]
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
            [ShowEditor] public string Path;
            [ShowEditor] public List<SpriteBody> Sprites = new();
        }

        private SpriteAtlas _currentSprite;

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
                    if (ImGui.Selectable(sprite.Path, _currentSprite == sprite, ImGuiSelectableFlags.None, new System.Numerics.Vector2(0, 30.0f)))
                    {
                        _currentSprite = sprite;
                    }
                    ImGui.Separator();
                }

                ImGui.SetWindowFontScale(1.2f);
                if (Fields.Buttons.BlueButtonLarge("add Sprite"))
                {
                    var openFileDialog = OpenFileDialogue.OpenFileDialog("Import Image", "Sprite", ".xnb");
                    if (OpenFileDialogue.SaveFileDialog(openFileDialog))
                    {
                        Atlas.Add(new SpriteAtlas()
                        {
                            Path = openFileDialog.FileName.Replace(".xnb", "").Replace(Directory.GetCurrentDirectory()+@"\", "")
                        });
                    }
                }
            }
            ImGui.End();

            ImGui.SetNextWindowDockID(idSpriteView, ImGuiCond.Once);
            ImGui.Begin("Sprite View");
            {
                var drawList = ImGui.GetWindowDrawList();
                var windowPosition = ImGui.GetWindowPos();
                var windowSize = ImGui.GetWindowSize();

                Primitives.Square.Draw(
                    drawList,
                    windowPosition.ToXnaVector2(),
                    windowSize.ToXnaVector2(),
                    Color.DarkGray
                );
            }
            ImGui.End();
        }
    }
}
