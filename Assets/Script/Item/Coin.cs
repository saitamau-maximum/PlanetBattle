using UnityEngine;

public class Coin : Item
{
    [SerializeField] private int _cost = 1;

    protected override void ApplyTo(GameObject target)
    {
        // コインを獲得する処理
        if (target.TryGetComponent(out CurrencyWallet wallet))
        {
            wallet.AddCoins(_cost);
        }
    }
}
