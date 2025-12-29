using UmbrellaToolsKit.EditorEngine.Attributes;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;
using UmbrellaToolsKit.EditorEngine.Primitives;
using System;

namespace UmbrellaToolsKit.EditorEngine.GameSettings
{
    public abstract class TimelineItem
    {
        public float Start;
        public float Duration;
        public string Name;

        public virtual void Draw(ImDrawListPtr drawList, Vector2 position, SpriteAnimationGameSettings timeLineSettings)
        {
            var timeLinePosition = new Vector2(position.X + timeLineSettings.GetPositionXOnTimeLine(Start), position.Y);
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
        }
    }

    public class TimelineEvent : TimelineItem
    {
        public string EventId;
    }

    public class TimelineSequence : TimelineItem
    {
    }

    [GameSettingsProperty(nameof(SpriteAnimationGameSettings), "/Content/")]
    public class SpriteAnimationGameSettings : GameSettingsProperty
    {
        private TimelineItem _selected;
        private float _currentTime = 0.5f;
        private float _durationInSeconds = 2f;
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

        public void DrawTimeLine(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("TimelineDock", ImGuiWindowFlags.NoScrollbar);

            _timeLinePosition = ImGui.GetCursorScreenPos();
            var drawList = ImGui.GetWindowDrawList();

            DrawTimeLineRule(drawList, _timeLinePosition);
            DrawTimeLines(drawList, _timeLinePosition);
            DrawTimeCursor(drawList, _timeLinePosition);

            ImGui.End();
        }

        public void DrawProperties(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("PropertiesDock");
            foreach (var timeLineItem in _timeLineItemTypes)
            {
                if (Fields.Buttons.BlueButton($"Add {AttributesHelper.FormatName(timeLineItem.Name)}"))
                {

                }
            }
            ImGui.End();
        }

        public void DrawTimeLines(ImDrawListPtr drawList, Vector2 position)
        {
            float timeLineWidth = ImGui.GetWindowSize().X;

            int timeLineCount = 0;
            float offsetY = _timelineRuleHight;
            foreach (var timelineItem in TimeLines)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * timeLineCount;
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, TimeLineHight),
                    Microsoft.Xna.Framework.Color.White
                );
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition + 1),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, TimeLineHight - 2),
                    Microsoft.Xna.Framework.Color.Black
                );

                timeLineCount++;
            }

            timeLineCount = 0;
            foreach (var timeLine in TimeLines)
            {
                float yPosition = position.Y + offsetY + TimeLineHight * timeLineCount;
                foreach (var timeLineItem in timeLine)
                {
                    timeLineItem.Draw(drawList, new Vector2(position.X, yPosition), this);
                }
                timeLineCount++;
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

            ImGui.BeginChild("properties", new Vector2(ImGui.GetWindowWidth() * 0.2f));
            ImGui.DockSpace(idProperties, new Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("timelineMain", new Vector2(ImGui.GetWindowWidth() * 0.8f));
            ImGui.DockSpace(idTimeline, new Vector2(0, 0));
            ImGui.EndChild();

            DrawTimeLine(idTimeline);
            DrawProperties(idProperties);
        }

        public float GetPositionXOnTimeLine(float valueInSeconds)
        {
            return valueInSeconds * FramePerSecond * StepSize;
        }
    }
}