using System;
using System.Linq;
using UnityEngine;

public class WaveEnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private int _firstIndex = 0;
    [SerializeField] private WaveData _waveData;

    private int _maxEnemyCount = 0;
    private int _killedEnemyCount = 0;
    private float _timer = 0;
    private int _enemyIndex = 0;

    public float ProgressRatio => (float)_killedEnemyCount / _maxEnemyCount;
    public event Action<float> OnProgressChanged;

    private void Start()
    {
        _enemyIndex = _firstIndex;
        _maxEnemyCount = _waveData.EnemyList.Length != 0 ? _waveData.EnemyList.Length : 1;
    }

    private void Update()
    {
        if (_enemyIndex >= _waveData.EnemyList.Length) return;

        _timer += Time.deltaTime;

        WaveData.EnemySpawnInfo spawnInfo = _waveData.EnemyList[_enemyIndex];
        if (spawnInfo.SpawnDelaySecond <= _timer)
        {
            GameObject enemy = Instantiate(spawnInfo.EnemyPrefab, transform.position, transform.rotation, transform);
            enemy.GetComponent<CharacterAIController>().Init(_target);
            enemy.GetComponent<Health>().OnDied += OnEnemyKilled;

            _enemyIndex++;

            _timer = 0;
        }
    }

    private void OnEnemyKilled()
    {
        _killedEnemyCount++;
        OnProgressChanged?.Invoke(ProgressRatio);
    }
    private bool CheckStageClear()
    {
        if (transform.childCount <= 0)
        {
            return true;
        }

        return false;
    }
}