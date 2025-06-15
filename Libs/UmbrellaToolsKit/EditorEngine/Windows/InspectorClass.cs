#if !RELEASE
using ImGuiNET;
#endif
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UmbrellaToolsKit.EditorEngine.Attributes;
using UmbrellaToolsKit.Utils;

namespace UmbrellaToolsKit.EditorEngine.Windows
{
    public static class InspectorClass
    {
        public class InspectorField
        {
            public Type Type;
            public string Name;
            public Object Value;
        }

        public static Dictionary<Type, int> TypeDict = new Dictionary<Type, int>()
        {
            {typeof(Vector2), 0},
            {typeof(Vector3), 1},
            {typeof(float), 2},
            {typeof(string), 3},
            {typeof(bool), 4},

        };

        public static void DrawAllFields(object obj)
        {
#if !RELEASE
            if (obj is null) return;
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
                        if (treeNode) SetField(field, obj);
                    }
                }
                if (treeNode) ImGui.Unindent();
            }

            foreach (var field in fieldsWithoutCategories)
                SetField(field, obj);
#endif
        }

#if !RELEASE
        public static void SetField(FieldInfo field, object obj)
        {
            var fieldSettings = new InspectorField() { Name = field.Name, Value = field.GetValue(obj), Type = field.FieldType };
            DrawField(fieldSettings);
            field.SetValue(obj, fieldSettings.Value);
        }
#endif

#if !RELEASE
        public static bool DrawSeparator(string name)
        {
            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();
            bool treeNode = ImGui.TreeNodeEx(name, ImGuiTreeNodeFlags.DefaultOpen);

            return treeNode;
        }

        public static void DrawField(InspectorField obj)
        {
            if (TypeDict.ContainsKey(obj.Type))
            {
                switch (TypeDict[obj.Type])
                {
                    case 0:
                        var vector2 = (Vector2)obj.Value;
                        Fields.Field.DrawVector(obj.Name, ref vector2);
                        obj.Value = vector2;
                        break;
                    case 1:
                        var vector3 = (Vector3)obj.Value;
                        Fields.Field.DrawVector(obj.Name, ref vector3);
                        obj.Value = vector3;
                        break;
                    case 2:
                        var floatValue = (float)obj.Value;
                        Fields.Field.DrawFloat(obj.Name, ref floatValue);
                        obj.Value = floatValue;
                        break;
                    case 3:
                        var stringValue = (string)obj.Value;
                        Fields.Field.DrawString(obj.Name, ref stringValue);
                        obj.Value = stringValue;
                        break;
                    case 4:
                        var boolValue = (bool)obj.Value;
                        Fields.Field.DrawBoolean(obj.Name, ref boolValue);
                        obj.Value = boolValue;
                        break;
                }
            }

            if (obj.Value is IList)
            {
                var list = (IList)obj.Value;
                Fields.Field.DrawList(obj.Name, ref list);
                obj.Value = list;
                return;
            }

            if (obj.Value is Enum)
            {
                Fields.Field.DrawEnum(obj.Name, obj.Type, ref obj.Value);
                return;
            }

            if (obj.Type.HasPropertyAttribute(typeof(SerializableAttribute)))
            {
                DrawAllFields(obj.Value);
                return;
            }
        }
#endif
    }
}
