using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    private Health _health;

    protected void Awake()
    {
        _health = GetComponent<Health>();
    }
    protected void Start()
    {
        if (_health != null)
        {
            _health.OnDied += Die;
        }
    }
    protected void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDied -= Die;
        }
    }
    protected void Die()
    {
        Destroy(gameObject);
    }
}