using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Transform _deathTeleportPoint;
    [SerializeField, Min(0f)] private float _lockDuration = 2f;
    [SerializeField] private CountdownDisplay _countdownDisplay;

    private PlayerController _controller;
    private PlayerAnimator _playerAnimator;
    private Rigidbody2D _rb;
    private PlayerBuildingManager _buildingManager;
    private Health _health;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _rb = GetComponent<Rigidbody2D>();
        _buildingManager = GetComponent<PlayerBuildingManager>();
        _health = GetComponent<Health>();

        if (_countdownDisplay == null)
        {
            _countdownDisplay = FindFirstObjectByType<CountdownDisplay>();
        }
    }

    private void Start()
    {
        if (_health != null)
        {
            _health.OnDied += HandleDeath;
        }
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDied -= HandleDeath;
        }
    }

    private void HandleDeath()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        _controller.SetControlLock(true);

        _rb.linearVelocity = Vector2.zero;
        float originalGravityScale = _rb.gravityScale;
        _rb.gravityScale = 0f;

        // 建築モード中なら攻撃モードへ戻す
        _buildingManager.ExitBuildingMode();

        if (_deathTeleportPoint != null)
        {
            transform.position = _deathTeleportPoint.position;
        }

        // カウントダウン表示開始
        if (_countdownDisplay != null)
        {
            _countdownDisplay.SetActive(true);
        }

        // ブリンク処理を実行
        yield return StartCoroutine(_playerAnimator.PlayDeathBlinking(_lockDuration));

        // カウントダウン中の時間経過処理
        float remainingTime = _lockDuration;
        while (remainingTime > 0)
        {
            float dt = Time.deltaTime;
            remainingTime -= dt;
            remainingTime = Mathf.Max(0, remainingTime);

            if (_countdownDisplay != null)
            {
                _countdownDisplay.SetCountdown(remainingTime);
            }

            yield return null;
        }

        // HP を最大値に回復
        if (_health != null)
        {
            _health.RecoverToMax();
        }

        // 重力を元に戻す
        _rb.gravityScale = originalGravityScale;

        // カウントダウン表示非表示
        if (_countdownDisplay != null)
        {
            _countdownDisplay.SetActive(false);
        }

        _controller.SetControlLock(false);
    }
}
