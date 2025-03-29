using System;

namespace UmbrellaToolsKit.EditorEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GameSettingsPropertyAttribute : Attribute
    {
        public string Name;
        public string Path;

        public GameSettingsPropertyAttribute(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
