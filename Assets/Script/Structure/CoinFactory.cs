using UnityEngine;

public class CoinFactory : Structure
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _spawnForceMax;
    [SerializeField] private float _spawnForceMin;

    protected override void Execute()
    {
        GameObject obj = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
        if (obj.TryGetComponent(out Rigidbody2D rb))
        {
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            float forceMagnitude = Random.Range(_spawnForceMin, _spawnForceMax);
            rb.AddForce(randomDirection * forceMagnitude, ForceMode2D.Impulse);
        }
    }
}