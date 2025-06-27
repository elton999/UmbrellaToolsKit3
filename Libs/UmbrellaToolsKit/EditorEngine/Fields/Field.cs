#if !RELEASE
using ImGuiNET;
#endif
using UmbrellaToolsKit.Utils;
using System;
using System.Collections;
using UmbrellaToolsKit.EditorEngine.Windows;
using System.Numerics;

namespace UmbrellaToolsKit.EditorEngine.Fields
{
	public class Field
	{
		public static void DrawVector(string name, ref Vector2 vector)
        {
#if !RELEASE
            const float kWeightName = 0.30f;
            const float kWeightVal = 0.35f;

            if (ImGui.BeginTable($"##vec{name}", 3, ImGuiTableFlags.SizingStretchSame))
            {
                ImGui.TableSetupColumn("##n", ImGuiTableColumnFlags.WidthStretch, kWeightName);
                ImGui.TableSetupColumn("##x", ImGuiTableColumnFlags.WidthStretch, kWeightVal);
                ImGui.TableSetupColumn("##y", ImGuiTableColumnFlags.WidthStretch, kWeightVal);
                ImGui.TableNextRow();

                ImGui.TableNextColumn();
                ImGui.TextUnformatted(name);

                ImGui.TableNextColumn();
                {
                    float inner = ImGui.GetColumnWidth()
                                - ImGui.GetStyle().CellPadding.X * 2;
                    ImGui.SetNextItemWidth(inner);
                    ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(1, 0, 0, 0.5f));
                    ImGui.InputFloat($"##{name}_x", ref vector.X);
                    ImGui.PopStyleColor();
                }

                ImGui.TableNextColumn();
                {
                    float inner = ImGui.GetColumnWidth()
                                - ImGui.GetStyle().CellPadding.X * 2;
                    ImGui.SetNextItemWidth(inner);
                    ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0, 1, 0, 0.5f));
                    ImGui.InputFloat($"##{name}_y", ref vector.Y);
                    ImGui.PopStyleColor();
                }

                ImGui.EndTable();
            }
#endif
        }

		public static void DrawVector(string name, ref Vector3 vector)
		{
#if !RELEASE
            ImGui.InputFloat3(name, ref vector);
#endif
        }

		public static void DrawFloat(string name, ref float value)
		{
#if !RELEASE
			ImGui.InputFloat(name, ref value);
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
			if (String.IsNullOrEmpty(value)) value = string.Empty;
			ImGui.InputText(name, ref value, 255);
#endif
		}

		public static void DrawLongText(string name, ref string value)
		{
#if !RELEASE
			TableFormatBegin(name, 1);
			if (String.IsNullOrEmpty(value)) value = string.Empty;
			ImGui.InputTextMultiline(string.Empty, ref value, 500, (Vector2.One * 500), ImGuiInputTextFlags.EnterReturnsTrue);
			TableFormatEnd();
#endif
		}

		public static void DrawEnum(string name, Type type, ref object value)
		{
#if !RELEASE
			var values = Enum.GetNames(type);
			string valueName = value.ToString();
			DrawStringOptions(name, ref valueName, values);
			value = Enum.Parse(type, valueName, true);
#endif
		}

		public static void DrawStringOptions(string name, ref string value, string[] options)
		{
#if !RELEASE
			if (String.IsNullOrEmpty(value)) value = string.Empty;

			if (ImGui.BeginCombo(name, value, ImGuiComboFlags.HeightLarge | ImGuiComboFlags.HeightLargest))
			{
				for (int optionIndex = 0; optionIndex < options.Length; optionIndex++)
				{
					bool is_selected = options[optionIndex] == value;
					if (ImGui.Selectable(options[optionIndex], is_selected)) value = options[optionIndex];
					if (is_selected) ImGui.SetItemDefaultFocus();
				}
				ImGui.EndCombo();
			}
#endif
		}

		public static void DrawList(string name, ref IList value)
		{
#if !RELEASE
			int listCount = value.IsValid() ? value.Count : 0;

			if (ImGui.TreeNode(name, $"{name} ({listCount})"))
			{
                DrawListFields(name, value);

                if (ImGui.Button("add new item")) value.AddNewItem();
				ImGui.Unindent();
            }
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
