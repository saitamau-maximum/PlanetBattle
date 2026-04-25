using UnityEngine;

public class GridSnapper
{
    private Vector2 _cellSize;
    private Vector2 _gridOffset;

    public void GridSetup(Vector2 cellSize)
    {
        _cellSize = cellSize;
        _gridOffset = _cellSize / 2.0f;
    }

    public Vector2 GetSnappedPosition(Vector2 worldPos, Vector2 gridSize)
    {
        Vector2 basePos = new Vector2(
            Mathf.FloorToInt(worldPos.x / _cellSize.x) * _cellSize.x,
            Mathf.FloorToInt(worldPos.y / _cellSize.y) * _cellSize.y
        );

        // 建物サイズに応じたオフセット
        Vector2 sizeOffset = new Vector2(
            (gridSize.x - 1) * _cellSize.x * 0.5f,
            (gridSize.y - 1) * _cellSize.y * 0.5f
        );

        return basePos + _gridOffset + sizeOffset;
    }
}