using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathDropSpawner : MonoBehaviour
{
    [SerializeField] private DropTable _dropTable;
    [SerializeField] private float _spawnForceMax = 3f;
    [SerializeField] private float _spawnForceMin = 1f;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }
    private void Start()
    {
        if (_health != null)
        {
            _health.OnDied += SpawnDrops;
        }
    }

    private void SpawnDrops()
    {
        if (_dropTable == null || _dropTable.Entries == null) return;

        foreach (DropEntry entry in _dropTable.Entries)
        {
            if (Random.value > entry.Probability) continue;

            int spawnCount = Random.Range(entry.MinCount, entry.MaxCount + 1);
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject obj = Instantiate(entry.Prefab, transform.position, Quaternion.identity);
                if (obj.TryGetComponent(out Rigidbody2D rb))
                {
                    Vector3 randomDirection = Random.insideUnitSphere.normalized;
                    float forceMagnitude = Random.Range(_spawnForceMin, _spawnForceMax);
                    rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
                }
            }

        }
    }
}
