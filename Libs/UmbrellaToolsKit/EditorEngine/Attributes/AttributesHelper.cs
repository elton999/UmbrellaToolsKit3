using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<Type> GetTypesWithAttribute(Type attributeType)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Assembly projectAssembly = Assembly.GetEntryAssembly();

            List<Type> types = GetTypesWithAttribute(currentAssembly, attributeType).ToList();
            types.AddRange(GetTypesWithAttribute(projectAssembly, attributeType));
            return types;
        }

        public static string FormatName(string name)
        {
            string cleaned = name.Replace("_", "");
            string spaced = Regex.Replace(cleaned, "(?<!^)([A-Z])", " $1");
            return spaced.ToLower().Trim();
        }

        public static object GetConstructorArgumentsValue(Type type, string name)
        {
            var attributesData = type.GetCustomAttributesData();

            foreach (var attr in attributesData)
            {
                var ctorParams = attr.Constructor.GetParameters();
                var ctorArgs = attr.ConstructorArguments;

                for (int argumentsIndex = 0; argumentsIndex < Math.Min(ctorParams.Length, ctorArgs.Count); argumentsIndex++)
                {
                    if (string.Equals(ctorParams[argumentsIndex].Name, name, StringComparison.OrdinalIgnoreCase))
                        return ctorArgs[argumentsIndex].Value;
                }
                foreach (var named in attr.NamedArguments)
                {
                    if (string.Equals(named.MemberName, name, StringComparison.OrdinalIgnoreCase))
                        return named.TypedValue.Value;
                }
            }

            return null;
        }

        public bool TryGetConstructorArgumentsValue(Type type, string name, out object value)
        {
            object tempValue = GetConstructorArgumentsValue(type, name);
            value = null;
            return tempValue != null;
        }
    }
}
