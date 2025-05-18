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
        private Dictionary<int, VariableSettings> _variables = new();

        public Dictionary<int, VariableSettings> Variables => _variables ??= new();

        public bool AddVariable(string name, VariableType type)
        {
            if (string.IsNullOrEmpty(name) || type == VariableType.NONE) return false;

            name = name.Trim().Replace(" ", "_");
            foreach (var variable in Variables)
                if (variable.Value.Name.ToLower() == name.ToLower())
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
        

        public int GetIdByName(string name)
        {
            int id = -1;
            foreach (var variable in Variables)
                if (variable.Value.Name.ToLower() == name.ToLower())
                    id = variable.Key;
            return id;
        }

        public string[] GetAllVariablesNames()
        {
            var names = new List<string>();
            foreach (var variable in Variables)
                names.Add(variable.Value.Name);

            return names.ToArray();
        }
    }
}
