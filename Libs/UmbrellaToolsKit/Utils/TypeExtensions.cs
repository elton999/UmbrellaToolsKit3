using System;

namespace UmbrellaToolsKit.Utils
{
    public static class TypeExtensions
    {
        public static bool HasPropertyAttribute(this Type type, Type property)
        {
            return type.GetCustomAttributes(property, true).Length > 0;
        }
    }
}
