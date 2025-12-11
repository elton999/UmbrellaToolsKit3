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

namespace UmbrellaToolsKit.EditorEngine.Windows.GameSettings.Atlas
{
    public class AtlasEditor
    {
        private AtlasGameSettings.SpriteAtlas _currentSprite;

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

        public void DrawSpriteList(uint dockId, EditorMain editorMain, List<AtlasGameSettings.SpriteAtlas> atlas)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("Sprite List");
            {
                foreach (var sprite in atlas)
                {
                    if (!sprite.HasAreadyLoad)
                    {
                        sprite.SetTexture(editorMain.GameManagement.Game.Content.Load<Texture2D>(sprite.Path));
                        sprite.SetBuffer(editorMain.ImGuiRenderer);
                    }

                    if (ImGui.Selectable(sprite.Path, _currentSprite == sprite, ImGuiSelectableFlags.None, new(0, 30.0f)))
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
                        var sprite = new AtlasGameSettings.SpriteAtlas()
                        {
                            Path = openFileDialog.FileName.Replace(".xnb", "").Replace(Directory.GetCurrentDirectory() + @"\Content\", "")
                        };
                        sprite.SetTexture(editorMain.GameManagement.Game.Content.Load<Texture2D>(sprite.Path));
                        sprite.SetBuffer(editorMain.ImGuiRenderer);
                        atlas.Add(sprite);
                    }
                    LoadingWindow.End();
                }
            }
            ImGui.End();
        }

        public void DrawSpriteView(uint dockId, out ImDrawListPtr drawList)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("Sprite View", ImGuiWindowFlags.NoScrollbar);

            var io = ImGui.GetIO();

            var mouseScreen = io.MousePos;
            var contentStart = ImGui.GetCursorScreenPos();

            if (ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem))
            {
                float wheel = io.MouseWheel;
                if (wheel != 0.0f)
                {
                    float oldZoom = _zoom;

                    _zoom = MathHelper.Clamp(_zoom + wheel * _zoomStep, _minZoom, _maxZoom);
                    var mouseRel = mouseScreen - contentStart;
                    _scrollOffset = (mouseRel + _scrollOffset) * (_zoom / oldZoom) - mouseRel;
                }
            }

            bool isPanning = ImGui.IsWindowHovered() && ImGui.IsMouseDown(ImGuiMouseButton.Middle);
            if (isPanning) _scrollOffset -= io.MouseDelta;


            var contentStartPos = ImGui.GetCursorScreenPos();

            var winPos = ImGui.GetWindowPos();
            var winSize = ImGui.GetWindowSize();
            drawList = ImGui.GetWindowDrawList();
            drawList.AddRectFilled(winPos, winPos + winSize,
                                   ImGui.GetColorU32(ImGuiCol.WindowBg));

            if (_currentSprite != null)
            {
                var tex = _currentSprite.GetTexture();
                System.Numerics.Vector2 texSize = new(tex.Width, tex.Height);
                var drawSize = texSize * _zoom;
                var topLeft = contentStartPos - _scrollOffset;

                uint borderColor = ImGui.ColorConvertFloat4ToU32(new(1, 0, 1, 1));
                drawList.AddRect(topLeft, topLeft + drawSize, borderColor);

                ImGui.SetCursorScreenPos(topLeft);
                ImGui.Image(_currentSprite.GetTextureBuffer(), drawSize);

                var mouseLocal = (mouseScreen - contentStart + _scrollOffset) / _zoom;
                _currentSriteHover = null;

                foreach (var sprite in _currentSprite.Sprites)
                    if (sprite.GetRectangle().Intersects(new Rectangle(mouseLocal.ToXnaVector2().ToPoint(), new Point(1, 1))))
                        _currentSriteHover = sprite;

                bool hasClicked = ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left);
                bool hasReleased = ImGui.IsWindowHovered() && ImGui.IsMouseReleased(ImGuiMouseButton.Left);

                if (hasClicked && _currentSriteHover != null)
                    _currentSpriteSelect = _currentSriteHover;

                if (hasReleased && _currentSpriteSelect != null && _currentSriteHover == null)
                    _currentSpriteSelect = null;

                if (hasClicked && _currentSpriteSelect == null)
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
                    topLeft = topLeft.ToXnaVector2().Truncate().ToNumericVector2();
                    bottomRight = bottomRight.ToXnaVector2().Truncate().ToNumericVector2();

                    var size = bottomRight - topLeft;

                    string spriteName = $"{_currentSprite.Path} : {_currentSprite.Sprites.Count}";
                    _currentSprite.Sprites.Add(
                        new AtlasGameSettings.SpriteBody()
                        {
                            Name = spriteName,
                            Position = topLeft.ToXnaVector2(),
                            Size = size.ToXnaVector2(),
                            Path = _currentSprite.Path
                        });
                }

                if (isSelecting)
                {
                    var topLeftScreen = contentStart - _scrollOffset + selectionStart * _zoom;
                    var bottomRightScreen = contentStart - _scrollOffset + selectionEnd * _zoom;

                    var rectMin = new System.Numerics.Vector2(Math.Min(topLeftScreen.X, bottomRightScreen.X), Math.Min(topLeftScreen.Y, bottomRightScreen.Y));
                    var rectMax = new System.Numerics.Vector2(Math.Max(topLeftScreen.X, bottomRightScreen.X), Math.Max(topLeftScreen.Y, bottomRightScreen.Y));

                    drawList.AddRect(rectMin, rectMax, ImGui.ColorConvertFloat4ToU32(new(0, 0.6f, 1, 1)), 0, ImDrawFlags.None, 2.0f);
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
        }

        public void DrawSpriteData(uint dockId, ImDrawListPtr drawList)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("Sprite Data");
            {
                ImGui.BeginChild("sprite data");
                drawList = ImGui.GetWindowDrawList();
                if (_currentSpriteSelect != null)
                {
                    float windowWidth = 120.0f;
                    var spriteViewSize = new System.Numerics.Vector2(windowWidth, windowWidth);
                    var windowPos = ImGui.GetWindowPos();
                    uint backgroundColor = ImGui.ColorConvertFloat4ToU32(new(1, 1, 1, 1));

                    var uv0 = _currentSpriteSelect.Position / _currentSprite.GetTexture().Bounds.Size.ToVector2();
                    var uv1 = uv0 + _currentSpriteSelect.Size / _currentSprite.GetTexture().Bounds.Size.ToVector2();

                    float spritePreViewScale = _currentSpriteSelect.Size.X > _currentSpriteSelect.Size.Y ? spriteViewSize.X / _currentSpriteSelect.Size.X : spriteViewSize.Y / _currentSpriteSelect.Size.Y;
                    ImGui.Image(_currentSprite.GetTextureBuffer(), spriteViewSize, uv0.ToNumericVector2(), uv1.ToNumericVector2());


                    InspectorClass.DrawAllFields(_currentSpriteSelect);
                    if (Fields.Buttons.RedButton("Delete"))
                        if (_currentSprite.Sprites.Remove(_currentSpriteSelect))
                            _currentSpriteSelect = null;
                }

                if (_currentSpriteSelect == null && _currentSprite != null)
                    if (Fields.Buttons.RedButton("Delete All selections"))
                        _currentSprite.Sprites.Clear();
                ImGui.EndChild();
            }
            ImGui.End();
        }
    }
}