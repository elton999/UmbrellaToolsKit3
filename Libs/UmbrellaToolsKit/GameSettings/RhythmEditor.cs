using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.EditorEngine.Windows.Feature;
using Framework = Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.EditorEngine.GameSettings
{
    public class EventItem : TimelineItem
    {
        protected override Framework.Color _color => Framework.Color.Green;

        public enum Keys
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
        }

        public Keys Key;

        public override void DrawProperties()
        {
            InspectorClass.DrawAllFields(this);
        }
    }

    public class MusicItem : TimelineItem
    {
        public string MusicPath;
        public float StartMusicAt;
        private Framework.Media.Song _song;

        public override void DrawProperties()
        {
            InspectorClass.DrawAllFields(this);
        }

        public override void Draw(ImDrawListPtr drawList, Vector2 position, TimeLineFeature timeLineSettings)
        {
            base.Draw(drawList, position, timeLineSettings);
        }

        private void SetSong(Framework.Media.Song song) => _song = song;
    }

    public class RhythmTimeLine : TimeLineFeature
    {
        protected override List<Type> _timeLineItemTypes => new List<Type>()
        {
            typeof(MusicItem),
            typeof(RhythmTimeLine),
        };

        public void CallOnUpdateTimeLineData(Action callback)
        {
            callback?.Invoke();
        }

        public override void AddANewItem(Type timeLineItem, object item)
        {
            base.AddANewItem(timeLineItem, item);
        }
    }

    [GameSettingsProperty(nameof(RhythmEditor), "/Content/")]
    public class RhythmEditor : GameSettingsProperty
    {
        public RhythmTimeLine TimeLineFeature = new RhythmTimeLine();
        private EditorMain _editorMain;

        public RhythmEditor()
        {
            TimeLineFeature.CallOnUpdateTimeLineData(UpdateFilesFromTimeLine);
        }

        public void DrawTimeLine(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("TimelineDock", ImGuiWindowFlags.HorizontalScrollbar);
            TimeLineFeature.DrawTimeLine();
            ImGui.End();
        }

        public void DrawProperties(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("PropertiesDock");
            TimeLineFeature.DrawProperties();
            ImGui.End();
        }

        public override void DrawFields(EditorMain editorMain)
        {
            _editorMain ??= editorMain;

            uint idProperties = ImGui.GetID("Properties");
            uint idTimeline = ImGui.GetID("Timeline");

            ImGui.BeginChild("timelineLeft", new Vector2(ImGui.GetWindowWidth() * 0.15f, 0));
            ImGui.DockSpace(idProperties, new Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("timelineRight", new Vector2(ImGui.GetWindowWidth() * 0.85f, 0), false, ImGuiWindowFlags.HorizontalScrollbar);
            ImGui.DockSpace(idTimeline, new Vector2(0, 0));
            ImGui.EndChild();

            DrawTimeLine(idTimeline);
            DrawProperties(idProperties);
            TimeLineFeature.TimeLineUpdate();
        }

        private void UpdateFilesFromTimeLine()
        {
            if (_editorMain == null) return;

            foreach (var tracks in TimeLineFeature.TimeLines)
            {
                foreach (var item in tracks)
                {
                    if (item is MusicItem musicItem)
                    {
                        _
                    }
                }
            }

        }
    }
}