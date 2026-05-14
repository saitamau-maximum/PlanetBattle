using System;
using System.Linq;
using UnityEngine;

public class WaveEnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private int _firstIndex = 0;
    [SerializeField] private WaveData _waveData;

    public float ProgressRatio => 1 - (_waveData.EnemyList.Length == 0 ? 0 : Mathf.Clamp01((float)_enemyIndex / _waveData.EnemyList.Length));
    public event Action<float> OnProgressChanged;


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
            OnProgressChanged?.Invoke(ProgressRatio);

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