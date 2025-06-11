using MoonSharp.Interpreter;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.EditorEngine;

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
            tag += $"_{nameof(LuaGameObject)}";
            try
            {
                _scriptText = scriptText;
                _script = new Script();
                SetAllVariables();
                SetMethods();
                _dynValue = _script.DoString(scriptText);

                _startLuaFunction = _script.Globals.Get("start");
                _updateLuaFunction = _script.Globals.Get("update");
                _updateDateLuaFunction = _script.Globals.Get("update_data");

            }
            catch (ScriptRuntimeException ex)
            {
                Log.Write($"Error: {ex.DecoratedMessage}");
            }
        }

        public override void Start()
        {
            base.Start();
            if (_startLuaFunction == DynValue.Nil) return;
            try { _script.Call(_startLuaFunction); }
            catch (ScriptRuntimeException ex)
            {
                Log.Write($"Error: {ex.DecoratedMessage}");
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (_updateLuaFunction == DynValue.Nil) return;
            try
            {
                _script.Call(_updateLuaFunction, deltaTime);
                GetAllVariables();
            }
            catch (ScriptRuntimeException ex)
            {
                Log.Write($"Error: {ex.DecoratedMessage}");
            }
        }

        public override void UpdateData(float deltaTime)
        {
            base.UpdateData(deltaTime);
            if (_updateDateLuaFunction == DynValue.Nil) return;
            try
            {
                _script.Call(_updateDateLuaFunction, deltaTime);
                GetAllVariables();
            }
            catch (ScriptRuntimeException ex)
            {
                Log.Write($"Error: {ex.DecoratedMessage}");
            }
        }

        private void SetSprite(string spriteName)
        {
            if (string.IsNullOrEmpty(spriteName))
            {
                Log.Write($"Error: sprite Not Found");
                return;
            }
            Log.Write($"Loading sprite: {spriteName}");
            Sprite = Content.Load<Texture2D>(spriteName);
        }

        private void SetBody(int posX, int posY, int width, int hight) => Body = new Rectangle(posX, posY, width, hight);

        private void GetAllVariables()
        {
            Origin = new Vector2((float)_script.Globals.Get("origin_x").Number, (float)_script.Globals.Get("origin_y").Number);

            Scale = (float)_script.Globals.Get("scale").Number;
            Rotation = (float)_script.Globals.Get("rotation").Number;
            Transparent = (float)_script.Globals.Get("transparent").Number;

            Position = new Vector2((float)_script.Globals.Get("pos_x").Number, (float)_script.Globals.Get("pos_y").Number);

            size = new Point((int)_script.Globals.Get("size_x").Number, (int)_script.Globals.Get("size_y").Number);
        }

        private void SetAllVariables()
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

        private void SetMethods()
        {
            _script.Globals["set_sprite"] = (Action<string>)SetSprite;
            _script.Globals["set_body"] = (Action<int, int, int, int>)SetBody;
        }
    }
}
