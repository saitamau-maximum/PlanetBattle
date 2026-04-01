using UnityEngine;
using BehaviourTrees;
using BlackboardSystem;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterAIController : MonoBehaviour
{
    [SerializeField] private CharacterContext _context;
    [SerializeField] private WeaponBase _weapon;
    [SerializeField] private EnvironmentSensor _environmentSensor;
    [SerializeField] private Transform _baseTarget;
    [SerializeField] private BehaviourTreeBuilder _behaviourTreeBuilder;
    [SerializeField] private bool _isChasing;//テスト用    

    private Rigidbody2D _rigidbody;

    private Blackboard _blackboard = new Blackboard();
    private BehaviourTree _behaviourTree;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Transform baseTarget)
    {
        _baseTarget = baseTarget;
    }

    private void Start()
    {
        if (_baseTarget == null)
        {
            Debug.Assert(false, $"{gameObject.name}: BaseTarget is not set.");
            return;
        }

        //ブラックボードの構築
        _blackboard.SetValue(CharacterKeys.SelfTransform, transform);
        _blackboard.SetValue(CharacterKeys.SelfRigidbody2D, _rigidbody);
        _blackboard.SetValue(CharacterKeys.SelfEnvironmentSensor, _environmentSensor);
        _blackboard.SetValue(CharacterKeys.TargetTransform, _baseTarget);
        _blackboard.SetValue(CharacterKeys.BaseTargetTransform, _baseTarget);
        _blackboard.SetValue(CharacterKeys.CharacterContext, _context);
        _blackboard.SetValue(CharacterKeys.Weapon, _weapon);

        // ビヘイビアツリーの構築
        _behaviourTree = _behaviourTreeBuilder.Build(_blackboard);
        _behaviourTree.SetBlackboard(_blackboard);
    }

    private void Update()
    {
        if (_baseTarget == null)
            return;

        if (_isChasing)
        {
            _behaviourTree.Process();
        }
        else
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }
}
