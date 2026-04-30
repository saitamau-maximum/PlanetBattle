using UnityEngine;

[RequireComponent(typeof(CurrencyWallet))]
public class PlayerBuildingManager : MonoBehaviour
{
    [SerializeField] private StructureData[] _structures;
    [SerializeField] private StructurePlacementController _structurePlacement;

    private CurrencyWallet _currencyWallet;
    private int _selectedStructureIndex = 0;

    private void Awake()
    {
        _currencyWallet = GetComponent<CurrencyWallet>();
    }

    private void Start()
    {
        SelectStructure(0); //最初の建造物を選択しておく
        ExitBuildingMode();

        _currencyWallet.OnCurrencyChanged += OnCoinAmountChanged;
    }

    private void OnDestroy()
    {
        _currencyWallet.OnCurrencyChanged -= OnCoinAmountChanged;
    }

    public void EnterBuildingMode()
    {
        _structurePlacement.gameObject.SetActive(true);
    }

    public void ExitBuildingMode()
    {
        _structurePlacement.gameObject.SetActive(false);
    }

    private void OnCoinAmountChanged(CurrencyData.CurrencyType type, int amount)
    {
        if (type == CurrencyData.CurrencyType.Coin)
        {
            bool canAfford = amount >= _structures[_selectedStructureIndex].Cost;
            _structurePlacement.SetBuildingAllowed(canAfford);
        }
    }

    public void SelectStructure(int index)
    {
        if (index < 0 || index >= _structures.Length) return;

        _selectedStructureIndex = index;
        _structurePlacement.SetStructure(_structures[_selectedStructureIndex]);

        bool canAfford = _currencyWallet.GetCurrencyAmount(CurrencyData.CurrencyType.Coin) >= _structures[_selectedStructureIndex].Cost;
        _structurePlacement.SetBuildingAllowed(canAfford);
    }

    public void TryPlaceStructure()
    {
        if (_structurePlacement.TryPlaceStructure())
        {
            _currencyWallet.TryConsumeCurrency(CurrencyData.CurrencyType.Coin, _structures[_selectedStructureIndex].Cost);
        }
    }
}