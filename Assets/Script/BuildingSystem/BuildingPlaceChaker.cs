using UnityEngine;

public class BuildingPlacementChecker : MonoBehaviour
{
    [SerializeField] private Transform[] _groundCheckPoints;
    [SerializeField] private float _raycastDistance = 0.3f;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private int _overlappingCount = 0;

    public bool CanPlaceBuilding()
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
        _overlappingCount--;
    }
}
