using UnityEngine;
using Utility;

public class RandomEnemySpawner : MonoBehaviour
{
    [SerializeField] private CharacterAIController[] _enemyPre;
    [SerializeField] private float _maxCoolTime;
    [SerializeField] private float _minCoolTime;
    [SerializeField] private int _maxEnemyCount;

    [SerializeField] private Transform _target;
    private CountdownTimer _coolTimer = new(0);

    private void Update()
    {
        _coolTimer.Tick();

        if (_coolTimer.IsFinished() && transform.childCount < _maxEnemyCount)
        {
            CharacterAIController enemy = Instantiate(_enemyPre[Random.Range(0, _enemyPre.Length)], transform.position, transform.rotation, transform);
            enemy.Init(_target);

            SetCoolTimer();
        }
    }

    private void SetCoolTimer()
    {
        _coolTimer.Reset(Random.Range(_minCoolTime, _maxCoolTime));
        _coolTimer.Start();
    }
}
