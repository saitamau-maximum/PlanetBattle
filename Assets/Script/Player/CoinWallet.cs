using System;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    public int CoinCount { get; private set; }

    public Action<int> OnCoinCountChanged;

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        CoinCount += amount;
        OnCoinCountChanged?.Invoke(CoinCount);
        Debug.Log($"コインを{amount}枚獲得！ 現在のコイン数: {CoinCount}");
    }
}
