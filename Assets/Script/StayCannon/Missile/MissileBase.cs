using UnityEngine;

public class MissileLauncher2D : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform firePoint;

    [Header("発射設定")]
    public int missileCount = 5;
    public float baseAngle = 0f;      // 基準角度（度）
    public float spreadAngle = 30f;   // バラけ幅

    public void Fire()
    {
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