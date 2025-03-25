using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace UmbrellaToolsKit.Storage.Integrations
{
    public class BinaryIntegration<T> : ISaveIntegration<T>
    {
        private object _values;

        public string Extension { get => ".bin"; }

        public object Get(string filename, Type type = null)
        {
            T values = (T)Activator.CreateInstance(typeof(T));
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(new FileStream(filename + Extension, FileMode.Open)))
                {
                    string base64String = binaryReader.ReadString();
                    byte[] bytes = Convert.FromBase64String(base64String);
                    string jsonString = Encoding.UTF8.GetString(bytes);
                    type ??= typeof(T);
                    values = (T)JsonConvert.DeserializeObject(jsonString, type);
                    Console.WriteLine(JsonConvert.SerializeObject(values));
                    binaryReader.Close();
                }

            }
            catch { };

            return values;
        }

        public T Get(string filename) => (T)Get(filename, typeof(T));

        public void Save(string filename)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filename + Extension, FileMode.Create)))
            {
                string jsonString = JsonConvert.SerializeObject(_values);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

                binaryWriter.Write(Convert.ToBase64String(bytes));
                binaryWriter.Close();
            }
        }

        public void Set(object value) => _values = value;
    }
}
