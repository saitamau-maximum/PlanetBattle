using UnityEngine;

public class StructurePlacementController : MonoBehaviour
{
    [SerializeField] private GameObject _placementPreviewObject;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private BuildingBox _buildingBox;

    private StructurePreview _placementPreview;
    private StructurePlacementValidator _placementValidator;
    private GridSnapper _gridSnapper;
    private StructureEntry _structureEntry;
    private bool _canBuild => _structureEntry != null && _structureEntry.IsAvailable && _placementValidator.CanPlace();

    private void Awake()
    {
        _placementPreview = _placementPreviewObject.GetComponent<StructurePreview>();
        _placementValidator = _placementPreviewObject.GetComponent<StructurePlacementValidator>();
        _gridSnapper = _placementPreviewObject.GetComponent<GridSnapper>();
        _gridSnapper.GridSetup(_cellSize);
    }

    private void Update()
    {
        if (_structureEntry == null) return;

        _placementPreview.UpdateState(_canBuild);
    }

    public void SetStructureEntry(StructureEntry entry)
    {
        _structureEntry = entry;
        _placementPreview.SetStructure(_structureEntry.StructureData);
        _gridSnapper.SetGridSize(_structureEntry.StructureData.GridSize);
    }

    public bool TryPlaceStructure()
    {
        //建造物の配置処理
        if (_canBuild)
        {
            BuildingBox buildingBox = Instantiate(_buildingBox, _placementPreview.transform.position, Quaternion.identity);
            buildingBox.Init(_structureEntry.StructureData);
            return true;
        }

        return false;
    }
}