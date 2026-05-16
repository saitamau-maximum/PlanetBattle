using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
public class PlayerAllyManager : MonoBehaviour
{
    [SerializeField] private AllyData[] _allyEntries;
    [SerializeField] private int _spawnCost;
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private Transform _targetTransform;

    private CurrencyWallet _currencyWallet;
    private AllyData _nextAllyEntry;

    public int SpawnCost => _spawnCost;
    public float ExperienceForNextRatio { get; private set; }
    public int EntryAllyCount { get; private set; }

    public Action<float, int> OnSpawnCapacityChanged;

    private void Awake()
    {
        _currencyWallet = GetComponent<CurrencyWallet>();
        OnExperienceChanged(CurrencyData.CurrencyType.Experience, _currencyWallet.GetCurrencyAmount(CurrencyData.CurrencyType.Experience));
    }

    private void Start()
    {
        _nextAllyEntry = GetRandomAllyEntry();

        _currencyWallet.OnCurrencyChanged += OnExperienceChanged;
    }

    private void OnDestroy()
    {
        _currencyWallet.OnCurrencyChanged -= OnExperienceChanged;
    }

    private void OnExperienceChanged(CurrencyData.CurrencyType type, int amount)
    {
        if (type == CurrencyData.CurrencyType.Experience)
        {
            int experienceForNext = amount % _spawnCost;
            ExperienceForNextRatio = (float)experienceForNext / _spawnCost;
            EntryAllyCount = amount / _spawnCost;
            OnSpawnCapacityChanged?.Invoke(ExperienceForNextRatio, EntryAllyCount);
        }
    }

    public bool TrySpawnAlly()
    {
        if (_currencyWallet.TryConsumeCurrency(CurrencyData.CurrencyType.Experience, _spawnCost))
        {
            GameObject ally = Instantiate(_nextAllyEntry.Prefab, _spawnTransform.position, _spawnTransform.rotation);
            ally.GetComponent<CharacterAIController>().Init(_targetTransform);
            _nextAllyEntry = GetRandomAllyEntry();

            return true;
        }

        return false;
    }

    private AllyData GetRandomAllyEntry()
    {
        return _allyEntries[UnityEngine.Random.Range(0, _allyEntries.Length)];
    }
}
