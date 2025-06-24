using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using MonoGame.ImGui.Extensions;
using MonoGame.ImGui;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit.EditorEngine.Windows.GameSettings
{
    [GameSettingsProperty(nameof(AtlasGameSettings), "/Content/")]
    public class AtlasGameSettings : GameSettingsProperty
    {
        [System.Serializable]
        public class SpriteBody : ISprite
        {
            private Texture2D _texture;
            [ShowEditor] private string _name;
            [ShowEditor] private Vector2 _position;
            [ShowEditor] private Vector2 _size;

            public string Name {get => _name; set => _name = value; }
            public Vector2 Position { get => _position; set => _position = value; }
            public Vector2 Size { get => _size; set => _size = value; }

            public Rectangle GetRectangle() => new Rectangle(Position.ToPoint(), Size.ToPoint());

            public void SetTexture(Texture2D texture) => _texture = texture;

            public Texture2D GetTexture() => _texture;
        }

        [System.Serializable]
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

        private SpriteAtlas _currentSprite;

        private float _zoom = 1.0f;
        private readonly float _zoomStep = 0.1f;
        private readonly float _minZoom = 0.1f;
        private readonly float _maxZoom = 32f;
        private System.Numerics.Vector2 _scrollOffset = System.Numerics.Vector2.Zero;

        private bool isSelecting = false;
        private System.Numerics.Vector2 selectionStart;
        private System.Numerics.Vector2 selectionEnd;
        private ISprite _currentSpriteSelect = null;
        private ISprite _currentSriteHover = null;

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
            ImGui.Begin("Sprite View",
             ImGuiWindowFlags.NoScrollbar);

            ImGuiIOPtr io = ImGui.GetIO();

            System.Numerics.Vector2 mouseScreen = io.MousePos;
            System.Numerics.Vector2 contentStart = ImGui.GetCursorScreenPos();

            if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem))
            {
                float wheel = io.MouseWheel;
                if (wheel != 0)
                {
                    float oldZoom = _zoom;

                    _zoom = MathHelper.Clamp(_zoom + wheel * _zoomStep, _minZoom, _maxZoom);
                    System.Numerics.Vector2 mouseRel = mouseScreen - contentStart;
                    _scrollOffset = (mouseRel + _scrollOffset) * (_zoom / oldZoom) - mouseRel;
                }
            }

            bool isPanning = ImGui.IsWindowHovered() && ImGui.IsMouseDown(ImGuiMouseButton.Middle);
            if (isPanning) _scrollOffset -= io.MouseDelta;

            ImDrawListPtr drawList = ImGui.GetWindowDrawList();
            System.Numerics.Vector2 contentStartPos = ImGui.GetCursorScreenPos();

            System.Numerics.Vector2 winPos = ImGui.GetWindowPos();
            System.Numerics.Vector2 winSize = ImGui.GetWindowSize();
            drawList.AddRectFilled(winPos, winPos + winSize,
                                   ImGui.GetColorU32(ImGuiCol.WindowBg));

            if (_currentSprite != null)
            {
                var tex = _currentSprite.GetTexture();
                System.Numerics.Vector2 texSize = new(tex.Width, tex.Height);
                System.Numerics.Vector2 drawSize = texSize * _zoom;
                System.Numerics.Vector2 topLeft = contentStartPos - _scrollOffset;

                uint borderColor = ImGui.ColorConvertFloat4ToU32(new System.Numerics.Vector4(1, 0, 1, 1));
                drawList.AddRect(topLeft, topLeft + drawSize, borderColor);

                ImGui.SetCursorScreenPos(topLeft);
                ImGui.Image(_currentSprite.GetTextureBuffer(), drawSize);

                var mouseLocal = (mouseScreen - contentStart + _scrollOffset) / _zoom;
                _currentSriteHover = null;

                foreach (var sprite in _currentSprite.Sprites)
                    if (sprite.GetRectangle().Intersects(new Rectangle(mouseLocal.ToXnaVector2().ToPoint(), new Point(1, 1))))
                        _currentSriteHover = sprite;

                if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left) && _currentSriteHover != null)
                    _currentSpriteSelect = _currentSriteHover;

                if (ImGui.IsWindowHovered() && ImGui.IsMouseReleased(ImGuiMouseButton.Left) && _currentSpriteSelect != null && _currentSriteHover == null)
                    _currentSpriteSelect = null;

                if (ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left) && _currentSpriteSelect == null)
                {
                    isSelecting = true;
                    selectionStart = mouseLocal;
                    selectionEnd = mouseLocal;
                }

                if (isSelecting && ImGui.IsMouseDown(ImGuiMouseButton.Left) && _currentSpriteSelect == null)
                    selectionEnd = mouseLocal;

                if (isSelecting && ImGui.IsMouseReleased(ImGuiMouseButton.Left) && _currentSpriteSelect == null)
                {
                    isSelecting = false;

                    topLeft = new System.Numerics.Vector2(
                        Math.Min(selectionStart.X, selectionEnd.X),
                        Math.Min(selectionStart.Y, selectionEnd.Y)
                    );
                    var bottomRight = new System.Numerics.Vector2(
                        Math.Max(selectionStart.X, selectionEnd.X),
                        Math.Max(selectionStart.Y, selectionEnd.Y)
                    );
                    topLeft = topLeft.ToXnaVector2().ToPoint().ToVector2().ToNumericVector2();
                    bottomRight = bottomRight.ToXnaVector2().ToPoint().ToVector2().ToNumericVector2();

                    var size = bottomRight - topLeft;

                    string spriteName = $"{_currentSprite.Path} : {_currentSprite.Sprites.Count}";
                    _currentSprite.Sprites.Add(new SpriteBody() { Name = spriteName, Position = topLeft.ToXnaVector2().ToPoint().ToVector2(), Size = size.ToXnaVector2().ToPoint().ToVector2() });
                }

                if (isSelecting)
                {
                    var topLeftScreen = contentStart - _scrollOffset + selectionStart * _zoom;
                    var bottomRightScreen = contentStart - _scrollOffset + selectionEnd * _zoom;

                    var rectMin = new System.Numerics.Vector2(Math.Min(topLeftScreen.X, bottomRightScreen.X), Math.Min(topLeftScreen.Y, bottomRightScreen.Y));
                    var rectMax = new System.Numerics.Vector2(Math.Max(topLeftScreen.X, bottomRightScreen.X), Math.Max(topLeftScreen.Y, bottomRightScreen.Y));

                    drawList.AddRect(rectMin, rectMax, ImGui.ColorConvertFloat4ToU32(new System.Numerics.Vector4(0, 0.6f, 1, 1)), 0, ImDrawFlags.None, 2.0f);
                }

                foreach (var sprite in _currentSprite.Sprites)
                {
                    var rectStart = contentStart - _scrollOffset + sprite.Position.ToNumericVector2() * _zoom;
                    var rectEnd = rectStart + sprite.Size.ToNumericVector2() * _zoom;

                    var corners = new System.Numerics.Vector2[]
                    {
                        rectStart,
                        new (rectEnd.X, rectStart.Y),
                        new (rectStart.X, rectEnd.Y),
                        rectEnd
                    };

                    var borderColorSpriteDefault = new System.Numerics.Vector4(0, 1, 0, 1);
                    var borderColorHover = new System.Numerics.Vector4(1, 1, 1, 1);
                    var borderColorSprite = sprite == _currentSpriteSelect || sprite == _currentSriteHover ? borderColorHover : borderColorSpriteDefault;

                    drawList.AddRect(rectStart, rectEnd, ImGui.ColorConvertFloat4ToU32(borderColorSprite), 0, ImDrawFlags.None, 1.5f);

                    if (_currentSpriteSelect == sprite)
                    {
                        var cornerSquareSize = new System.Numerics.Vector2(2, 2) * _zoom;
                        foreach (var corner in corners)
                            drawList.AddRectFilled(corner - cornerSquareSize, corner + cornerSquareSize, ImGui.ColorConvertFloat4ToU32(borderColorSprite));
                    }
                }
            }

            ImGui.End();

            ImGui.SetNextWindowDockID(idSpriteBody, ImGuiCond.Once);
            ImGui.Begin("Sprite Data");
            {
                if (_currentSpriteSelect != null)
                {
                    InspectorClass.DrawAllFields(_currentSpriteSelect);
                    if (Fields.Buttons.RedButton("Delete"))
                        if (_currentSprite.Sprites.Remove(_currentSpriteSelect))
                            _currentSpriteSelect = null;
                }
            }
            ImGui.End();
        }
    }
}
