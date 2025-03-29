using System;
using System.Collections.Generic;
using UmbrellaToolsKit.Input;
using UmbrellaToolsKit.EditorEngine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Utils
{
    public class CheatListener : GameObject
    {
        private static CheatListener _instance;
        private List<Tuple<Keys, Action>> _cheatList = new List<Tuple<Keys, Action>>();

        public override void Start() => _instance ??= this;

        public override void Update(float deltaTime)
        {
            foreach (var cheat in _cheatList)
                if (KeyBoardHandler.KeyPressed(cheat.Item1))
                    Execute(cheat);
        }

        public static void Execute(Tuple<Keys, Action> cheat)
        {
#if DEBUG
            Log.Write($"[CheatListener] Executing cheat {cheat.Item2.Method.Name}");
            cheat.Item2?.Invoke();
#endif
        }

        public static void AddCheat(Keys key, Action action = null)
        {
#if DEBUG
            KeyBoardHandler.AddInput(key);
            _instance._cheatList.Add(new Tuple<Keys, Action>(key, action));
#endif
        }

        public static void RemoveCheat(Keys key)
        {
#if DEBUG
            foreach (var cheat in _instance._cheatList)
            {
                if (cheat.Item1 == key)
                {
                    _instance._cheatList.Remove(cheat);
                    return;
                }
            }
#endif
        }

        public override void OnDestroy() => _cheatList.Clear();

        public override void Dispose()
        {
            _cheatList.Clear();
            _instance = null;
            base.Dispose();
        }
    }
}