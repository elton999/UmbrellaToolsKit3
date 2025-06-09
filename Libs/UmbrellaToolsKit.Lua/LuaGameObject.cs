using MoonSharp.Interpreter;
using System;

namespace UmbrellaToolsKit.Lua
{
    public class LuaGameObject : GameObject
    {
        private string _scriptText;
        private Script _script;

        DynValue _dynValue;
        DynValue _startLuaFunction;
        DynValue _updateLuaFunction;
        DynValue _updateDateLuaFunction;

        public void SetScript(string scriptText)
        {
            try
            {
                _scriptText = scriptText;
                _script = new Script();
                _dynValue = _script.DoString(scriptText);

                _startLuaFunction = _script.Globals.Get("start");
                _updateLuaFunction = _script.Globals.Get("update");
                _updateDateLuaFunction = _script.Globals.Get("update_data");
            }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
            SetAllVariables();
        }

        public override void Start()
        {
            base.Start();
            if (_startLuaFunction == DynValue.Nil) return;
            try { _script.Call(_startLuaFunction); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (_updateLuaFunction == DynValue.Nil) return;
            try { _script.Call(_updateLuaFunction, deltaTime); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public override void UpdateData(float deltaTime)
        {
            base.UpdateData(deltaTime);
            if (_updateDateLuaFunction == DynValue.Nil) return;
            try { _script.Call(_updateDateLuaFunction, deltaTime); }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public void SetAllVariables()
        {

        }
    }
}
