using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _lifetime;
    protected Hitbox _hitbox;
    private float _damageAmount;

    private void Awake()
    {
        _hitbox = GetComponent<Hitbox>();
    }

    public void Init(float damage)
    {
        _damageAmount = damage;
    }

    private void Start()
    {
        _hitbox.OnFirstHit += DealDamage;
        Destroy(gameObject, _lifetime);
    }

    private void OnDestroy()
    {
        _hitbox.OnFirstHit -= DealDamage;
    }

    private void DealDamage(Health targetHealth)
    {
        targetHealth.TakeDamage(_damageAmount);
    }
}