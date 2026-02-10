using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Move")]
    public float speed = 12f;
    public float lifeTime = 5f;

    [Header("Hit")]
    public bool destroyOnHit = true;

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
        // Enemy に当たったかどうかだけ判断
        if (other.CompareTag("Enemy"))
        {
            // 何もしない（Enemy 側が処理する）

            if (destroyOnHit)
                Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
