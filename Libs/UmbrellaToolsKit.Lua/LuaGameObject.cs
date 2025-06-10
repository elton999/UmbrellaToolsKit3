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
            tag += "_lua";
            try
            {
                _scriptText = scriptText;
                _script = new Script();
                SetAllVariables();
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
            try { 
                _script.Call(_updateLuaFunction, deltaTime);
                GetAllVariables();
            }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public override void UpdateData(float deltaTime)
        {
            base.UpdateData(deltaTime);
            if (_updateDateLuaFunction == DynValue.Nil) return;
            try {
                _script.Call(_updateDateLuaFunction, deltaTime);
                GetAllVariables();
            }
            catch (ScriptRuntimeException ex)
            {
                Console.WriteLine("Error: {0}", ex.DecoratedMessage);
            }
        }

        public void GetAllVariables()
        {
            Origin = new Microsoft.Xna.Framework.Vector2((float)_script.Globals.Get("origin_x").Number, (float)_script.Globals.Get("origin_y").Number);

            Scale = (float)_script.Globals.Get("scale").Number;
            Rotation = (float)_script.Globals.Get("rotation").Number;
            Transparent = (float)_script.Globals.Get("transparent").Number;

            Position = new Microsoft.Xna.Framework.Vector2((float)_script.Globals.Get("pos_x").Number, (float)_script.Globals.Get("pos_y").Number);

            size = new Microsoft.Xna.Framework.Point((int)_script.Globals.Get("size_x").Number, (int)_script.Globals.Get("size_y").Number);
        }

        public void SetAllVariables()
        {
            _script.Globals.Set("pos_x", DynValue.NewNumber(Position.X));
            _script.Globals.Set("pos_y", DynValue.NewNumber(Position.Y));

            _script.Globals.Set("origin_x", DynValue.NewNumber(Origin.X));
            _script.Globals.Set("origin_y", DynValue.NewNumber(Origin.Y));

            _script.Globals.Set("scale", DynValue.NewNumber(Scale));
            _script.Globals.Set("rotation", DynValue.NewNumber(Rotation));
            _script.Globals.Set("transparent", DynValue.NewNumber(Transparent));

            _script.Globals.Set("size_x", DynValue.NewNumber(size.X));
            _script.Globals.Set("size_y", DynValue.NewNumber(size.Y));

        }
    }
}
