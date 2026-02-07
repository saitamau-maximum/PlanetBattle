using System;
using UnityEngine;

namespace BehaviourTrees
{
    /// <summary>
    /// Condition ノード。
    /// 登録された条件式（Func<bool>）を評価し、
    /// true の場合は Success、false の場合は Failure を返す。    
    /// </summary>
    public class Condition : Node
    {
        private readonly Func<bool> _condition;

        public Condition(string name, Func<bool> condition)
            : base(name)
        {
            _condition = condition;
        }

        public override Status Process()
        {
            Debug.Log($"Processing Condition: {Name}");
            return _condition() ? Status.Success : Status.Failure;
        }

        public override void Reset() { }
    }
}