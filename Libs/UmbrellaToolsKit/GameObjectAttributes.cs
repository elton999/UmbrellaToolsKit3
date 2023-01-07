using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UmbrellaToolsKit
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class ShowEditorAttribute : Attribute
    {
        public ShowEditorAttribute()
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
        }
    }
}
