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
    private bool _canPlace = false;

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

        _canPlace = _validator.CanPlaceStructure();
        _placementPreview.UpdateState(_canPlace);
    }

    public void SetStructure(StructureData data)
    {
        _structureData = data; ;
        _placementPreview.SetStructure(_structureData);
        _gridSnapper.SetGridSize(_structureData.GridSize);
    }

    public void PlaceStructure()
    {
        //建造物の配置処理
        if (!_canPlace) return;
        BuildingBox buildingBox = Instantiate(_buildingBox, _placementPreview.transform.position, Quaternion.identity);
        buildingBox.Init(_structureData);
    }
}
