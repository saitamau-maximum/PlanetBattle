using UnityEngine;

public class DamageArea : MonoBehaviour
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
            other.gameObject.GetComponent<Health>()?.TakeDamage(_damageAmount);
            _canAttack = false;
        }
    }
}
