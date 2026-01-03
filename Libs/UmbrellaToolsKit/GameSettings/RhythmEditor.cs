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
        protected override bool _showName => false;

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
        [ShowEditor] public float StartMusicAt;
        private Framework.Media.Song _song;
        private bool _isPlaying = false;

        public MusicItem()
        {
            if (string.IsNullOrEmpty(MusicPath))
                return;
            SetMusicFromPath(MusicPath);
        }

        private void SetMusicFromPath(string path)
        {
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return;

            var fullPath = System.IO.Path.GetFullPath(path);
            var uri = new Uri(fullPath, UriKind.Absolute);

            try
            {
                _song = Framework.Media.Song.FromUri(System.IO.Path.GetFileName(fullPath), uri);
                Duration = (float)(_song?.Duration.TotalSeconds ?? 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load song '{path}': {ex.Message}");
                _song = null;
                MusicPath = string.Empty;
            }
        }

        public override void DrawProperties()
        {
            InspectorClass.DrawAllFields(this);
            if (string.IsNullOrEmpty(MusicPath))
            {
                ImGui.Text("No music loaded");
            }

            if (!string.IsNullOrEmpty(MusicPath))
            {
                ImGui.Text($"Loaded music: {System.IO.Path.GetFileName(MusicPath)}");
            }

            if (Fields.Buttons.BlueButton("Load Music"))
            {
                var openFileDialog = OpenFileDialogue.OpenFileDialog("Import music", "Music", ".ogg");
                if (OpenFileDialogue.SaveFileDialog(openFileDialog))
                {
                    string filePath = openFileDialog.FileName;
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        MusicPath = filePath;
                        SetMusicFromPath(MusicPath);
                    }
                }
            }
        }

        public override void Draw(ImDrawListPtr drawList, Vector2 position, TimeLineFeature timeLineSettings)
        {
            base.Draw(drawList, position, timeLineSettings);

            if (timeLineSettings.CurrentState is TimeLineFeature.State.PLAYING && _song != null && !_isPlaying)
            {
                if (timeLineSettings.CurrentTimeInSeconds >= Start && timeLineSettings.CurrentTimeInSeconds <= Start + Duration)
                {
                    float songOffset = (timeLineSettings.CurrentTimeInSeconds - Start) * 1.0f;
                    var songPosition = TimeSpan.FromSeconds(StartMusicAt + songOffset);
                    Framework.Media.MediaPlayer.Play(_song, songPosition);
                    _isPlaying = true;
                }
            }

            if (timeLineSettings.CurrentState is TimeLineFeature.State.STOPPED && _song != null)
            {
                Framework.Media.MediaPlayer.Stop();
                _isPlaying = false;
            }
        }
    }

    public class RhythmTimeLine : TimeLineFeature
    {
        protected override List<Type> _timeLineItemTypes => new List<Type>()
        {
            typeof(MusicItem),
            typeof(EventItem),
        };

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
    }
}