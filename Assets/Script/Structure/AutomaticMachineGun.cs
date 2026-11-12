using UnityEngine;
using Utility;

public class AutomaticMachineGun : Structure
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _damageAmount = 5f;
    [SerializeField, Min(0f)] private float _targetRange = 10f;
    [SerializeField, Min(1)] private int _burstShotCount = 5;
    [SerializeField, Min(0f)] private float _burstShotInterval = 0.1f;

    private int _shotsFired;
    private CountdownTimer _shotIntervalTimer;

    protected override void Start()
    {
        base.Start();
        _shotIntervalTimer = new CountdownTimer(_burstShotInterval);
    }

    protected override ExecutionStatus Execute()
    {
        if (_shotsFired == 0)
        {
            if (!HasEnemyInFront()) return ExecutionStatus.Failure;
        }
        else
        {
            _shotIntervalTimer.Tick();
            if (!_shotIntervalTimer.IsFinished()) return ExecutionStatus.Running;
        }

        Projectile projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        projectile.Init(_damageAmount);
        _shotsFired++;

        if (_shotsFired >= _burstShotCount)
        {
            _shotsFired = 0;
            return ExecutionStatus.Success;
        }

        _shotIntervalTimer.Start();
        return ExecutionStatus.Running;
    }

    private bool HasEnemyInFront()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(_firePoint.position, _targetRange);
        foreach (var target in targets)
        {
            if (!target.CompareTag("Enemy")) continue;

            Vector2 direction = target.transform.position - _firePoint.position;
            if (Vector2.Dot(transform.right, direction) > 0f) return true;
        }

        return false;
    }
}
