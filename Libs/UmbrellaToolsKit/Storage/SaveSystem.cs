using System;
using System.Collections.Generic;
using UmbrellaToolsKit.Utils;

namespace UmbrellaToolsKit.Storage
{
    public class SaveSystem : IDisposable
    {
        public class SaveData
        {
            public Dictionary<string, string> StringValues = new();
            public Dictionary<string, int> IntValues = new();
            public Dictionary<string, float> FloatValues = new();
        }

        private SaveSettings<SaveData> _integration;
        private static SaveData _saveData;

        public SaveSystem(SaveSettings<SaveData> integration)
        {
            _integration = integration;
            _saveData ??=_integration.SaveIntegration.Get(_integration.FilePath);
        }

        public void SetString(string key, string value) => _saveData.StringValues.AddForce(key, value);

        public void SetInt(string key, int value) => _saveData.IntValues.AddForce(key, value);

        public void SetFloat(string key, float value) => _saveData.FloatValues.AddForce(key, value);

        public string GetString(string key, string defaultValue)
        {
            if (_saveData.StringValues.ContainsKey(key))
                return _saveData.StringValues[key];
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            if (_saveData.IntValues.ContainsKey(key))
                return _saveData.IntValues[key];
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue)
        {
            if (_saveData.FloatValues.ContainsKey(key))
                return _saveData.FloatValues[key];
            return defaultValue;
        }

        public void Save(string filename)
        {
            _integration.SaveIntegration.Set(_saveData);
            _integration.SaveIntegration.Save(filename);
        }

        public void Save() => Save(_integration.FilePath);

        public void Dispose() { }
    }
}
