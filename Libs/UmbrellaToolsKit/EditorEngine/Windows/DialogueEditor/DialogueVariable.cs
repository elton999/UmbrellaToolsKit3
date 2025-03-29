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

    public class DialogueVariable
    {
        private Dictionary<int, (string, VariableType)> _variables = new Dictionary<int, (string, VariableType)>();

        public bool AddVariable(string name, VariableType type)
        {
            foreach (var variable in _variables.Values)
                if (variable.Item1 == name)
                    return false;

            _variables.Add(GetLastId() + 1, (name, type));
            return true;
        }

        public VariableType GetVariableType(int id)
        {
            if(!_variables.ContainsKey(id)) return VariableType.NONE;
            return _variables[id].Item2;
        }

        public int GetLastId()
        {
            int lastId = 0;
            foreach (var id in _variables.Keys)
                lastId = id;
            return lastId;
        }
    }
}
