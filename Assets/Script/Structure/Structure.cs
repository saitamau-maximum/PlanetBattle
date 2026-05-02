using UnityEngine;
using Utility;

public abstract class Structure : MonoBehaviour
{
    [Header("連射設定")]
    [SerializeField] private float fireInterval = 2.0f;

    private Health _health;
    private CountdownTimer _cooldownTimer;

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
        _cooldownTimer = new CountdownTimer(fireInterval);
    }

    protected virtual void Update()
    {
        _cooldownTimer.Tick();
        if (_cooldownTimer.IsFinished())
        {
            Execute();
            _cooldownTimer.Start();
        }
    }

    protected abstract void Execute();

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