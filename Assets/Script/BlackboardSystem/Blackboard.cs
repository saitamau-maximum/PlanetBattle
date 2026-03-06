using UnityEngine;
using System.Collections.Generic;

namespace BlackboardSystem
{
    public class Blackboard
    {
        private Dictionary<int, object> _entries = new();

        public T GetValue<T>(BlackboardKey<T> key)
        {
            if (!_entries.TryGetValue(key.HashedKey, out var entry))
            {
                throw new KeyNotFoundException(
                    $"Blackboard key '{key}' was not found. "
                );
            }

            return (T)entry;
        }


        public void SetValue<T>(BlackboardKey<T> key, T value)
        {
            _entries[key.HashedKey] = value;
        }

        public bool ContainsKey<T>(BlackboardKey<T> key)
        {
            return _entries.ContainsKey(key.HashedKey);
        }

        public void RemoveKey<T>(BlackboardKey<T> key)
        {
            _entries.Remove(key.HashedKey);
        }
    }
}