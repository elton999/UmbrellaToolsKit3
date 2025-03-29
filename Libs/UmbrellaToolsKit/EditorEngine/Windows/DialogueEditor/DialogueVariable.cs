using System.Collections.Generic;

namespace UmbrellaToolsKit.EditorEngine.Windows.DialogueEditor
{
    public enum VariableType
    {
        NONE = 0,
        INT = 1,
        FLOAT = 2,
        STRING = 3,
    }

    public class VariableSettings
    {
        public VariableSettings(string name, VariableType type)
        {
            Name = name;
            Type = type;
        }

        public string Name;
        public VariableType Type;
    }

    public class DialogueVariable
    {
        private Dictionary<int, VariableSettings> _variables = new Dictionary<int, VariableSettings>();
        private int lastId = -1;

        public Dictionary<int, VariableSettings> Variables 
        {
            get 
            {
                if (_variables == null)
                    _variables = new();
                return _variables;
            }
        }

        public bool AddVariable(string name, VariableType type)
        {
            foreach (var variable in Variables)
                if (variable.Value.Name == name)
                    return false;
            
            int id = GetLastId() + 1;
            Variables.Add(id, new VariableSettings(name, type));
            return true;
        }

        public VariableType GetVariableType(int id)
        {
            if (!Variables.ContainsKey(id)) return VariableType.NONE;
            return Variables[id].Type;
        }

        public int GetLastId()
        {
            int lastId = -1;
            foreach (var variable in Variables)
                lastId = variable.Key;
            return lastId;
        }

        public string GetName(int id)
        {
            if (!Variables.ContainsKey(id)) return "";

            return Variables[id].Name;
        }
    }
}
