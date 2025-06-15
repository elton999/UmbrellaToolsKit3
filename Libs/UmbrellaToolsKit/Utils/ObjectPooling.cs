using System;
using System.Collections.Generic;
using UmbrellaToolsKit.Interfaces;

namespace UmbrellaToolsKit.Utils
{
    public class ObjectPooling<T> : IObjectPooling where T : IPoolable
    {
        private Queue<T> _objectsPooling;

        public ObjectPooling(int maxObjects)
        {
            _objectsPooling = new Queue<T>();
            for (int objectCount = 0; objectCount < maxObjects; objectCount++) {
                var item = (T)Activator.CreateInstance(typeof(T));
                _objectsPooling.Enqueue(item);
            }
        }

        public IPoolable GetObject()
        {
            var item = _objectsPooling.Dequeue();
            item.Reset();
            _objectsPooling.Enqueue(item);

            return item;
        }
    }
}
