using System;

namespace UmbrellaToolsKit.EditorEngine
{
    public class Log
    {
        public static event Action<string> OnLog;

        public static void Write(string value)
        {
            OnLog?.Invoke(value + "\n");
        }
    }
}
