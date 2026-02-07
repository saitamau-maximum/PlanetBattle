using NUnit.Framework;
using UnityEngine;

public class EnvironmentSensor : MonoBehaviour
{
    [SerializeField] private Transform _groundCheckPointFront;
    [SerializeField] private Transform _groundCheckPointCenter;
    [SerializeField] private Transform _groundCheckPointTop;
    [SerializeField] private Transform _groundCheckPointBack;
    [SerializeField] private Transform _groundCheckPointStepUp;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _raycastDistance = 0.3f;
    [SerializeField] private float _raycastAcrossDistance = 1.5f;
    [SerializeField] private float _raycastStepUpDistance = 0.8f;
    [SerializeField] private float _raycastFallDistance = 1.3f;

    // 地面に接地しているかどうかを判定する
    public bool IsGrounded()
    {
        return HasHit(_groundCheckPointCenter.position, Vector2.down, _raycastDistance);
    }

    // 後方に地面があるかどうかを判定する
    public bool IsGroundBehind()
    {
        return HasHit(_groundCheckPointBack.position, Vector2.down, _raycastDistance);
    }

    // 前方に地面があるかどうかを判定する
    public bool IsEdgeAhead()
    {
        return !HasHit(_groundCheckPointFront.position, Vector2.down, _raycastDistance);
    }

    // 前方にジャンプで渡れる地形があるかどうかを判定する
    public bool CanJumpAcross()
    {
        Vector3 groundOffset = new(0, -0.1f, 0);
        return IsEdgeAhead() && HasHit(_groundCheckPointFront.position + groundOffset, transform.right, _raycastAcrossDistance);
    }

    // 一段ジャンプできる地形があるかどうかを判定する
    public bool CanJumpStepUp()
    {
        return HasHit(_groundCheckPointStepUp.position, Vector2.up, _raycastStepUpDistance) && !HasHit(_groundCheckPointTop.position, Vector2.up, _raycastStepUpDistance);
    }

    // 前方に落下できる地形があるかどうかを判定する
    public bool CanStepDown()
    {
        Vector3 groundOffset = new(transform.right.x * 0.5f, 0, 0);
        return IsEdgeAhead() && HasHit(_groundCheckPointFront.position + groundOffset, Vector2.down, _raycastFallDistance);
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
}
