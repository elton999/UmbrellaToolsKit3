using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard;
using MonoGame.ImGui.Standard.Extensions;

namespace UmbrellaToolsKit.EditorEngine.Fields
{
    public class Field
    {
        public static void DrawVector(string name, ref Vector2 vector)
        {
            if (ImGui.BeginTable($"##{name}", 3))
            {
                ImGui.TableNextColumn();
                ImGui.TextUnformatted(name);
                ImGui.TableNextColumn();
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(1, 0, 0, 0.5f));
                ImGui.InputFloat("x", ref vector.X);
                ImGui.PopStyleColor();
                ImGui.TableNextColumn();
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0, 1, 0, 0.5f));
                ImGui.InputFloat("y", ref vector.Y);
                ImGui.PopStyleColor();
                ImGui.EndTable();
            }
        }

        public static void DrawVector(string name, ref Vector3 vector)
        {
            if (ImGui.BeginTable($"##{name}", 4))
            {
                ImGui.TableNextColumn();
                ImGui.TextUnformatted(name);
                ImGui.TableNextColumn();
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(1, 0, 0, 0.5f));
                ImGui.InputFloat("x", ref vector.X);
                ImGui.PopStyleColor();
                ImGui.TableNextColumn();
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0, 1, 0, 0.5f));
                ImGui.InputFloat("y", ref vector.Y);
                ImGui.PopStyleColor();
                ImGui.TableNextColumn();
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(0, 0, 1, 0.5f));
                ImGui.InputFloat("z", ref vector.Z);
                ImGui.PopStyleColor();
                ImGui.EndTable();
            }
        }

        public static void DrawFloat(string name, ref float value)
        {
            TableFormatBegin(name);
            ImGui.InputFloat("", ref value);
            TableFormatEnd();
        }

        public static void DrawInt(string name, ref int value) => ImGui.InputInt(name, ref value);

        public static void DrawString(string name, ref string value)
        {
            TableFormatBegin(name);
            ImGui.InputText("", ref value, 255);
            TableFormatEnd();
        }

        public static void DrawLongText(string name, ref string value)
        {
            TableFormatBegin(name);
            ImGui.InputTextMultiline("", ref value, 500, Vector2.Zero.ToNumericVector2());
            TableFormatEnd();
        }

        public static void DrawBoolean(string name, ref bool value) => ImGui.Checkbox(name, ref value);

        public static void TableFormatBegin(string name)
        {
            ImGui.BeginTable($"##{name}", 2);
            ImGui.TableNextColumn();
            ImGui.TextUnformatted(name);
            ImGui.TableNextColumn();
        }

        public static void TableFormatEnd() => ImGui.EndTable();
    }
}
