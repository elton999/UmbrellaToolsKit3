using System;

namespace UmbrellaToolsKit.Storage
{
    public struct SaveSettings<T>
    {
        public ISaveIntegration<T> SaveIntegration;
        public string FilePath;
    }
}
