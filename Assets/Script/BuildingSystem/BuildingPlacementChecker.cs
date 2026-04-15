using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GameObject _placementPreview;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private Vector2 _buildingGridSize;
    [SerializeField] private Color _previewValidColor;
    [SerializeField] private Color _previewErrorColor;
    [SerializeField] private GameObject _buildingPrefab;

    private BuildingPlacementChecker _groundChecker;
    private SpriteRenderer _previewRenderer;
    private Vector2 _initialScale;
    private Vector2 _gridOffset;

    private void Awake()
    {
        _groundChecker = GetComponentInChildren<BuildingPlacementChecker>();
        _previewRenderer = _placementPreview.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _initialScale = _placementPreview.transform.localScale;
        _gridOffset = _cellSize / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 basePos = new Vector2(
            Mathf.FloorToInt(transform.position.x / _cellSize.x) * _cellSize.x,
            Mathf.FloorToInt(transform.position.y / _cellSize.y) * _cellSize.y
        );

        // 建物サイズに応じたオフセット
        Vector2 sizeOffset = new Vector2(
            (_buildingGridSize.x - 1) * _cellSize.x * 0.5f,
            (_buildingGridSize.y - 1) * _cellSize.y * 0.5f
        );

        _placementPreview.transform.position = basePos + _gridOffset + sizeOffset;
        _placementPreview.transform.localScale = new Vector2(
            _initialScale.x * _buildingGridSize.x,
            _initialScale.y * _buildingGridSize.y
        );

        if (_groundChecker.CanPlaceBuilding())
        {
            _previewRenderer.color = _previewValidColor;
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(_buildingPrefab, _placementPreview.transform.position, Quaternion.identity);
            }
        }
        else
        {
            _previewRenderer.color = _previewErrorColor;
        }
    }
}
