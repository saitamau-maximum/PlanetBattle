using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [Header("Target")]
    public string targetTag = "Enemy";
    public float maxRange = 100f;

    [Header("Fire")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireInterval = 1.5f;

    [Header("Rotation")]
    public float rotateSpeed = 5f;

    [Header("Angle Offset")]
    [Tooltip("見た目補正用の角度（バリスタは -32.74）")]
    public float angleOffset = -32.74f;

    protected float fireTimer;

    protected virtual void Update()
    {
        Transform target = FindNearestTarget();
        if (target == null) return;

        AimAt(target);
        TryFire(target);
    }

    protected virtual Transform FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        if (targets.Length == 0) return null;

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (GameObject t in targets)
        {
            float dist = Vector2.Distance(transform.position, t.transform.position);
            if (dist < minDist && dist <= maxRange)
            {
                minDist = dist;
                nearest = t.transform;
            }
        }
        return nearest;
    }

    protected virtual void AimAt(Transform target)
    {
        // 敵方向ベクトル
        Vector2 dir = target.position - transform.position;

        // 矢（発射方向）の角度
        float fireAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // バリスタの見た目角度（補正込み）
        float turretAngle = fireAngle + angleOffset;

        Quaternion rot = Quaternion.Euler(0f, 0f, turretAngle);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rot,
            rotateSpeed * Time.deltaTime
        );
    }

    protected virtual void TryFire(Transform target)
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            Fire(target);
            fireTimer = 0f;
        }
    }

    protected virtual void Fire(Transform target)
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
