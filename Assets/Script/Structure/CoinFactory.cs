using UnityEngine;

public class CoinFactory : Structure
{
    private CurrencyWallet _currencyWallet;

    public override void Init(GameObject player, int buildCost)
    {
        base.Init(player, buildCost);
        _currencyWallet = player.GetComponent<CurrencyWallet>();
    }

    protected override ExecutionStatus Execute()
    {
        _currencyWallet.AddCurrency(CurrencyData.CurrencyType.Coin, 1);
        return ExecutionStatus.Success;
    }
}
