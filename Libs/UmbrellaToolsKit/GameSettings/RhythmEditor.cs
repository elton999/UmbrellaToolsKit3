using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Eto.Forms;
using ImGuiNET;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.EditorEngine.Fields;
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

        [ShowEditor] public Keys Key;

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
        private WaveformData _wave;

        public void Init()
        {
            if (string.IsNullOrEmpty(MusicPath))
                return;
            SetMusicFromPath(MusicPath);
            Log.Write($"[MusicItem] Initialized music item with path: {MusicPath}");
        }

        public void SetMusicFromPath(string path)
        {
            if (!File.Exists(path))
                return;

            var cachePath = path + ".wavecache";

            if (!File.Exists(cachePath))
            {
                var wave = WaveformGenerator.Generate(path);
                WaveformGenerator.Save(cachePath, wave);
            }

            _wave = WaveformGenerator.Load(cachePath);

            var uri = new Uri(Path.GetFullPath(path), UriKind.Absolute);
            _song = Framework.Media.Song.FromUri(Path.GetFileName(path), uri);
            Duration = (float)_song.Duration.TotalSeconds;
            MusicPath = path;
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
                ImGui.Text($"Loaded music: {Path.GetFileName(MusicPath)}");
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

        private void DrawWaveform(ImDrawListPtr drawList, Vector2 pos, Vector2 size, TimeLineFeature timeline)
        {
            if (_wave == null)
                return;
            float pixelsPerSecond = timeline.FramePerSecond * timeline.StepSize;

            float secondsPerBucket = 1f / _wave.BucketsPerSecond;

            int startBucket = (int)(Start / secondsPerBucket);
            int endBucket = (int)((Start + Duration) / secondsPerBucket);

            for (int i = startBucket; i < endBucket && i < _wave.BucketCount; i++)
            {
                float min = _wave.MinMax[i * 2];
                float max = _wave.MinMax[i * 2 + 1];

                float x = pos.X + (i * secondsPerBucket - Start) * pixelsPerSecond;

                float yMid = pos.Y + size.Y * 0.5f;

                drawList.AddLine(
                    new Vector2(x, yMid - max * size.Y * 0.5f),
                    new Vector2(x, yMid - min * size.Y * 0.5f),
                    ImGui.GetColorU32(new Vector4(0, 1, 0, 1))
                );
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

            var size = new Vector2(
                timeLineSettings.GetPositionXOnTimeLine(Duration),
                timeLineSettings.TimeLineHight - 2f
            );

            DrawWaveform(drawList, position, size, timeLineSettings);
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

    [Serializable]
    public class RhythmTimeLineSettings
    {
        [ShowEditor] public string Name;
        [ShowEditor] public RhythmTimeLine TimeLineFeature = new();
    }

    [GameSettingsProperty(nameof(RhythmEditor), "/Content/")]
    public class RhythmEditor : GameSettingsProperty
    {
        [ShowEditor] public List<RhythmTimeLineSettings> TimeLineFeatureList;

        private int _currentTimeLineIndex = -1;
        private string _newTimeLineName = "New Timeline";
        private bool _isInitialized = false;
        private RhythmTimeLine _currentTimeLine
        {
            get
            {
                if (_currentTimeLineIndex < 0 && TimeLineFeatureList.Count > 0)
                    _currentTimeLineIndex = 0;
                return TimeLineFeatureList[_currentTimeLineIndex].TimeLineFeature;
            }
        }
        private EditorMain _editorMain;

        public void Init()
        {
            if (TimeLineFeatureList == null) return;
            if (_isInitialized) return;
            _isInitialized = true;

            foreach (var timeLineFeature in TimeLineFeatureList)
            {
                foreach (var track in timeLineFeature.TimeLineFeature.Tracks)
                    foreach (var item in track.Items)
                        if (item is MusicItem musicItem)
                            musicItem.Init();
            }
        }

        public void DrawTimeLine(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("TimelineDock", ImGuiWindowFlags.HorizontalScrollbar);
            if (_currentTimeLineIndex != -1)
            {
                _currentTimeLine.DrawTimeLine();
            }
            ImGui.End();
        }

        public void DrawProperties(uint dockId)
        {
            ImGui.SetNextWindowDockID(dockId, ImGuiCond.Once);
            ImGui.Begin("PropertiesDock");
            if (_currentTimeLineIndex != -1)
            {
                _currentTimeLine.DrawProperties();
            }
            ImGui.End();
        }

        private void ExportJson()
        {
            var exportFileDialog = ExportDialogue.SaveFileDialog("Export", "file json", ".json");
            if (ExportDialogue.ShowSaveDialog(exportFileDialog))
            {
                var songJson = new SongEvents.Song();
                var rhythmEditorList = new List<SongEvents.RhythmEditor>();
                string filePath = exportFileDialog.FileName;
                foreach (var music in TimeLineFeatureList)
                {
                    var allEvents = new List<TimelineItem>();
                    var rhythmEditor = new SongEvents.RhythmEditor();
                    var eventList = new List<SongEvents.Event>();
                    foreach (var track in music.TimeLineFeature.Tracks)
                    {
                        allEvents.AddRange(track.Items);
                    }

                    foreach (var item in allEvents.OrderBy(x => x.Start))
                    {
                        if (item is EventItem eventItem)
                        {
                            var songEvent = new SongEvents.Event()
                            {
                                Timer = item.Start,
                                Arrow = eventItem.Key switch
                                {
                                    EventItem.Keys.UP => 0,
                                    EventItem.Keys.DOWN => 1,
                                    EventItem.Keys.LEFT => 2,
                                    EventItem.Keys.RIGHT => 3,
                                    _ => 0,
                                }
                            };

                            eventList.Add(songEvent);
                        }

                        if (item is MusicItem musicItem)
                            rhythmEditor.Song = Path.GetFileName(musicItem.MusicPath);
                    }

                    rhythmEditor.Events = eventList.ToArray();
                    rhythmEditorList.Add(rhythmEditor);
                }

                songJson.RhythmEditor = rhythmEditorList.ToArray();
                SongEvents.Song.ExportJsonSong(songJson, filePath);
            }
        }

        public override void DrawFields(EditorMain editorMain)
        {
            Init();
            _editorMain ??= editorMain;

            uint idProjects = ImGui.GetID("Projects");
            uint idProperties = ImGui.GetID("Properties");
            uint idTimeline = ImGui.GetID("Timeline");

            ImGui.BeginChild("timelineLeftProjects", new Vector2(ImGui.GetWindowWidth() * 0.15f, 0));
            ImGui.DockSpace(idProjects, new Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("timelineLeft", new Vector2(ImGui.GetWindowWidth() * 0.15f, 0));
            ImGui.DockSpace(idProperties, new Vector2(0, 0));
            ImGui.EndChild();
            ImGui.SameLine();

            ImGui.BeginChild("timelineRight", new Vector2(ImGui.GetWindowWidth() * 0.70f, 0), false, ImGuiWindowFlags.HorizontalScrollbar);
            ImGui.DockSpace(idTimeline, new Vector2(0, 0));
            ImGui.EndChild();

            DrawTimeLine(idTimeline);
            DrawProperties(idProperties);
            if (_currentTimeLineIndex != -1)
            {
                _currentTimeLine.TimeLineUpdate();
            }

            ImGui.SetNextWindowDockID(idProjects, ImGuiCond.Once);
            ImGui.Begin("ProjectsDocks", ImGuiWindowFlags.HorizontalScrollbar);
            if (TimeLineFeatureList != null)
            {
                foreach (var rhythmTimeLine in TimeLineFeatureList)
                {
                    if (ImGui.Selectable(rhythmTimeLine.Name, _currentTimeLineIndex == TimeLineFeatureList.IndexOf(rhythmTimeLine), ImGuiSelectableFlags.None, new(0, 30.0f)))
                    {
                        _currentTimeLineIndex = TimeLineFeatureList.IndexOf(rhythmTimeLine);
                    }
                }
            }

            ImGui.Separator();
            Field.DrawString("Timeline Name", ref _newTimeLineName);
            if (ImGui.Button("Add New Timeline"))
            {
                if (TimeLineFeatureList == null)
                    TimeLineFeatureList = new List<RhythmTimeLineSettings>();
                TimeLineFeatureList.Add(new RhythmTimeLineSettings() { Name = _newTimeLineName });
                _currentTimeLineIndex = TimeLineFeatureList.Count - 1;
                _newTimeLineName = "New Timeline";
            }

            if (Fields.Buttons.BlueButton("Export JSON"))
            {
                ExportJson();
            }

            ImGui.Separator();
            ImGui.End();
        }
    }
}