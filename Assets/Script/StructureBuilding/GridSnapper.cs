using UnityEngine;

public class GridSnapper : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;
    private Vector2 _cellSize;
    private Vector2 _gridOffset;
    private Vector2 _gridSize = Vector2.one;

    public void GridSetup(Vector2 cellSize)
    {
        _cellSize = cellSize;
        _gridOffset = _cellSize / 2.0f;
    }

    private void Update()
    {
        SnappedPosition();
    }

    public void SetGridSize(Vector2 size)
    {
        _gridSize = size;
    }

    public void SnappedPosition()
    {
        Vector2 basePos = new Vector2(
            Mathf.FloorToInt(_parentObject.transform.position.x / _cellSize.x) * _cellSize.x,
            Mathf.FloorToInt(_parentObject.transform.position.y / _cellSize.y) * _cellSize.y
        );

        // 建物サイズに応じたオフセット
        Vector2 sizeOffset = new Vector2(
            (_gridSize.x - 1) * _cellSize.x * 0.5f,
            (_gridSize.y - 1) * _cellSize.y * 0.5f
        );

        transform.position = basePos + _gridOffset + sizeOffset;
    }
}