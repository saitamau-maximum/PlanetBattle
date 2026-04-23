using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurrencyData
{
    [SerializeField] private int _max = 1000;
    private int _value = 0;
    public int Value
    {
        get { return _value; }
        set { _value = Mathf.Clamp(value, 0, _max); }
    }
}

public class CurrencyWallet : MonoBehaviour
{
    public enum CurrencyType
    {
        Coin,
        Gem
    }

    public Dictionary<CurrencyType, CurrencyData> Currencies { get; private set; }

    public Action<CurrencyType, int> OnCurrencyChanged;

    public void AddCurrency(CurrencyType type, int amount)
    {
        if (Currencies.TryGetValue(type, out CurrencyData data))
        {
            data.Value += amount;
            OnCurrencyChanged?.Invoke(type, data.Value);
        }
    }
}
