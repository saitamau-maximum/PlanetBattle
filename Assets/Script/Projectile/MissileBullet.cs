using UnityEngine;
using Utility;

public class MissileBullet : ExplosiveProjectile
{
    [Header("MissileBullet")]
    [SerializeField] protected float _rotateSpeed = 200f;
    [SerializeField] protected float _homingDelay = 0.5f;

    private Transform target;
    private CountdownTimer timer;

    private enum State { Launch, Homing }
    private State state = State.Launch;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    protected override void Start()
    {
        base.Start();
        timer = new CountdownTimer(_homingDelay);
        timer.Start();
    }

    protected override void Move()
    {
        if (state == State.Launch)
        {
            if (timer.IsFinished())
            {
                state = State.Homing;
            }
        }
        else if (state == State.Homing)
        {
            if (target != null)
            {
                RotateTowardsTarget();
            }
        }

        // 常に前進
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    private void RotateTowardsTarget()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector3 currentDirection = transform.right;
        float currentAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;

        float angle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            _rotateSpeed * Time.deltaTime
        );

        float rotationDelta = Mathf.DeltaAngle(currentAngle, angle);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationDelta) * transform.rotation;
    }
}
