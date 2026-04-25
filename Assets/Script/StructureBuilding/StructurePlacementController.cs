using UnityEngine;

public class StructurePlacementController : MonoBehaviour
{
    [SerializeField] private StructurePreview _placementPreview;
    [SerializeField] private StructurePlacementValidator _validator;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private BuildingBox _buildingBox;

    private StructureData _structureData;
    private readonly GridSnapper _gridSnapper = new();
    private bool _canPlace = false;

    private void Awake()
    {
        _gridSnapper.GridSetup(_cellSize);
    }

    private void Update()
    {
        if (_structureData == null) return;
        _placementPreview.transform.position = _gridSnapper.GetSnappedPosition(transform.position, _structureData.GridSize);
        _canPlace = _validator.CanPlaceStructure();
        _placementPreview.UpdateState(_canPlace);
    }

    public void SetStructure(StructureData data)
    {
        _structureData = data; ;
        _placementPreview.SetStructure(_structureData);
    }

    public void PlaceStructure()
    {
        //建造物の配置処理
        if (!_canPlace) return;
        BuildingBox buildingBox = Instantiate(_buildingBox, _placementPreview.transform.position, Quaternion.identity);
        buildingBox.Init(_structureData);
    }
}
