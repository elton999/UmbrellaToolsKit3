using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

        [ShowEditor] public List<SpriteAtlas> Atlas = new();

        public override void DrawFields()
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
        }
    }
}
