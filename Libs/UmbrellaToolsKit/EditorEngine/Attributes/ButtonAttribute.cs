using System;

namespace UmbrellaToolsKit.EditorEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute() { }
    }
}
