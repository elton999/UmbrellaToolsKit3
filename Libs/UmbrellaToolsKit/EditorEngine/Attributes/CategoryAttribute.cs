using System;

namespace UmbrellaToolsKit.EditorEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class CategoryAttribute : Attribute
    {
        public string CategoryName;

        public CategoryAttribute(string categoryName) => CategoryName = categoryName;
    }
}

