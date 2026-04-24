using UnityEngine;

public class Gem : Item
{
    [SerializeField] private int _cost = 1;

    protected override void Execute(GameObject target)
    {
        // ジェムを獲得する処理
        if (target.TryGetComponent(out CurrencyWallet wallet))
        {
            wallet.AddCurrency(CurrencyData.CurrencyType.Gem, _cost);
        }
    }
}
