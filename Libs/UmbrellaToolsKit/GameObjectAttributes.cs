using System;

namespace UmbrellaToolsKit
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class ShowEditorAttribute : Attribute
    {
        public ShowEditorAttribute()
        {

        }
    }
}
