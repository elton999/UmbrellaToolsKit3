using UmbrellaToolsKit.EditorEngine.Attributes;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;
using System;
using UmbrellaToolsKit.EditorEngine.Primitives;


namespace UmbrellaToolsKit.EditorEngine.GameSettings
{
    public abstract class TimelineItem
    {
        public float Start;
        public float Duration;
        public string Name;
        public Vector4 _cachedRect;
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
        private float _durationInSeconds = 2f;
        private float _totalFramesInWindow = 250f;
        private float _framePerSecond = 60f;
        private int _frameStep = 15;
        float _timelineHight = 15f;

        public List<List<TimelineItem>> TimeLines = new List<List<TimelineItem>>() { new List<TimelineItem>(), new List<TimelineItem>() };

        public void Draw(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("Timeline", ImGuiWindowFlags.NoScrollbar);

            var position = ImGui.GetCursorScreenPos();
            var drawList = ImGui.GetWindowDrawList();

            DrawTimeLineRule(drawList, position);
            DrawTimeLines(drawList, position);

            ImGui.End();
        }

        public void DrawTimeLines(ImDrawListPtr drawList, Vector2 position)
        {
            float timeLineWidth = ImGui.GetWindowSize().X;

            int timeLineCount = 0;
            float offsetY = _timelineHight;
            foreach (var timelineItem in TimeLines)
            {
                float yPosition = position.Y + offsetY * timeLineCount;
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition + offsetY),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, _timelineHight),
                    Microsoft.Xna.Framework.Color.White
                );
                Square.Draw(
                    drawList,
                    new Microsoft.Xna.Framework.Vector2(position.X, yPosition + offsetY + 1),
                    new Microsoft.Xna.Framework.Vector2(timeLineWidth, _timelineHight - 2),
                    Microsoft.Xna.Framework.Color.Black
                );

                timeLineCount++;
            }

        }

        public void DrawTimeLineRule(ImDrawListPtr drawList, Vector2 position)
        {
            float timeLineWidth = ImGui.GetWindowSize().X;
            float stepSize = timeLineWidth / _totalFramesInWindow;
            int totalFrames = (int)(_durationInSeconds * _framePerSecond);

            drawList.AddLine(
                new Vector2(position.X, position.Y + _timelineHight),
                new Vector2(position.X + timeLineWidth, position.Y + _timelineHight),
                ImGui.GetColorU32(Vector4.One)
            );

            for (int frameIndex = 0; frameIndex < totalFrames; frameIndex += _frameStep)
            {
                drawList.AddText
                (
                    new Vector2(position.X + stepSize * frameIndex, position.Y),
                    ImGui.GetColorU32(Vector4.One),
                    $"{frameIndex}|"
                );
            }
        }

        public override void DrawFields(EditorMain editorMain)
        {
            uint idTimeline = ImGui.GetID("Timeline");

            ImGui.BeginChild("timelineMain", new Vector2(ImGui.GetWindowWidth()));
            ImGui.DockSpace(idTimeline, new Vector2(0, 0));
            ImGui.EndChild();

            Draw(idTimeline);
        }
    }
}