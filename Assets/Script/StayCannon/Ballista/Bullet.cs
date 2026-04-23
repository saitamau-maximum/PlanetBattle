using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Move")]
    public float speed = 12f;
    public float lifeTime = 5f;

    [Header("Hit")]
    public float damageAmount;
    public bool destroyOnHit = true;

    private Hitbox[] _hitbox;

    void Awake()
    {
        _hitbox = GetComponentsInChildren<Hitbox>(true);
    }

    public void Init(float damage)
    {
        damageAmount = damage;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
        foreach (var hitbox in _hitbox)
        {
            hitbox.OnFirstHit += OnHit;
        }
    }

    void OnDestroy()
    {
        foreach (var hitbox in _hitbox)
        {
            hitbox.OnFirstHit -= OnHit;
        }
    }

    void Update()
    {
        // FirePoint の右方向へ直進
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnHit(Health targetHealth)
    {
        // ヒットした相手にダメージを与える
        targetHealth.TakeDamage(damageAmount);

        // ヒット後の処理
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}