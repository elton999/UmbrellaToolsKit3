using System.Numerics;
using ImGuiNET;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.EditorEngine.Primitives;

namespace UmbrellaToolsKit.EditorEngine.Windows.Feature
{
    public abstract class TimelineItem
    {
        [ShowEditor] public float Start;
        [ShowEditor] public float Duration;
        [ShowEditor] public string Name;

        protected bool _isMouseHover = false;
        protected bool _isSelected = false;
        protected virtual bool _showName => true;

        protected virtual Microsoft.Xna.Framework.Color _color => Microsoft.Xna.Framework.Color.Red;

        public bool IsSelected { get => _isSelected; set => _isSelected = value; }

        public virtual void Draw(ImDrawListPtr drawList, Vector2 position, TimeLineFeature timeLineSettings)
        {
            var timeLinePosition = new Vector2(position.X + timeLineSettings.GetPositionXOnTimeLine(Start), position.Y);

            if (_isMouseHover || _isSelected)
            {
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(timeLinePosition.X - 1, timeLinePosition.Y),
                    new Microsoft.Xna.Framework.Vector2(timeLineSettings.GetPositionXOnTimeLine(Duration) + 2, timeLineSettings.TimeLineHight),
                    Microsoft.Xna.Framework.Color.Yellow
                );
            }

            Square.Draw(
                drawList,
                new Microsoft.Xna.Framework.Vector2(timeLinePosition.X, timeLinePosition.Y + 1f),
                new Microsoft.Xna.Framework.Vector2(timeLineSettings.GetPositionXOnTimeLine(Duration), timeLineSettings.TimeLineHight - 2f),
                _color
            );

            if (_showName)
            {
                drawList.AddText
                (
                    timeLinePosition,
                    ImGui.GetColorU32(Vector4.One),
                    Name
                );
            }

            HandleMouse(position, timeLineSettings);
        }

        public virtual void DrawProperties()
        {
            InspectorClass.DrawAllFields(this);
        }

        private void HandleMouse(Vector2 position, TimeLineFeature timeLineSettings)
        {
            if (_isSelected && timeLineSettings.CurrentItem != this)
                _isSelected = false;

            _isMouseHover = false;

            if (!ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem))
                return;

            var mouse = ImGui.GetIO().MousePos;

            var pos = new Vector2(
                position.X + timeLineSettings.GetPositionXOnTimeLine(Start),
                position.Y
            );

            var size = new Vector2(
                timeLineSettings.GetPositionXOnTimeLine(Duration),
                timeLineSettings.TimeLineHight
            );

            bool hover =
                mouse.X >= pos.X &&
                mouse.X <= pos.X + size.X &&
                mouse.Y >= pos.Y &&
                mouse.Y <= pos.Y + size.Y;

            _isMouseHover = hover;

            if (hover && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                timeLineSettings.SetSelectedItem(this);
                _isSelected = true;
            }
        }
    }
}