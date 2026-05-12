using System;
using UnityEngine;

public class WaveEnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private int _firstIndex = 0;
    [SerializeField] private WaveData _waveData;

    private float _timer = 0;
    private int _enemyIndex = 0;

    private void Start()
    {
        _enemyIndex = _firstIndex;
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

            _enemyIndex++;
            Debug.Log(_enemyIndex);
            _timer = 0;
        }
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