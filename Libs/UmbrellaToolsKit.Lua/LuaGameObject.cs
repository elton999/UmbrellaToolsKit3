using MoonSharp.Interpreter;
using System;

namespace UmbrellaToolsKit.Lua
{
    public class LuaGameObject : GameObject
    {
        private string _scriptText;
        private Script _script;

        public void SetScript(string scriptText)
        {
            try
            {
                _scriptText = scriptText;
                _script = new Script();
                _script.DoString(scriptText);
            }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public override void Start()
        {
            base.Start();
            try{ _script.Call("start"); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            try { _script.Call("update", deltaTime); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public override void UpdateData(float deltaTime)
        {
            try { _script.Call("update_data", deltaTime); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
            base.UpdateData(deltaTime);
        }

        public override void Destroy()
        {
            try { _script.Call("on_destroy"); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
            base.Destroy();
        }
    }
}
