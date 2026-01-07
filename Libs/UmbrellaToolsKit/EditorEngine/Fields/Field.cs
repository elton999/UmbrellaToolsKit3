#if !RELEASE
using ImGuiNET;
#endif
using UmbrellaToolsKit.Utils;
using System;
using System.Collections;
using UmbrellaToolsKit.EditorEngine.Windows;
using System.Numerics;
using UmbrellaToolsKit.EditorEngine.GameSettings;
using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Fields
{
	public class Field
	{
#if !RELEASE
		private const float LabelWidthRatio = 0.32f;
#endif

		public static void DrawFloat(string name, ref float value)
		{
#if !RELEASE
			BeginField(name);
			ImGui.SetNextItemWidth(-1);
			ImGui.InputFloat($"##{name}", ref value);
			EndField();
#endif
		}

		public static void DrawInt(string name, ref int value)
		{
#if !RELEASE
			BeginField(name);
			ImGui.SetNextItemWidth(-1);
			ImGui.InputInt($"##{name}", ref value);
			EndField();
#endif
		}

		public static void DrawBoolean(string name, ref bool value)
		{
#if !RELEASE
			BeginField(name);
			ImGui.Checkbox($"##{name}", ref value);
			EndField();
#endif
		}

		public static void DrawString(string name, ref string value)
		{
#if !RELEASE
			if (string.IsNullOrEmpty(value)) value = string.Empty;
			BeginField(name);
			ImGui.SetNextItemWidth(-1);
			ImGui.InputText($"##{name}", ref value, 255);
			EndField();
#endif
		}

		public static void DrawLongText(string name, ref string value)
		{
#if !RELEASE
			if (string.IsNullOrEmpty(value)) value = string.Empty;

			BeginField(name);
			ImGui.InputTextMultiline(
				$"##{name}",
				ref value,
				1000,
				new Vector2(-1, 120)
			);
			EndField();
#endif
		}

		public static void DrawVector(string name, ref Vector2 vector)
		{
#if !RELEASE
			BeginField(name);

			float spacing = ImGui.GetStyle().ItemSpacing.X + 60f;
			float width = (ImGui.GetContentRegionAvail().X - spacing) * 0.5f;

			ImGui.PushItemWidth(width);
			ImGui.InputFloat("X", ref vector.X);
			ImGui.SameLine();
			ImGui.InputFloat("Y", ref vector.Y);
			ImGui.PopItemWidth();

			EndField();
#endif
		}

		public static void DrawVector(string name, ref Vector3 vector)
		{
#if !RELEASE
			BeginField(name);

			float spacing = ImGui.GetStyle().ItemSpacing.X;
			float width = (ImGui.GetContentRegionAvail().X - spacing * 2) / 3f;

			ImGui.PushItemWidth(width);
			ImGui.InputFloat("X", ref vector.X);
			ImGui.SameLine();
			ImGui.InputFloat("Y", ref vector.Y);
			ImGui.SameLine();
			ImGui.InputFloat("Z", ref vector.Z);
			ImGui.PopItemWidth();

			EndField();
#endif
		}

		public static void DrawEnum(string name, Type type, ref object value)
		{
#if !RELEASE
			var values = Enum.GetNames(type);
			string current = value.ToString();

			BeginField(name);
			if (ImGui.BeginCombo($"##{name}", current))
			{
				foreach (var option in values)
				{
					bool selected = option == current;
					if (ImGui.Selectable(option, selected))
						value = Enum.Parse(type, option);
					if (selected) ImGui.SetItemDefaultFocus();
				}
				ImGui.EndCombo();
			}
			EndField();
#endif
		}

		public static void DrawStringOptions(string name, ref string value, string[] options)
		{
#if !RELEASE
			if (string.IsNullOrEmpty(value)) value = string.Empty;

			BeginField(name);
			if (ImGui.BeginCombo($"##{name}", value))
			{
				foreach (var option in options)
				{
					bool selected = option == value;
					if (ImGui.Selectable(option, selected))
						value = option;
					if (selected) ImGui.SetItemDefaultFocus();
				}
				ImGui.EndCombo();
			}
			EndField();
#endif
		}

		public static void DrawList(string name, ref IList value)
		{
#if !RELEASE
			int count = value.IsValid() ? value.Count : 0;

			if (ImGui.TreeNode($"#{name}", $"{name} ({count})"))
			{
				ImGui.Spacing();
				DrawListFields(name, value);

				ImGui.Spacing();
				ImGui.Separator();
				ImGui.Spacing();

				ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 6);
				if (ImGui.Button("+ Add Item"))
					value.AddNewItem();

				ImGui.TreePop();
			}
#endif
		}

#if !RELEASE
		private static void DrawListFields(string name, IList value)
		{
			int removeIndex = -1;

			for (int i = 0; i < value.Count; i++)
			{
				ImGui.PushID(i);

				if (ImGui.BeginTable("##item_header", 2, ImGuiTableFlags.SizingStretchProp))
				{
					ImGui.TableSetupColumn("Header", ImGuiTableColumnFlags.WidthStretch, 1f);
					ImGui.TableSetupColumn("Action", ImGuiTableColumnFlags.WidthFixed, 32f);
					ImGui.TableNextRow();

					ImGui.TableNextColumn();
					bool open = ImGui.TreeNodeEx(
						$"Item {i}",
						ImGuiTreeNodeFlags.FramePadding
					);

					ImGui.TableNextColumn();
					ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 2);
					if (Buttons.RedButton("X"))
						removeIndex = i;

					ImGui.EndTable();

					if (open)
					{
						ImGui.Indent();

						ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(6, 4));
						ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(6, 3));

						var field = new InspectorClass.InspectorField
						{
							Name = name,
							Value = value[i],
							Type = value[i].GetType()
						};

						InspectorClass.DrawField(field);
						value[i] = field.Value;

						ImGui.PopStyleVar(2);
						ImGui.Unindent();
						ImGui.TreePop();
					}
				}

				ImGui.PopID();
			}

			value.RemoveAtSafe(removeIndex);
		}
#endif

		public static void DrawSprite(string name, Components.Sprite.Sprite spriteRef, out Components.Sprite.Sprite sprite)
		{
#if !RELEASE
			sprite = null;
			var property = GameSettingsProperty.GetProperty<AtlasGameSettings>(@"Content/AtlasGameSettings");
			var spriteList = new List<string>();

			foreach (var atlas in property.Atlas)
			{
				foreach (var spriteBody in atlas.Sprites)
				{
					spriteList.Add(spriteBody.Name);
				}
			}

			string spriteName = spriteRef != null ? spriteRef.Name : string.Empty;
			DrawStringOptions(name, ref spriteName, spriteList.ToArray());

			if (spriteName != string.Empty && spriteRef != null && spriteName != spriteRef.Name)
			{
				if (property.TryGetSpriteByName(spriteName, out var spriteValue))
				{
					sprite = new Components.Sprite.Sprite(spriteValue.Name, spriteValue.Path, spriteValue.GetRectangle());
					Log.Write(sprite.Name);
				}
			}
#endif
		}

#if !RELEASE
		private static void BeginField(string label)
		{
			ImGui.BeginTable($"##{label}", 2, ImGuiTableFlags.SizingStretchProp);
			ImGui.TableSetupColumn("Label", ImGuiTableColumnFlags.WidthStretch, LabelWidthRatio);
			ImGui.TableSetupColumn("Value", ImGuiTableColumnFlags.WidthStretch, 1f);

			ImGui.TableNextRow();
			ImGui.TableNextColumn();
			ImGui.TextUnformatted(label);
			ImGui.TableNextColumn();
		}

		private static void EndField()
		{
			ImGui.EndTable();
		}
#endif
	}
}