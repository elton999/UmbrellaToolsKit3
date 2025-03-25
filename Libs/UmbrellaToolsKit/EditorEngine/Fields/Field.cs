#if !RELEASE
using ImGuiNET;
using MonoGame.ImGui.Extensions;
#endif
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Utils;
using System;
using System.Collections;
using UmbrellaToolsKit.EditorEngine.Windows;

namespace UmbrellaToolsKit.EditorEngine.Fields
{
	public class Field
	{
		public static void DrawVector(string name, ref Vector2 vector)
		{
#if !RELEASE
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
#endif
		}

		public static void DrawVector(string name, ref Vector3 vector)
		{
#if !RELEASE
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
#endif
		}

		public static void DrawFloat(string name, ref float value)
		{
#if !RELEASE
			TableFormatBegin(name);
			ImGui.InputFloat(string.Empty, ref value);
			TableFormatEnd();
#endif
		}

		public static void DrawInt(string name, ref int value)
		{
#if !RELEASE
			ImGui.InputInt(name, ref value);
#endif
		}
		public static void DrawString(string name, ref string value)
		{
#if !RELEASE
			TableFormatBegin(name);
			if (String.IsNullOrEmpty(value)) value = string.Empty;
			ImGui.InputText(string.Empty, ref value, 255);
			TableFormatEnd();
#endif
		}

		public static void DrawLongText(string name, ref string value)
		{
#if !RELEASE
			TableFormatBegin(name, 1);
			if (String.IsNullOrEmpty(value)) value = string.Empty;
			ImGui.InputTextMultiline(string.Empty, ref value, 500, (Vector2.One * 500).ToNumericVector2(), ImGuiInputTextFlags.EnterReturnsTrue);
			TableFormatEnd();
#endif
		}

		public static void DrawStringOptions(string name, ref string value, string[] options)
		{
#if !RELEASE
			TableFormatBegin(name);
			if (String.IsNullOrEmpty(value)) value = string.Empty;

			if (ImGui.BeginCombo(string.Empty, value, ImGuiComboFlags.HeightLarge | ImGuiComboFlags.HeightLargest))
			{
				for (int optionIndex = 0; optionIndex < options.Length; optionIndex++)
				{
					bool is_selected = options[optionIndex] == value;
					if (ImGui.Selectable(options[optionIndex], is_selected)) value = options[optionIndex];
					if (is_selected) ImGui.SetItemDefaultFocus();
				}
				ImGui.EndCombo();
			}
			TableFormatEnd();
#endif
		}

		public static void DrawList(string name, ref IList value)
		{
#if !RELEASE
			int listCount = value.IsValid() ? value.Count : 0;

			if (!ImGui.TreeNode(name, $"{name} ({listCount})")) return;

			DrawListFields(name, value);

			if (ImGui.Button("add new item")) value.AddNewItem();
#endif
		}

#if !RELEASE
		private static void DrawListFields(string name, IList value)
		{
			int indexToRemove = -1;
			for (int itemIndex = 0; itemIndex < value.Count; itemIndex++)
			{
				var item = value[itemIndex];
				var fieldSettings = new InspectorClass.InspectorField()
				{
					Name = name,
					Value = item,
					Type = item.GetType()
				};

				TableFormatBeginCustom($"#{name}");
				InspectorClass.DrawField(fieldSettings);
				ImGui.TableNextColumn();
				if (Buttons.RedButton("Delete")) indexToRemove = itemIndex;
				TableFormatEnd();

				value[itemIndex] = fieldSettings.Value;
			}

			value.RemoveAtSafe(indexToRemove);
		}
#endif

#if !RELEASE
		public static void DrawBoolean(string name, ref bool value) => ImGui.Checkbox(name, ref value);
#endif

		public static void TableFormatBegin(string name, int columns = 2)
		{
#if !RELEASE
			ImGui.BeginTable($"##{name}", columns);
			ImGui.TableNextColumn();
			ImGui.TextUnformatted(name);
			ImGui.TableNextColumn();
#endif
		}

		public static void TableFormatBeginCustom(string name, int columns = 2)
		{
#if !RELEASE
			ImGui.BeginTable($"##{name}", columns);
			ImGui.TableNextColumn();
#endif
		}

		public static void TableFormatEnd()
		{
#if !RELEASE
			ImGui.EndTable();
#endif
		}
	}
}
