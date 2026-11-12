using UnityEngine;

public class Ballista : Structure
{
    [Header("Target")]
    public string targetTag = "Enemy";
    public float maxRange = 100f;

    [Header("Fire")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Rotation")]
    public Transform muzzle;
    public float rotateSpeed = 5f;
    [Tooltip("回転できる最小角度")]
    public float minAngle = -90f;
    [Tooltip("回転できる最大角度")]
    public float maxAngle = 90f;

    [Header("Angle Offset")]
    [Tooltip("見た目補正用の角度（バリスタは -32.74）")]
    public float angleOffset = -32.74f;

    private Transform _currentTarget;
    private bool _isTargetInAngle = false;

    protected override void Update()
    {
        _currentTarget = FindNearestTarget();

        if (_currentTarget == null)
        {
            _isTargetInAngle = false;
            ReturnToDefaultRotation();
        }
        else
        {
            AimAt(_currentTarget);

            if (!_isTargetInAngle)
            {
                ReturnToDefaultRotation();
            }
        }

        base.Update(); // 現在の索敵・照準結果で発射を判定する。
    }

    protected override ExecutionStatus Execute()
    {
        if (_isTargetInAngle && _currentTarget != null)
        {
            Fire(_currentTarget);
            return ExecutionStatus.Success;
        }

        return ExecutionStatus.Failure;
    }

    protected virtual Transform FindNearestTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(firePoint.position, maxRange);
        if (targets.Length == 0) return null;

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (Collider2D t in targets)
        {
            if (!t.CompareTag(targetTag)) continue;
            if (!TryGetFireAngle(t.transform, out _)) continue;
            float dist = Vector2.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = t.transform;
            }
        }
        return nearest;
    }

    protected virtual void AimAt(Transform target)
    {
        _isTargetInAngle = TryGetFireAngle(target, out float fireAngle);
        if (_isTargetInAngle)
        {
            ApplyRotation(fireAngle + angleOffset);
        }
    }

    private bool TryGetFireAngle(Transform target, out float fireAngle)
    {
        Vector3 localDirection = transform.InverseTransformDirection(target.position - transform.position);
        fireAngle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;

        return localDirection.x > 0f && fireAngle >= minAngle && fireAngle <= maxAngle;
    }

    protected virtual void ReturnToDefaultRotation()
    {
        ApplyRotation(angleOffset);
    }

    private void ApplyRotation(float targetZAngle)
    {
        Quaternion targetRot = transform.rotation * Quaternion.Euler(0f, 0f, targetZAngle);
        muzzle.rotation = Quaternion.Lerp(
            muzzle.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }

    protected virtual void Fire(Transform target)
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
