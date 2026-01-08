using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.EditorEngine.Primitives;
using UmbrellaToolsKit.Utils;

namespace UmbrellaToolsKit.EditorEngine.Windows.Feature
{
    public class TimeLineFeature
    {
        public class Track
        {
            public List<TimelineItem> Items = new();
        }

        public enum State
        {
            PLAYING,
            STOPPED
        }

        private Timer timer;

        private TimelineItem _selectedSequenceItem;
        private int _trackSelected = -1;
        private int _trackHover = -1;
        private bool _clickedHoverRule = false;
        private State _currentState = State.STOPPED;

        [ShowEditor] public float _currentTime = 0f;
        [ShowEditor] public float _durationInSeconds = 2f;
        private float _totalFramesInWindow = 250f;
        [ShowEditor] public float _framePerSecond = 60f;
        private int _frameStep = 15;
        private float _timelineRuleHight = 15f;
        private float _stepSize;
        private float _timeLineHight = 30f;
        private float _timePerFrame => 1f / _framePerSecond;
        private Vector2 _timeLinePosition;

        protected virtual List<Type> _timeLineItemTypes { get; }

        [ShowEditor]
        public List<Track> Tracks = new();

        public State CurrentState { get => _currentState; }
        public float CurrentTimeInSeconds { get => _currentTime; }

        public float DurationInSeconds { get => _durationInSeconds; set => _durationInSeconds = value; }
        public float TotalFramesInWindow { get => _totalFramesInWindow; set => _totalFramesInWindow = value; }
        public float FramePerSecond { get => _framePerSecond; set => _framePerSecond = value; }
        public int FrameStep { get => _frameStep; set => _frameStep = value; }
        public float TimelineRuleHight { get => _timelineRuleHight; set => _timelineRuleHight = value; }
        public Vector2 TimeLinePosition { get => _timeLinePosition; set => _timeLinePosition = value; }
        public float StepSize { get => _stepSize; set => _stepSize = value; }
        public float TimeLineHight { get => _timeLineHight; set => _timeLineHight = value; }
        public TimelineItem CurrentItem => _selectedSequenceItem;

        public virtual void DrawTimeLine()
        {
            _timeLinePosition = ImGui.GetCursorScreenPos();
            var drawList = ImGui.GetWindowDrawList();

            float scrollX = ImGui.GetScrollX();
            float scrollY = ImGui.GetScrollY();

            _timeLinePosition = ImGui.GetCursorScreenPos();
            _timeLinePosition.X -= scrollX;
            _timeLinePosition.Y -= scrollY;

            float contentWidth = _durationInSeconds * _framePerSecond * StepSize + 200f;
            float contentHeight = _timelineRuleHight + Tracks.Count * TimeLineHight + 50f;
            ImGui.SetCursorPos(new Vector2(contentWidth, contentHeight));


            DrawTimeLineRule(drawList, _timeLinePosition);
            DrawTracks(drawList, _timeLinePosition);
            DrawTimeCursor(drawList, _timeLinePosition);

            HandleTrackSelect(_timeLinePosition);
            HandleCursorTrackMouse(_timeLinePosition);
        }

        public virtual void DrawProperties()
        {
            if (Fields.Buttons.BlueButton("|<", new Vector2(50f, 0f)))
                _currentTime = 0f;

            ImGui.SameLine();
            if (Fields.Buttons.BlueButton(_currentState is State.STOPPED ? ">" : "||", new Vector2(50f, 0f)))
            {
                switch (_currentState)
                {
                    case State.PLAYING:
                        Stop();
                        break;
                    case State.STOPPED:
                        Play();
                        break;
                }
            }

            var currentTime = TimeSpan.FromSeconds(_currentTime);
            var totalTime = TimeSpan.FromSeconds(_durationInSeconds);
            string timerInfo = $"timer {currentTime.Minutes}:{currentTime.Seconds}:{currentTime.Milliseconds} ({totalTime.Minutes}:{totalTime.Seconds}:{totalTime.Milliseconds})";
            ImGui.Text(timerInfo);
            ImGui.Text($"Frame: {GetFrameByTimer(_currentTime)}");

            InspectorClass.DrawAllFields(this);

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            if (_trackSelected != -1)
            {
                foreach (var timeLineItem in _timeLineItemTypes)
                {
                    if (Fields.Buttons.BlueButton($"Add {AttributesHelper.FormatName(timeLineItem.Name)}") && _trackSelected != -1)
                    {
                        var item = Activator.CreateInstance(timeLineItem);
                        AddANewItem(timeLineItem, item);
                    }
                }
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            if (_selectedSequenceItem != null)
            {
                _selectedSequenceItem.DrawProperties();
                if (Fields.Buttons.RedButton("Delete Item"))
                {
                    Tracks[_trackSelected].Items.Remove(_selectedSequenceItem);
                    _selectedSequenceItem = null;
                }
            }
        }

        public virtual void AddANewItem(Type timeLineItem, object item)
        {
            if (item is TimelineItem timeLineItemInstance)
            {
                timeLineItemInstance.Start = GetFrameByTimer(_currentTime) * _timePerFrame;
                timeLineItemInstance.Duration = _timePerFrame;
                timeLineItemInstance.Name = AttributesHelper.FormatName(timeLineItem.Name);
                Tracks[_trackSelected].Items.Add(timeLineItemInstance);
                SetSelectedItem(timeLineItemInstance);
            }
        }

        public void SetSelectedItem(TimelineItem timelineItem)
        {
            if (_selectedSequenceItem != null)
                _selectedSequenceItem.IsSelected = false;
            timelineItem.IsSelected = true;
            _selectedSequenceItem = timelineItem;
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

        public void HandleTrackSelect(Vector2 position)
        {
            _trackHover = -1;
            if (!ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem)) return;

            var io = ImGui.GetIO();
            var mouseScreen = io.MousePos;

            int trackCount = 0;
            float offsetY = _timelineRuleHight;
            foreach (var timelineItem in Tracks)
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

        public void DrawTracks(ImDrawListPtr drawList, Vector2 position)
        {
            float timeLineWidth = GetPositionXOnTimeLine(_durationInSeconds);

            int trackCount = 0;
            float offsetY = _timelineRuleHight;
            var selectedColor = ImGui.GetColorU32(ImGuiCol.TabActive);
            var hoverColor = ImGui.GetColorU32(ImGuiCol.TabHovered);

            foreach (var timelineItem in Tracks)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * trackCount;
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, TimeLineHight),
                    ImGui.GetColorU32(ImGuiCol.Border)
                );

                var trackColor = trackCount == _trackHover ? hoverColor : ImGui.GetColorU32(ImGuiCol.WindowBg);
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
            foreach (var timeLine in Tracks)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * trackCount;
                foreach (var timeLineItem in timeLine.Items)
                    timeLineItem.Draw(drawList, new Vector2(position.X, yPosition), this);
                trackCount++;
            }
        }

        public void DrawTimeCursor(ImDrawListPtr drawList, Vector2 position)
        {
            var fg = ImGui.GetForegroundDrawList();

            float x = position.X + GetPositionXOnTimeLine(_currentTime);
            float top = position.Y;
            float bottom = position.Y + ImGui.GetWindowHeight();

            fg.AddLine(
                new Vector2(x, top),
                new Vector2(x, bottom),
                ImGui.GetColorU32(Microsoft.Xna.Framework.Color.Yellow.PackedValue),
                2f
            );

            float t = 6f;
            fg.AddTriangleFilled(
                new Vector2(x - t, top),
                new Vector2(x + t, top),
                new Vector2(x, top + t),
                ImGui.GetColorU32(Microsoft.Xna.Framework.Color.Yellow.PackedValue)
            );
        }

        public void Play()
        {
            _currentState = State.PLAYING;
            timer = new Timer();
            timer.Begin();
        }

        public void Stop()
        {
            _currentState = State.STOPPED;
            timer.End();
        }

        [Button]
        public void AddTrack()
        {
            Tracks.Add(new Track());
        }

        public void TimeLineUpdate()
        {
            if (_currentState == State.STOPPED) return;
            timer.End();
            _currentTime += timer.GetTotalSeconds();
            if (_currentTime >= DurationInSeconds)
            {
                _currentTime = DurationInSeconds;
                Stop();
                return;
            }
            timer.Begin();
        }

        public int GetFrameByTimer(float timeInSeconds)
        {
            float timePerFrame = _timePerFrame;
            return (int)(timeInSeconds / timePerFrame);
        }

        public float GetPositionXOnTimeLine(float valueInSeconds) => valueInSeconds * FramePerSecond * StepSize;

        public float GetTimeLineOnPositionX(float positionValue) => positionValue / (FramePerSecond * StepSize);
    }
}