using UnityEngine;
using BehaviourTrees;
using BlackboardSystem;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _jumpForwardspeed = 1.2f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _attackRange;
    [SerializeField] private WeaponBase _weapon;
    [SerializeField] private EnvironmentSensor _environmentSensor;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _isChasing;//テスト用

    private Rigidbody2D _rigidbody;

    private Blackboard _blackboard = new Blackboard();
    private BehaviourTree _behaviourTree;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        //ブラックボードの構築
        _blackboard.SetValue(CharacterKeys.SelfTransform, transform);
        _blackboard.SetValue(CharacterKeys.SelfRigidbody2D, _rigidbody);
        _blackboard.SetValue(CharacterKeys.SelfEnvironmentSensor, _environmentSensor);
        _blackboard.SetValue(CharacterKeys.TargetTransform, _target);
        _blackboard.SetValue(CharacterKeys.AttackRange, _attackRange);
        _blackboard.SetValue(CharacterKeys.JumpForce, _jumpForce);

        // ビヘイビアツリーの構築
        _behaviourTree = CreateBehaviourTree();
        _behaviourTree.SetBlackboard(_blackboard);
    }

    public void Init(Transform target)
    {
        _blackboard.SetValue(CharacterKeys.TargetTransform, target);
    }

    private BehaviourTree CreateBehaviourTree()
    {
        BehaviourTree behaviourTree = new BehaviourTree();

        Selector rootSelector = new Selector("rootSelector",
            new Sequence("attackSequence",
                new IsTargetInAttackRange(),
                new StopVelocityX(),
                new BehaviourTrees.WaitForSeconds(0.3f),
                new UseWeapon(_weapon),
                new Sequence("changeDirectionSequence",
                    new ShouldChangeDirection(),
                    new ChangeDirection()
                )
            ),
            new Selector("goToTargetSelector",
                new Sequence("InAir",
                    new IsInAir(),
                    new StopVelocityX()
                ),
                new Sequence("changeDirectionSequence",
                    new ShouldChangeDirection(),
                    new ChangeDirection()
                ),
                new Sequence("jumpAcrossSequence",
                    new ShouldAcross(),
                    new Jump(),
                    new MoveForward(_jumpForwardspeed)
                ),
                new Sequence("stepDownSequence",
                    new ShouldStepDown(),
                    new RepeatWhile("", () => { return _environmentSensor.HasGroundBehind(); }, new MoveForward(_speed)),
                    new StopVelocityX()
                ),
                new Sequence("stepUpSequence",
                    new ShouldStepUp(),
                    new Jump(),
                    new MoveForward(_jumpForwardspeed)
                ),
                new Sequence("moveForwardnSequence",
                    new MoveForward(_speed)
                )
            )
        );

        behaviourTree.AddChild(new RepeatWhile("chasing", () => _isChasing, rootSelector));
        return behaviourTree;
    }

    private void Update()
    {
        _behaviourTree.Process();
    }
}
