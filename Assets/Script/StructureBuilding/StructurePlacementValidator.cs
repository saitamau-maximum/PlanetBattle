using UnityEngine;

/**
 * 建造物の配置が可能かどうか判断するクラス
 * 建造物のプレビューを表示し、配置できるか判定する
 */

[RequireComponent(typeof(SpriteRenderer))]
public class StructurePlacementValidator : MonoBehaviour
{
    [SerializeField] private Color _previewValidColor;
    [SerializeField] private Color _previewErrorColor;
    [SerializeField] private Transform[] _groundCheckPoints;
    [SerializeField] private float _raycastDistance = 0.3f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _overlappingCount = 0;

    private SpriteRenderer _previewRenderer;
    private Vector2 _initialScale;

    private void Awake()
    {
        _previewRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;
    }

    private void Update()
    {
        if (CanPlaceStructure())
        {
            _previewRenderer.color = _previewValidColor;
        }
        else
        {
            _previewRenderer.color = _previewErrorColor;
        }
    }

    public void SetStructureSize(Vector2 structureGridSize)
    {
        transform.localScale = new Vector2(
           _initialScale.x * structureGridSize.x,
           _initialScale.y * structureGridSize.y
       );
    }

    public bool CanPlaceStructure()
    {
        if (_overlappingCount > 0 || !IsGrounded()) return false;

        return true;
    }

    private bool IsGrounded()
    {
        foreach (var point in _groundCheckPoints)
        {
            if (!HasHit(point.position, Vector2.down, _raycastDistance))
            {
                return false;
            }
        }
        return true;
    }

    private bool HasHit(Vector3 point, Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            point,
            direction,
            distance,
            _groundLayer
        );
        Debug.DrawRay(point, direction * distance, Color.yellowGreen);
        return hit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _overlappingCount++;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_overlappingCount > 0)
            _overlappingCount--;
    }
}
