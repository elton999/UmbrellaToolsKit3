using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace UmbrellaToolsKit.EditorEngine.Attributes
{
    public class AttributesHelper
    {
        public static IEnumerable<Type> GetTypesWithAttribute(Assembly assembly, Type attributeType)
        {
            foreach (Type type in assembly.GetTypes())
                if (type.GetCustomAttributes(attributeType, true).Length > 0)
                    yield return type;
        }

        public static string FormatName(string name)
        {
            string cleaned = name.Replace("_", "");
            string spaced = Regex.Replace(cleaned, "(?<!^)([A-Z])", " $1");
            return spaced.ToLower().Trim();
        }
    }
}
