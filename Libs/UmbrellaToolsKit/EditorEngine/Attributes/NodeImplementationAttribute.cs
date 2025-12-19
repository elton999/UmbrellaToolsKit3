using System;

namespace UmbrellaToolsKit.EditorEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class NodeImplementationAttribute : Attribute
    {
        public string Name;

        public NodeImplementationAttribute(string name)
        {
            Name = name;
        }
    }
}
