using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace UmbrellaToolsKit.Input
{
    public class KeyBoardHandler
    {
        private static KeyBoardHandler _instance;

        private static Dictionary<Keys, Tuple<KeyState, KeyState>> _keysStatus = new Dictionary<Keys, Tuple<KeyState, KeyState>>();
        private static Dictionary<string, List<Keys>> _bindedInputs = new Dictionary<string, List<Keys>>();

        public KeyBoardHandler() => GameManagement.OnGameUpdateData += setInputData;

        internal static KeyBoardHandler GetStatus => _instance ??= new KeyBoardHandler();

        public static bool KeyPressed(Keys key)
        {
            var status = GetStatus;
            foreach (var keyState in _keysStatus)
                if (keyState.Key == key)
                    return keyState.Value.Item1 == KeyState.Down && keyState.Value.Item2 == KeyState.Up;
            return false;
        }

        public static bool KeyPressed(string name)
        {
            if (!_bindedInputs.ContainsKey(name)) return false;
            foreach (var input in _bindedInputs[name])
                if (KeyPressed(input)) return true;
            return false;
        }

        public static bool KeyDown(Keys key)
        {
            var status = GetStatus;
            foreach (var keyState in _keysStatus)
                if (keyState.Key == key)
                    return keyState.Value.Item1 == KeyState.Down;
            return false;
        }

        public static bool KeyDown(string name)
        {
            if (!_bindedInputs.ContainsKey(name)) return false;
            foreach (var input in _bindedInputs[name])
                if (KeyDown(input)) return true;
            return false;
        }

        public static bool KeyUp(Keys key)
        {
            var status = GetStatus;
            foreach (var keyState in _keysStatus)
                if (keyState.Key == key)
                    return keyState.Value.Item1 == KeyState.Up;
            return false;
        }

        public static bool KeyUp(string name)
        {
            if (!_bindedInputs.ContainsKey(name)) return false;
            foreach (var input in _bindedInputs[name])
                if (KeyUp(input)) return true;
            return false;
        }

        public static void AddInput(Keys key)
        {
            var status = GetStatus;

            if (_keysStatus.ContainsKey(key)) return;
            _keysStatus.Add(key, new Tuple<KeyState, KeyState>(KeyState.Up, KeyState.Up));
        }

        public static void AddInput(Keys key, Tuple<KeyState, KeyState> keyStatus)
        {
            var status = GetStatus;

            if (_keysStatus.ContainsKey(key)) return;
            _keysStatus.Add(key, keyStatus);
        }

        public static void AddInput(string name, Keys key)
        {
            if (!_bindedInputs.ContainsKey(name))
                _bindedInputs.Add(name, new List<Keys>());
            _bindedInputs[name].Add(key);
            AddInput(key);
        }

        public static void AddInput(string name, Keys[] keys)
        {
            foreach (var key in keys)
                AddInput(name, key);
        }

        internal void setInputData()
        {
            var newInputStatus = new Dictionary<Keys, Tuple<KeyState, KeyState>>();
            foreach (var input in _keysStatus)
            {
                var currentStatus = Keyboard.GetState()[input.Key];
                var currentTuple = new Tuple<KeyState, KeyState>(currentStatus, input.Value.Item1);

                newInputStatus.Add(input.Key, currentTuple);
            }

            _keysStatus.Clear();
            _keysStatus = newInputStatus;
        }
    }
}