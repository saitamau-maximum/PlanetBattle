using UnityEngine;

[RequireComponent(typeof(CurrencyWallet))]
public class PlayerBuildingManager : MonoBehaviour
{
    [SerializeField] private StructureData[] _structures;
    [SerializeField] private StructurePlacer _structurePlacer;

    private CurrencyWallet _currencyWallet;

    private void Awake()
    {
        _currencyWallet = GetComponent<CurrencyWallet>();
    }

    private void Start()
    {
        SelectStructure(0); //最初の建造物を選択しておく
        ExitBuildingMode();
    }

    public void EnterBuildingMode()
    {
        _structurePlacer.gameObject.SetActive(true);
    }

    public void ExitBuildingMode()
    {
        _structurePlacer.gameObject.SetActive(false);
    }

    public void SelectStructure(int index)
    {
        if (index < 0 || index >= _structures.Length) return;

        _structurePlacer.SetStructure(_structures[index]);
    }

    public void TryPlaceStructure()
    {
        if (_currencyWallet.TryConsumeCurrency(CurrencyData.CurrencyType.Coin, _structures[0].Cost))
            _structurePlacer.PlaceStructure();
    }
}