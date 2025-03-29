using System.Collections.Generic;
using System.Collections;
using System;

namespace UmbrellaToolsKit.Utils
{
    public static class CollectionsExtensions
    {
        public static void AddForce<T>(this Dictionary<string, T> dictionary, string key, T value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }
            dictionary.Add(key, value);
        }

        public static void AddNewItem(this IList list)
        {
            Type type = list.GetType().GetGenericArguments()[0];
            object value;
            if (type == typeof(string)) value = String.Empty;
            else value = System.Activator.CreateInstance(type);
            list.Add(value);
        }

        public static void RemoveAtSafe(this IList list, int index)
        {
            if (index < 0 || index >= list.Count) return;
            list.RemoveAt(index);
        }

        public static bool IsValid(this IList list)
        {
            return list is not null;
        }
    }
}
