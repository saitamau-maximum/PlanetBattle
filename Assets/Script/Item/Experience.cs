using UnityEngine;

public class Experience : Item
{
    [SerializeField] private int _experienceAmount = 1;

    protected override void Execute(GameObject target)
    {
        // 経験値を獲得する処理
        if (target.TryGetComponent(out CurrencyWallet wallet))
        {
            wallet.AddCurrency(CurrencyData.CurrencyType.Experience, _experienceAmount);
        }
    }
}
