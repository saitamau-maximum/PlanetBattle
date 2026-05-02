using UnityEngine;

public class MissileLauncher2D : Structure
{
    public MissileBullet missilePrefab;
    public Transform firePoint;

    [Header("発射設定")]
    public int missileCount = 5;
    public float baseAngle = 0f;      // 基準角度（度）
    public float spreadAngle = 30f;   // バラけ幅
    public float targetRange = 20f;   // ターゲット射程距離

    [Header("連射設定")]
    public float fireInterval = 2.0f; // 何秒ごとに発射するか
    private float timer;              // 内部タイマー用

    void Update()
    {
        // タイマーを加算
        timer += Time.deltaTime;

        // 設定した間隔を超えたら発射
        if (timer >= fireInterval)
        {
            Fire();
            timer = 0f; // タイマーをリセット
        }
    }

    public void Fire()
    {
        // 射程範囲内の敵を検出
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(firePoint.position, targetRange);

        // "Enemy"タグの敵を抽出
        int validEnemyCount = 0;
        Transform[] targets = new Transform[enemiesInRange.Length];

        foreach (var collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                targets[validEnemyCount] = collider.transform;
                validEnemyCount++;
            }
        }

        // 発射するミサイルの数を決定（敵の数 or 設定数のどちらか小さい方）
        int actualMissileCount = Mathf.Min(missileCount, validEnemyCount);

        // ミサイルを発射
        for (int i = 0; i < actualMissileCount; i++)
        {
            float offset = Random.Range(-spreadAngle, spreadAngle);
            float finalAngle = baseAngle + offset;

            Quaternion rot = Quaternion.Euler(0, 0, finalAngle);

            MissileBullet missile = Instantiate(
                missilePrefab,
                firePoint.position,
                rot
            );

            // ターゲットを設定            
            if (missile != null)
            {
                missile.SetTarget(targets[i]);
            }
        }
    }
}