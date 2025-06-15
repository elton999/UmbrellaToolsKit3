using System;

namespace UmbrellaToolsKit.EditorEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class ShowEditorAttribute : Attribute
    {
        public ShowEditorAttribute()
        {

        }
    }
}
