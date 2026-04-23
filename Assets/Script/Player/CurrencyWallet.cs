using System;
using Unity.Mathematics;
using UnityEngine;

public class CurrencyWallet : MonoBehaviour
{
    [SerializeField] private int _maxCoinCount = 200;

    public int CoinCount { get; private set; }
    public int GemCount { get; private set; }

    public Action<int> OnCoinCountChanged;

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        CoinCount = math.clamp(CoinCount + amount, 0, _maxCoinCount);
        OnCoinCountChanged?.Invoke(CoinCount);
        Debug.Log($"コインを{amount}枚獲得！ 現在のコイン数: {CoinCount}");
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;

        GemCount += amount;
        Debug.Log($"ジェムを{amount}個獲得！ 現在のジェム数: {GemCount}");
    }
}
