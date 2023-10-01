using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public class InspectorClass
    {
        public static void DrawAllFields(object obj)
        {
            var type = obj.GetType();
            var fieldsCategories = new Dictionary<string, List<FieldInfo>>();
            var fieldsWithoutCategories = new List<FieldInfo>();

            foreach (FieldInfo fInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (var attr in fInfo.CustomAttributes)
                {
                    if (attr.AttributeType == typeof(CategoryAttribute))
                    {
                        string categoryName = (string)attr.ConstructorArguments[0].Value;
                        if (!fieldsCategories.ContainsKey(categoryName))
                            fieldsCategories.Add(categoryName, new List<FieldInfo>());

                        fieldsCategories[categoryName].Add(fInfo);

                    }
                    if (attr.AttributeType == typeof(ShowEditorAttribute))
                        fieldsWithoutCategories.Add(fInfo);
                }
            }

            foreach (var category in fieldsCategories)
            {
                bool treeNode = DrawSeparator(category.Key);

                foreach (var field in category.Value)
                {
                    if (fieldsWithoutCategories.Contains(field))
                    {
                        fieldsWithoutCategories.Remove(field);
                        if (treeNode) DrawField(field, obj);
                    }
                }
                if (treeNode) ImGui.Unindent();
            }

            foreach (var field in fieldsWithoutCategories)
                DrawField(field, obj);
        }

        public static bool DrawSeparator(string name)
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            bool treeNode = ImGui.TreeNodeEx(name, ImGuiTreeNodeFlags.DefaultOpen);

            return treeNode;
        }


        public static void DrawField(FieldInfo fInfo, object prop)
        {
            switch (fInfo.FieldType.ToString())
            {
                case "Microsoft.Xna.Framework.Vector2":
                    var vector2 = (Vector2)fInfo.GetValue(prop);
                    Fields.Field.DrawVector(fInfo.Name, ref vector2);
                    fInfo.SetValue(prop, vector2);
                    break;
                case "Microsoft.Xna.Framework.Vector3":
                    var vector3 = (Vector3)fInfo.GetValue(prop);
                    Fields.Field.DrawVector(fInfo.Name, ref vector3);
                    fInfo.SetValue(prop, vector3);
                    break;
                case "System.Single":
                    var floatValue = (float)fInfo.GetValue(prop);
                    Fields.Field.DrawFloat(fInfo.Name, ref floatValue);
                    fInfo.SetValue(prop, floatValue);
                    break;
                case "System.String":
                    var stringValue = (string)fInfo.GetValue(prop);
                    Fields.Field.DrawString(fInfo.Name, ref stringValue);
                    fInfo.SetValue(prop, stringValue);
                    break;
                case "System.Boolean":
                    var boolValue = (bool)fInfo.GetValue(prop);
                    Fields.Field.DrawBoolean(fInfo.Name, ref boolValue);
                    fInfo.SetValue(prop, boolValue);
                    break;
            }
        }
    }
}
