using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.ImGui.Standard;

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

        public static void DrawFloat(string name, ref float value) => ImGui.InputFloat(name, ref value);

        public static void DrawInt(string name, ref int value) => ImGui.InputInt(name, ref value);
    }
}
