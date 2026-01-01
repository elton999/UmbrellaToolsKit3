using UmbrellaToolsKit.EditorEngine.Attributes;
using ImGuiNET;
using System.Numerics;
using UmbrellaToolsKit.EditorEngine.Windows.Feature;
using System.Collections.Generic;
using System;
using UmbrellaToolsKit.EditorEngine.Windows;

namespace UmbrellaToolsKit.EditorEngine.GameSettings
{
    [Serializable]
    public class SpriteTimeLine : TimeLineFeature
    {
        protected override List<Type> _timeLineItemTypes => new List<Type>() { typeof(SpriteTimeLineItem) };
    }

    public class SpriteTimeLineItem : TimelineItem
    {
        // todo: fix the sprite serialize logic
        [ShowEditor]
        public Components.Sprite.Sprite CurrentSprite = new Components.Sprite.Sprite(
            string.Empty,
            string.Empty,
            new Microsoft.Xna.Framework.Rectangle()
        );

        public override void DrawProperties()
        {
            InspectorClass.DrawAllFields(this);
        }
    }

    [GameSettingsProperty(nameof(SpriteAnimationGameSettings), "/Content/")]
    public class SpriteAnimationGameSettings : GameSettingsProperty
    {
        public SpriteTimeLine TimeLineFeature = new SpriteTimeLine();

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