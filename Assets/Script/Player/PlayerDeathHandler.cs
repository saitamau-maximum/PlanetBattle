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
    private UnityEngine.SpriteRenderer[] _spriteRenderers;
    private UnityEngine.Color[] _originalColors;

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
        // キャラクターのスプライトを取得しておく
        _spriteRenderers = GetComponentsInChildren<UnityEngine.SpriteRenderer>(true);
        if (_spriteRenderers != null && _spriteRenderers.Length > 0)
        {
            _originalColors = new UnityEngine.Color[_spriteRenderers.Length];
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _originalColors[i] = _spriteRenderers[i].color;
            }
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

        // カウントダウン表示開始
        if (_countdownDisplay != null)
        {
            _countdownDisplay.SetActive(true);
        }

        float remainingTime = _lockDuration;
        float elapsed = 0f;

        while (remainingTime > 0)
        {
            float dt = Time.deltaTime;
            remainingTime -= dt;
            remainingTime = Mathf.Max(0, remainingTime);
            elapsed += dt;

            // アルファを0.5〜1で往復させる。片方向の遷移時間は0.5秒。
            float ping = Mathf.PingPong(elapsed, 0.5f) / 0.5f; // 0..1..0 over 1s
            float alpha = Mathf.Lerp(0.5f, 1f, ping);

            if (_spriteRenderers != null)
            {
                for (int i = 0; i < _spriteRenderers.Length; i++)
                {
                    if (_spriteRenderers[i] == null) continue;
                    Color c = _spriteRenderers[i].color;
                    c.a = alpha;
                    _spriteRenderers[i].color = c;
                }
            }

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
        // スプライトのアルファを元に戻す
        if (_spriteRenderers != null && _originalColors != null)
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                if (_spriteRenderers[i] == null) continue;
                _spriteRenderers[i].color = _originalColors[i];
            }
        }

        _controller.SetControlLock(false);
    }
}
