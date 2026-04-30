using UnityEngine;

public class StructurePlacementController : MonoBehaviour
{
    [SerializeField] private GameObject _placementPreviewObject;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private BuildingBox _buildingBox;

    private StructurePreview _placementPreview;
    private StructurePlacementValidator _validator;
    private GridSnapper _gridSnapper;
    private StructureData _structureData;
    private bool _isPlacementValid = false;
    private bool _isBuildAllowed = true;

    public bool CanBuild => _isBuildAllowed && _isPlacementValid;

    private void Awake()
    {
        _placementPreview = _placementPreviewObject.GetComponent<StructurePreview>();
        _validator = _placementPreviewObject.GetComponent<StructurePlacementValidator>();
        _gridSnapper = _placementPreviewObject.GetComponent<GridSnapper>();
        _gridSnapper.GridSetup(_cellSize);
    }

    private void Update()
    {
        if (_structureData == null) return;

        _isPlacementValid = _validator.CanPlaceStructure();
        _placementPreview.UpdateState(CanBuild);
    }

    public void SetBuildingAllowed(bool allowed)
    {
        _isBuildAllowed = allowed;
    }

    public void SetStructure(StructureData data)
    {
        _structureData = data;
        _placementPreview.SetStructure(_structureData);
        _gridSnapper.SetGridSize(_structureData.GridSize);
    }

    public bool TryPlaceStructure()
    {
        //建造物の配置処理
        if (CanBuild)
        {
            BuildingBox buildingBox = Instantiate(_buildingBox, _placementPreview.transform.position, Quaternion.identity);
            buildingBox.Init(_structureData);
            return true;
        }

        return false;
    }
}