using System;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace UmbrellaToolsKit.Storage.Integrations
{
    public class XmlIntegration<T> : ISaveIntegration<T>
    {
        private T _values;

        public string Extension { get => ".xml"; }

        public object Get(string filename, Type type = null)
        {
            T values = (T)Activator.CreateInstance(typeof(T));
            try
            {
                using (XmlReader reader = XmlReader.Create(filename + Extension))
                {
                    values = IntermediateSerializer.Deserialize<T>(reader, filename + Extension);
                    reader.Close();
                }
            }
            catch { };

            return values;
        }

        public T Get(string filename) => (T)Get(filename, typeof(T));

        public void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(filename + Extension, settings))
            {
                IntermediateSerializer.Serialize(writer, _values, null);
                writer.Close();
            }
        }

        public void Set(object value) => _values = (T)value;
    }
}
