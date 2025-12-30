using UmbrellaToolsKit.EditorEngine.Attributes;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;
using UmbrellaToolsKit.EditorEngine.Primitives;
using System;
using UmbrellaToolsKit.EditorEngine.Windows;

namespace UmbrellaToolsKit.EditorEngine.GameSettings
{
    public abstract class TimelineItem
    {
        [ShowEditor] public float Start;
        [ShowEditor] public float Duration;
        [ShowEditor] public string Name;

        protected bool _isMouseHover = false;
        protected bool _isSelected = false;

        public bool IsSelected { get => _isSelected; set => _isSelected = value; }

        public virtual void Draw(ImDrawListPtr drawList, Vector2 position, SpriteAnimationGameSettings timeLineSettings)
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
                Microsoft.Xna.Framework.Color.Red
            );

            drawList.AddText
            (
                timeLinePosition,
                ImGui.GetColorU32(Vector4.One),
                Name
            );

            HandleMouse(position, timeLineSettings);
        }

        private void HandleMouse(Vector2 position, SpriteAnimationGameSettings timeLineSettings)
        {
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

    public class TimelineEvent : TimelineItem
    {
        [ShowEditor] public string EventId;
    }

    public class TimelineSequence : TimelineItem
    {

    }

    [GameSettingsProperty(nameof(SpriteAnimationGameSettings), "/Content/")]
    public class SpriteAnimationGameSettings : GameSettingsProperty
    {
        private TimelineItem _selected;
        private int _trackSelected = -1;
        private int _trackHover = -1;
        private bool _clickedHoverRule = false;

        [ShowEditor] private float _currentTime = 0.5f;
        [ShowEditor] private float _durationInSeconds = 2f;
        private float _totalFramesInWindow = 250f;
        private float _framePerSecond = 60f;
        private int _frameStep = 15;
        private float _timelineRuleHight = 15f;
        private float _stepSize;
        private float _timeLineHight = 30f;
        private Vector2 _timeLinePosition;

        private List<Type> _timeLineItemTypes = new List<Type>()
        {
            typeof(TimelineEvent),
            typeof(TimelineSequence),
        };

        [ShowEditor]
        public List<List<TimelineItem>> TimeLines = new List<List<TimelineItem>>()
        {
            new List<TimelineItem>()
            {
                new TimelineSequence() { Name = "teste 1", Start = 0, Duration = 5f * ( 1f / 60f)  },
                new TimelineSequence() { Name = "teste 2", Start =  15f * ( 1f / 60f), Duration =  20f * ( 1f / 60f) }
            },
            new List<TimelineItem>()
            {
                new TimelineSequence() { Name = "teste 3", Start = 3f * ( 1f / 60f), Duration = 5f* ( 1f / 60f)},
                new TimelineSequence() { Name = "teste 4", Start =  9f * ( 1f / 60f), Duration =  10f* ( 1f / 60f) }
            }
        };

        public float DurationInSeconds { get => _durationInSeconds; set => _durationInSeconds = value; }
        public float TotalFramesInWindow { get => _totalFramesInWindow; set => _totalFramesInWindow = value; }
        public float FramePerSecond { get => _framePerSecond; set => _framePerSecond = value; }
        public int FrameStep { get => _frameStep; set => _frameStep = value; }
        public float TimelineRuleHight { get => _timelineRuleHight; set => _timelineRuleHight = value; }
        public Vector2 TimeLinePosition { get => _timeLinePosition; set => _timeLinePosition = value; }
        public float StepSize { get => _stepSize; set => _stepSize = value; }
        public float TimeLineHight { get => _timeLineHight; set => _timeLineHight = value; }

        public void SetSelectedItem(TimelineItem timelineItem)
        {
            if (_selected != null)
                _selected.IsSelected = false;
            _selected = timelineItem;
        }

        public void DrawTimeLine(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("TimelineDock", ImGuiWindowFlags.NoScrollbar);

            _timeLinePosition = ImGui.GetCursorScreenPos();
            var drawList = ImGui.GetWindowDrawList();

            DrawTimeLineRule(drawList, _timeLinePosition);
            DrawTracks(drawList, _timeLinePosition);
            DrawTimeCursor(drawList, _timeLinePosition);

            HandleTrackSelect(_timeLinePosition);
            HandleCursorTrackMouse(_timeLinePosition);

            ImGui.End();
        }

        public void DrawProperties(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("PropertiesDock");

            if (Fields.Buttons.BlueButton(">", new Vector2(50f, 0f)))
            {

            }

            var currentTime = TimeSpan.FromSeconds(_currentTime);
            var totalTime = TimeSpan.FromSeconds(_durationInSeconds);
            string timerInfo = $"timer {currentTime.Minutes}:{currentTime.Seconds}:{currentTime.Milliseconds} ({totalTime.Minutes}:{totalTime.Seconds}:{totalTime.Milliseconds})";
            ImGui.Text(timerInfo);

            InspectorClass.DrawAllFields(this);

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            foreach (var timeLineItem in _timeLineItemTypes)
            {
                if (Fields.Buttons.BlueButton($"Add {AttributesHelper.FormatName(timeLineItem.Name)}"))
                {

                }
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            if (_selected != null)
                InspectorClass.DrawAllFields(_selected);

            ImGui.End();
        }

        public void HandleTrackSelect(Vector2 position)
        {
            _trackHover = -1;
            if (!ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem)) return;

            var io = ImGui.GetIO();
            var mouseScreen = io.MousePos;

            int trackCount = 0;
            float offsetY = _timelineRuleHight;
            foreach (var timelineItem in TimeLines)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * trackCount;
                if (yPosition <= mouseScreen.Y && yPosition + TimeLineHight >= mouseScreen.Y)
                {
                    _trackHover = trackCount;
                    if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                        _trackSelected = trackCount;
                }
                trackCount++;
            }
        }

        public void DrawTracks(ImDrawListPtr drawList, Vector2 position)
        {
            float timeLineWidth = ImGui.GetWindowSize().X;

            int trackCount = 0;
            float offsetY = _timelineRuleHight;
            var selectedColor = Microsoft.Xna.Framework.Color.DarkGray;
            var hoverColor = Microsoft.Xna.Framework.Color.Gray;

            foreach (var timelineItem in TimeLines)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * trackCount;
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, TimeLineHight),
                    Microsoft.Xna.Framework.Color.White
                );

                var trackColor = trackCount == _trackHover ? hoverColor : Microsoft.Xna.Framework.Color.Black;
                trackColor = trackCount == _trackSelected ? selectedColor : trackColor;
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition + 1),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, TimeLineHight - 2),
                    trackColor
                );

                trackCount++;
            }

            trackCount = 0;
            foreach (var timeLine in TimeLines)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * trackCount;
                foreach (var timeLineItem in timeLine)
                    timeLineItem.Draw(drawList, new Vector2(position.X, yPosition), this);
                trackCount++;
            }
        }

        public void HandleCursorTrackMouse(Vector2 position)
        {
            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                _clickedHoverRule = false;

            if (!ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem)) return;
            var io = ImGui.GetIO();
            var mouseScreen = io.MousePos;

            if (position.Y <= mouseScreen.Y && position.Y + _timelineRuleHight >= mouseScreen.Y)
            {
                bool clicked = ImGui.IsMouseClicked(ImGuiMouseButton.Left);
                if (!clicked && !_clickedHoverRule) return;

                _currentTime = GetTimeLineOnPositionX(mouseScreen.X - position.X);
                _clickedHoverRule = true;
            }
        }

        public void DrawTimeLineRule(ImDrawListPtr drawList, Vector2 position)
        {
            float timeLineWidth = ImGui.GetWindowSize().X;
            _stepSize = timeLineWidth / _totalFramesInWindow;
            int totalFrames = (int)(_durationInSeconds * _framePerSecond);

            drawList.AddLine(
                new Vector2(position.X, position.Y + _timelineRuleHight),
                new Vector2(position.X + timeLineWidth, position.Y + _timelineRuleHight),
                ImGui.GetColorU32(Vector4.One)
            );

            for (int frameIndex = 0; frameIndex < totalFrames; frameIndex += _frameStep)
            {
                drawList.AddText
                (
                    new Vector2(position.X + _stepSize * frameIndex, position.Y),
                    ImGui.GetColorU32(Vector4.One),
                    $"{frameIndex}|"
                );
            }
        }

        public void DrawTimeCursor(ImDrawListPtr drawList, Vector2 position)
        {
            float xPosition = GetPositionXOnTimeLine(_currentTime);

            drawList.AddTriangleFilled
            (
                new Vector2(xPosition + position.X - 5f, position.Y),
                new Vector2(xPosition + position.X + 5f, position.Y),
                new Vector2(xPosition + position.X, position.Y + 5f),
                ImGui.GetColorU32(Microsoft.Xna.Framework.Color.Yellow.PackedValue)
            );

            drawList.AddLine
            (
               new Vector2(xPosition + position.X, position.Y),
               new Vector2(xPosition + position.X, position.Y + 200f),
               ImGui.GetColorU32(Microsoft.Xna.Framework.Color.Yellow.PackedValue)
            );
        }

        public override void DrawFields(EditorMain editorMain)
        {
            uint idProperties = ImGui.GetID("Properties");
            uint idTimeline = ImGui.GetID("Timeline");

            ImGui.BeginChild("timelineLeft", new Vector2(ImGui.GetWindowWidth() * 0.15f, 0));
            ImGui.DockSpace(idProperties, new Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("timelineRight", new Vector2(ImGui.GetWindowWidth() * 0.85f, 0));
            ImGui.DockSpace(idTimeline, new Vector2(0, 0));
            ImGui.EndChild();

            DrawTimeLine(idTimeline);
            DrawProperties(idProperties);
        }

        [Button]
        public void AddTrack()
        {
            TimeLines.Add(new List<TimelineItem>());
        }

        public float GetPositionXOnTimeLine(float valueInSeconds)
        {
            return valueInSeconds * FramePerSecond * StepSize;
        }

        public float GetTimeLineOnPositionX(float positionValue)
        {
            return positionValue / (FramePerSecond * StepSize);
        }
    }
}