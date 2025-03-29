using System;

namespace UmbrellaToolsKit.Storage
{
    public interface ISaveIntegration<T>
    {
        string Extension { get; }  

        object Get(string filename, Type type = null);
        T Get(string filename);
        void Set(object value);

        void Save(string filename);
    }
}
