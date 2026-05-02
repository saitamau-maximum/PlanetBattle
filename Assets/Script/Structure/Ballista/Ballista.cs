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
        base.Update(); // Structure のタイマー処理（Execute の呼び出し）

        _currentTarget = FindNearestTarget();

        if (_currentTarget == null)
        {
            _isTargetInAngle = false;
            ReturnToDefaultRotation();
            return;
        }

        AimAt(_currentTarget);

        if (!_isTargetInAngle)
        {
            ReturnToDefaultRotation();
        }
    }

    protected override void Execute()
    {
        if (_isTargetInAngle && _currentTarget != null)
        {
            Fire(_currentTarget);
        }
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
        Vector2 dir = target.position - transform.position;
        float rawAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float fireAngle = Mathf.DeltaAngle(0f, rawAngle);

        if (fireAngle >= minAngle && fireAngle <= maxAngle)
        {
            _isTargetInAngle = true;
            ApplyRotation(fireAngle + angleOffset);
        }
        else
        {
            _isTargetInAngle = false;
        }
    }

    protected virtual void ReturnToDefaultRotation()
    {
        ApplyRotation(angleOffset);
    }

    private void ApplyRotation(float targetZAngle)
    {
        Quaternion targetRot = Quaternion.Euler(0f, 0f, targetZAngle);
        muzzle.rotation = Quaternion.Lerp(
            muzzle.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }

    protected virtual void Fire(Transform target)
    {
        if (firePoint != null && projectilePrefab != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
}