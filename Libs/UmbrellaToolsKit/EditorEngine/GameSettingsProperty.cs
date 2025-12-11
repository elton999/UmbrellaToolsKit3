using System;
using UmbrellaToolsKit.EditorEngine.Windows;
using UmbrellaToolsKit.Storage;
using UmbrellaToolsKit.Storage.Integrations;

namespace UmbrellaToolsKit.EditorEngine
{
    public class GameSettingsProperty
    {
        public static ISaveIntegration<GameSettingsProperty> SaveIntegration = new XmlIntegration<GameSettingsProperty>();

        public static object GetProperty(string pathFile, Type type)
        {
            var timer = new Utils.Timer();

            timer.Begin();
            object property = SaveIntegration.Get(pathFile, type);
            timer.End();

            // Log.Write($"[{nameof(GameSettingsProperty)}] reading: {pathFile}, timer: {timer.GetTotalSeconds()}");
            if (property.GetType() == type) return property;
            return Activator.CreateInstance(type);
        }

        public static GameSettingsProperty GetGameSettingsProperty(string pathFile) => (GameSettingsProperty)GetProperty(pathFile, typeof(GameSettingsProperty));

        public static T GetProperty<T>(string pathFile) where T : GameSettingsProperty
        {
            var property = GetProperty(pathFile, typeof(T));

            if (property is T) return (T)property;
            return (T)Activator.CreateInstance(typeof(T));
        }

        public virtual void DrawFields(EditorMain editorMain) => InspectorClass.DrawAllFields(this);
    }
}
