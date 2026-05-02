using UnityEngine;

public class Ballista : Structure
{
    [Header("Target")]
    public string targetTag = "Enemy";
    public float maxRange = 100f;

    [Header("Fire")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireInterval = 1.5f;

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

    protected float fireTimer;
    protected bool isTargetInAngle = false; // ターゲットが射程角度内にいるか

    protected virtual void Update()
    {
        Transform target = FindNearestTarget();

        // 1. ターゲットがいない場合は初期位置に戻して終了
        if (target == null)
        {
            isTargetInAngle = false;
            ReturnToDefaultRotation();
            return;
        }

        // 2. ターゲットの方向を計算・判定
        AimAt(target);

        // 3. 角度内にいる場合のみ発射を試みる
        if (isTargetInAngle)
        {
            TryFire(target);
        }
        else
        {
            // 角度外に逃げられたら初期位置に戻る
            ReturnToDefaultRotation();
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
            float dist = Vector2.Distance(transform.position, t.transform.position);
            if (dist < minDist && t.CompareTag(targetTag))
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

        // 敵の方向（純粋な角度）を計算
        float rawAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float fireAngle = Mathf.DeltaAngle(0f, rawAngle);

        // 設定した角度の範囲内かどうかをチェック
        if (fireAngle >= minAngle && fireAngle <= maxAngle)
        {
            isTargetInAngle = true;

            // 範囲内の場合はその方向を向く（補正込み）
            float finalAngle = fireAngle + angleOffset;
            ApplyRotation(finalAngle);
        }
        else
        {
            isTargetInAngle = false;
        }
    }

    // 初期位置（0度 + オフセット）に戻る
    protected virtual void ReturnToDefaultRotation()
    {
        ApplyRotation(0f + angleOffset);
    }

    // 共通の回転処理
    private void ApplyRotation(float targetZAngle)
    {
        Quaternion targetRot = Quaternion.Euler(0f, 0f, targetZAngle);
        muzzle.transform.rotation = Quaternion.Lerp(
            muzzle.transform.rotation,
            targetRot,
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
        if (firePoint != null && projectilePrefab != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }
}