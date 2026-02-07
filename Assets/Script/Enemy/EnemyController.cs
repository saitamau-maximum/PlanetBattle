using UnityEngine;
using BehaviourTrees;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _jumpForwardspeed = 1.2f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private EnvironmentSensor _environmentSensor;

    [SerializeField] private Transform _target;
    [SerializeField] private bool _isChasing;

    private Rigidbody2D _rigidbody;
    private BehaviourTree _behaviourTree;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // ビヘイビアツリーの構築
        _behaviourTree = new BehaviourTree("EnemyBehaviourTree");

        Selector goToTargetSelector = new Selector("goToTargetSSelector");

        //空中にいる場合は何もしない
        Sequence idleSequence = new Sequence("idleSequence");
        idleSequence.AddChild(new Condition("IsGround?", () =>
        {
            return !_environmentSensor.IsGrounded() && _environmentSensor.IsEdgeAhead() && !_environmentSensor.IsGroundBehind();
        }));
        goToTargetSelector.AddChild(idleSequence);

        //ターゲットが自分より低い場合かつ前方に落下できる地形がある場合落下する
        Sequence stepDownSequence = new Sequence("stepDownSequence");
        stepDownSequence.AddChild(new Condition("CanStepDown?", _environmentSensor.CanStepDown));
        stepDownSequence.AddChild(new Condition(
            "IsTargetLower?",
            () => { return GetStepLevelDifferenceToTarget(_target) < 0; })
        );
        stepDownSequence.AddChild(new RepeatWhile("MoveForwardToFall", () =>
        {
            return _environmentSensor.IsGroundBehind();
        }, new Leaf(
            "MoveForward",
           new MoveForwardBehaviour(_speed, _rigidbody)
        )));
        goToTargetSelector.AddChild(stepDownSequence);

        //ターゲットが自分より高い場合かつ一段上に足場がある場合ジャンプする
        Sequence jumpStepUpSequence = new Sequence("jumpStepUpSequence");
        jumpStepUpSequence.AddChild(new Condition("CanJumpStepUp?", _environmentSensor.CanJumpStepUp));
        jumpStepUpSequence.AddChild(new Condition(
            "IsTargetHigher?", () =>
            {
                int stepDifference = GetStepLevelDifferenceToTarget(_target);
                return stepDifference > 0 || (stepDifference == 0 && _environmentSensor.IsEdgeAhead());
            })
        );
        jumpStepUpSequence.AddChild(new Leaf(
            "JumpStepUp",
            new JumpBehaviour(_jumpForce, _rigidbody)
        ));
        jumpStepUpSequence.AddChild(new RepeatWhile("MoveForwardToFall", () =>
        {
            return !(_environmentSensor.IsGrounded() && IsLinearVelocityYStopped());
        }, new Leaf(
            "MoveForward",
           new MoveForwardBehaviour(_speed, _rigidbody)
        )));
        goToTargetSelector.AddChild(jumpStepUpSequence);

        //同じ段にターゲットがいてその方向も向いていない場合向きを変える
        //段の角にいてターゲットの方向を向いていない場合向きを変える
        Sequence changeDirectionSequence = new Sequence("changeDirectionSequence");
        changeDirectionSequence.AddChild(new Condition(
            "IsTargetOnSameLevel?OrIsEdge?", () =>
            {
                return GetStepLevelDifferenceToTarget(_target) == 0 || _environmentSensor.IsEdgeAhead();
            })
        );
        changeDirectionSequence.AddChild(new Condition(
            "IsLookingAtTarget?",
            () => { return !IsLookingAtTarget(_target); })
        );
        changeDirectionSequence.AddChild(new Leaf(
            "ChangeDirection",
            new BehaviourTrees.Action(() => { transform.rotation *= Quaternion.Euler(0, 180, 0); }))
        );
        goToTargetSelector.AddChild(changeDirectionSequence);

        //向こう側にジャンプで渡れる地形がある場合ジャンプする
        Sequence jumpAcrossSequence = new Sequence("jumpAcrossSequence");
        jumpAcrossSequence.AddChild(new Condition("CanJumpAcross?", _environmentSensor.CanJumpAcross));
        jumpAcrossSequence.AddChild(new Leaf(
            "JumpAcross",
            new JumpBehaviour(_jumpForce, _rigidbody)
        ));
        jumpAcrossSequence.AddChild(new RepeatWhile("MoveForwardToJump", () =>
        {
            return !(_environmentSensor.IsGrounded() && IsLinearVelocityYStopped());
        }, new Leaf(
           "MoveForward",
          new MoveForwardBehaviour(_jumpForwardspeed, _rigidbody)
        )));
        goToTargetSelector.AddChild(jumpAcrossSequence);

        //次の分岐点まで進み続ける        
        Selector moveToBranchPointSelector = new Selector("moveToNextBranchPointSelector");
        Sequence moveToBranchPointSequence = new Sequence("moveToBranchPointSequence");
        moveToBranchPointSequence.AddChild(new Condition(
            "IsBranchPoint?",
            () => { return !IsBranchPoint(); })
        );
        moveToBranchPointSequence.AddChild(new RepeatWhile("moveToBranchPoint", () => !IsBranchPoint(),
            new Leaf(
                "MoveForward",
                new MoveForwardBehaviour(_speed, _rigidbody)
            )
        ));
        moveToBranchPointSelector.AddChild(moveToBranchPointSequence);
        Sequence moveToNextBranchPointSequence = new Sequence("moveToNextBranchPointSequence");
        moveToNextBranchPointSequence.AddChild(new RepeatWhile("moveToBranchPoint", () => IsBranchPoint(),
            new Leaf(
                "MoveForward",
                new MoveForwardBehaviour(_speed, _rigidbody)
            )
        ));
        moveToBranchPointSelector.AddChild(moveToNextBranchPointSequence);
        goToTargetSelector.AddChild(moveToBranchPointSelector);

        _behaviourTree.AddChild(new RepeatWhile("chasing", () => _isChasing, goToTargetSelector));
    }

    private bool IsBranchPoint()
    {
        return _environmentSensor.IsEdgeAhead() || _environmentSensor.CanJumpStepUp();
    }

    private int GetStepLevelDifferenceToTarget(Transform target)
    {
        if (Mathf.Abs(target.position.y - transform.position.y) < 0.5f) return 0;
        else if (target.position.y > transform.position.y) return 1;
        else return -1;
    }

    private bool IsLookingAtTarget(Transform target)
    {
        return Math.Sign(_target.position.x - transform.position.x) == Math.Sign(transform.right.x);
    }

    private bool IsLinearVelocityYStopped()
    {
        return _rigidbody.linearVelocityY > -0.001f && _rigidbody.linearVelocityY < 0.001f;
    }

    private void Update()
    {
        _behaviourTree.Process();
    }
}
