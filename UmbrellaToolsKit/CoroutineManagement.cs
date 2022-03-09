using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class CoroutineManagement
    {
        private List<IEnumerator> _coroutines = new List<IEnumerator>();
        private IEnumerator _coroutinesUpdate;
        private GameTime _gameTime;

        public CoroutineManagement()
        {
            _coroutinesUpdate = coroutinesUpdate();
        }

        public void StarCoroutine(IEnumerator coroutine)
        {
            _coroutines.Add(coroutine);
        }

        public void ClearCoroutines()
        {
            _coroutines.Clear();
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _coroutinesUpdate.MoveNext();
        }

        public IEnumerator Wait(float time)
        {
            float timer = time;
            while (timer >= 0)
            {
                timer -= (float)_gameTime.ElapsedGameTime.TotalMilliseconds;
                yield return null;
            }
            yield break;
        }

        private IEnumerator coroutinesUpdate()
        {
            while (true)
            {
                if (_coroutines.Count > 0)
                {
                    ExecuteCoroutine();
                }
                yield return null;
            }
        }

        private void ExecuteCoroutine()
        {
            bool hasCoroutines = _coroutines[0].MoveNext();

            if (!hasCoroutines)
            {
                _coroutines.RemoveAt(0);
                coroutinesUpdate().MoveNext();
            }

            if (hasCoroutines && _coroutines[0].Current != null)
            {
                IEnumerator curoutine = (IEnumerator)_coroutines[0].Current;
                curoutine.MoveNext();
                _coroutines.Insert(0, curoutine);
            }
        }
    }
}
