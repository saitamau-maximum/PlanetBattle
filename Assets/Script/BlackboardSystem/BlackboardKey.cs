using System;
using UnityEngine;

namespace BlackboardSystem
{
    public readonly struct BlackboardKey<T> : IEquatable<BlackboardKey<T>>
    {
        public readonly string Name;
        public readonly int HashedKey;

        public BlackboardKey(string name)
        {
            Name = name;
            HashedKey = Animator.StringToHash(name);
        }

        public bool Equals(BlackboardKey<T> other)
        {
            return HashedKey == other.HashedKey;
        }

        public override bool Equals(object obj)
        {
            return obj is BlackboardKey<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashedKey;
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(
            BlackboardKey<T> left,
            BlackboardKey<T> right)
        {
            return left.HashedKey == right.HashedKey;
        }

        public static bool operator !=(
            BlackboardKey<T> left,
            BlackboardKey<T> right)
        {
            return left.HashedKey != right.HashedKey;
        }
    }
}
