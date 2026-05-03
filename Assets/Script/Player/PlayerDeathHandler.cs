using System.Collections;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Transform _deathTeleportPoint;
    [SerializeField, Min(0f)] private float _lockDuration = 2f;
    [SerializeField] private HealthBarCanvas _healthBarCanvas;
    [SerializeField] private DeathCountdownDisplay _countdownDisplay;

    private PlayerController _controller;
    private Rigidbody2D _rb;
    private PlayerBuildingManager _buildingManager;
    private Health _health;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _buildingManager = GetComponent<PlayerBuildingManager>();
        _health = GetComponent<Health>();

        if (_healthBarCanvas == null)
        {
            _healthBarCanvas = FindFirstObjectByType<HealthBarCanvas>();
        }

        if (_countdownDisplay == null)
        {
            _countdownDisplay = FindFirstObjectByType<DeathCountdownDisplay>();
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

        if (_healthBarCanvas != null)
        {
            _healthBarCanvas.PlayLinearFill(1f, _lockDuration);
        }
        else
        {
            Debug.LogWarning("[PlayerDeathHandler] HealthBarCanvas not found");
        }

        // カウントダウン表示開始
        if (_countdownDisplay != null)
        {
            _countdownDisplay.SetActive(true);
        }
        else
        {
            Debug.LogError("[PlayerDeathHandler] CountdownDisplay is null in DeathRoutine!");
        }

        float remainingTime = _lockDuration;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
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
