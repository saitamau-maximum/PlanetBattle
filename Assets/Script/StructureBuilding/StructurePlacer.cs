using UnityEngine;

/**
 * 建造物を配置するクラス
 * 建造物を配置する場所を設定し、建造物を生成する
 */

public class StructurePlacer : MonoBehaviour
{
    [SerializeField] private GameObject _placementPreview;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private Vector2 _structureGridSize;
    [SerializeField] private GameObject _structurePrefab;

    private StructurePlacementValidator _validator;
    private Vector2 _gridOffset;

    private void Awake()
    {
        _validator = GetComponentInChildren<StructurePlacementValidator>();
    }

    private void Start()
    {
        _gridOffset = _cellSize / 2.0f;
    }

    private void Update()
    {
        Vector2 basePos = new Vector2(
            Mathf.FloorToInt(transform.position.x / _cellSize.x) * _cellSize.x,
            Mathf.FloorToInt(transform.position.y / _cellSize.y) * _cellSize.y
        );

        // 建物サイズに応じたオフセット
        Vector2 sizeOffset = new Vector2(
            (_structureGridSize.x - 1) * _cellSize.x * 0.5f,
            (_structureGridSize.y - 1) * _cellSize.y * 0.5f
        );

        _placementPreview.transform.position = basePos + _gridOffset + sizeOffset;
    }

    public void SetStructure(StructureData data)
    {
        _structurePrefab = data.Prefab;
        _structureGridSize = data.GridSize;
        _validator.SetStructureSize(_structureGridSize);
    }

    public void PlaceStructure()
    {
        //建造物の配置処理
        if (!_validator.CanPlaceStructure()) return;
        Instantiate(_structurePrefab, _placementPreview.transform.position, Quaternion.identity);
    }
}
