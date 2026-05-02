using Utility;
using UnityEngine;

public class BuildingBox : MonoBehaviour
{
    private GameObject _structure;
    private Health _health;
    private CountdownTimer _countdownTimer;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void Init(StructureData data)
    {
        _structure = data.Prefab;
        _countdownTimer = new CountdownTimer(data.BuildTime);
        _countdownTimer.Start();

        transform.localScale = new Vector2(
            transform.localScale.x * data.GridSize.x,
            transform.localScale.y * data.GridSize.y
        );
    }

    protected void Start()
    {
        _health.OnDied += Die;
    }

    private void OnDestroy()
    {
        _health.OnDied -= Die;
    }

    private void Update()
    {
        _countdownTimer.Tick();
        if (_countdownTimer.IsFinished())
        {
            Instantiate(_structure, transform.position, Quaternion.identity);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
