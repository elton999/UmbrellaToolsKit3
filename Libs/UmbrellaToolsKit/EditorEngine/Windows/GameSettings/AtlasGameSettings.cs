using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoGame.ImGui;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.Interfaces;
using UmbrellaToolsKit.EditorEngine.Windows.GameSettings.Atlas;

namespace UmbrellaToolsKit.EditorEngine.Windows.GameSettings
{
    [GameSettingsProperty(nameof(AtlasGameSettings), "/Content/")]
    public class AtlasGameSettings : GameSettingsProperty
    {
        [Serializable]
        public class SpriteBody : ISprite
        {
            private Texture2D _texture;
            [ShowEditor] private string _name;
            [ShowEditor] private Vector2 _position;
            [ShowEditor] private Vector2 _size;
            [ShowEditor] private string _id;
            private string _path;


            public string Name { get => _name; set => _name = value; }
            public Vector2 Position { get => _position; set => _position = value; }
            public Vector2 Size { get => _size; set => _size = value; }
            public string Path { get => _path; set => _path = value; }
            public string Id
            {
                get
                {
                    SetId();
                    return _id;
                }
                set
                {
                    if (string.IsNullOrEmpty(value)) SetId();
                    else _id = value;
                }
            }

            public Rectangle GetRectangle() => new Rectangle(Position.ToPoint(), Size.ToPoint());

            public void SetTexture(Texture2D texture) => _texture = texture;

            public Texture2D GetTexture() => _texture;

            private void SetId()
            {
                if (string.IsNullOrEmpty(_id))
                    _id = Guid.NewGuid().ToString();
            }
        }

        [Serializable]
        public class SpriteAtlas : ITexture
        {
            private Texture2D _texture;
            private IntPtr _bufferID;
            [ShowEditor] public string Path;
            [ShowEditor] public List<ISprite> Sprites = new();
            public bool HasAreadyLoad => _texture != null;

            public void SetBuffer(ImGuiRenderer imGuiRenderer)
            {
                if (_bufferID.ToInt32() > 0) return;
                _bufferID = imGuiRenderer.BindTexture(_texture);
            }

            public IntPtr GetTextureBuffer() => _bufferID;

            public void SetTexture(Texture2D texture) => _texture = texture;

            public Texture2D GetTexture() => _texture;
        }

        [ShowEditor] public List<SpriteAtlas> Atlas = new();

        public bool TryGetSpriteByName(string name, out ISprite sprite)
        {
            foreach (var atlas in Atlas)
            {
                foreach (var spriteBody in atlas.Sprites)
                {
                    if (spriteBody.Name == name)
                    {
                        sprite = spriteBody;
                        return true;
                    }
                }
            }

            sprite = null;
            return false;
        }

        private AtlasEditor atlasEditor = new AtlasEditor();

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


            atlasEditor.DrawSpriteList(idSpriteList, editorMain, Atlas);
            ImDrawListPtr drawList;
            atlasEditor.DrawSpriteView(idSpriteView, out drawList);
            atlasEditor.DrawSpriteData(idSpriteBody, drawList);
        }
    }
}
