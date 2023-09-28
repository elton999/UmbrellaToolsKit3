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
        private List<Tuple<Keys, Action>> _cheatList = new List<Tuple<Keys, Action>>();

        public override void Update(GameTime gameTime)
        {
            foreach (var cheat in _cheatList)
                if (KeyBoardHandler.KeyPressed(cheat.Item1))
                    Execute(cheat);
        }

        public static void Execute(Tuple<Keys, Action> cheat)
        {
            Log.Write($"Executing cheat {cheat.Item2.Method.Name}");
            cheat.Item2?.Invoke();
        }

        public void AddCheat(Keys key, Action action = null)
        {
            KeyBoardHandler.AddInput(key);
            _cheatList.Add(new Tuple<Keys, Action>(key, action));
        }

        public override void OnDestroy() => _cheatList.Clear();
    }
}