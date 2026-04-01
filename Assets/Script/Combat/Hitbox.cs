using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    private bool _canAttack = false;

    private void OnEnable()
    {
        _canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_canAttack)
        {
            if (other.gameObject.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(_damageAmount);
                _canAttack = false;
            }
        }
    }
}
