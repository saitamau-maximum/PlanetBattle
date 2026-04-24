using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CurrencyData
{
    public enum CurrencyType
    {
        Coin,
        Gem
    }

    public CurrencyType Type;
    public int MaxAmount;
    public int Amount;
}

public class CurrencyWallet : MonoBehaviour
{
    [SerializeField] private List<CurrencyData> _currencies;

    public Action<CurrencyData.CurrencyType, int> OnCurrencyChanged;

    public int GetCurrency(CurrencyData.CurrencyType type)
    {
        CurrencyData data = _currencies.FirstOrDefault(c => c.Type == type);
        if (data != null)
        {
            return data.Amount;
        }
        return 0;
    }

    public void AddCurrency(CurrencyData.CurrencyType type, int amount)
    {
        CurrencyData data = _currencies.FirstOrDefault(c => c.Type == type);
        if (data != null)
        {
            data.Amount += Mathf.Clamp(data.Amount + amount, 0, data.MaxAmount);
            OnCurrencyChanged?.Invoke(type, data.Amount);
        }
    }
}
