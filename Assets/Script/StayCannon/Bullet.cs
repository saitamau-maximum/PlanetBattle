using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Move")]
    public float speed = 12f;
    public float lifeTime = 5f;

    [Header("Hit")]
    public bool destroyOnHit = true;

    private bool hasHit = false;   // ★ 多重ヒット防止

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // FirePoint の右方向へ直進
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 既にヒット済みなら何もしない
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;

            // Enemy 側がダメージ処理を担当

            if (destroyOnHit)
                Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}