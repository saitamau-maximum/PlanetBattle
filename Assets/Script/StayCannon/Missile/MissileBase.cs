using UnityEngine;

public class MissileLauncher2D : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform firePoint;

    [Header("発射設定")]
    public int missileCount = 5;
    public float baseAngle = 0f;      // 基準角度（度）
    public float spreadAngle = 30f;   // バラけ幅

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
        // 既存の発射ロジック
        for (int i = 0; i < missileCount; i++)
        {
            float offset = Random.Range(-spreadAngle, spreadAngle);
            float finalAngle = baseAngle + offset;

            Quaternion rot = Quaternion.Euler(0, 0, finalAngle);

            Instantiate(
                missilePrefab,
                firePoint.position,
                rot
            );
        }
    }
}