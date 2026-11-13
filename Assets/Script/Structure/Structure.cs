using UnityEngine;
using Utility;

public abstract class Structure : MonoBehaviour
{
    private const float MIN_REMOVAL_HEALTH_RATIO = 1f;

    protected enum ExecutionStatus
    {
        Success,
        Failure,
        Running
    }

    [Header("時間設定")]
    [SerializeField] protected float _executeInterval = 2.0f;

    [Header("削除アイコン")]
    [SerializeField] private GameObject _removalIcon;

    private Health _health;
    private int _refundCost;
    private bool _isRemoving;
    public int RefundCost => _refundCost;
    public bool CanRemove => !_isRemoving && _health.HealthRatio >= MIN_REMOVAL_HEALTH_RATIO;
    protected GameObject _player;
    protected CountdownTimer _cooldownTimer;

    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
    }
    protected virtual void Start()
    {
        _health.OnDied += Die;
        _cooldownTimer = new CountdownTimer(_executeInterval);
        HideRemovalIcon();
    }

    public virtual void Init(GameObject player, int buildCost)
    {
        _player = player;
        _refundCost = buildCost;
    }

    public bool TryRemove()
    {
        if (!CanRemove) return false;

        _isRemoving = true;
        Destroy(gameObject);
        return true;
    }

    public void ShowRemovalIcon()
    {
        _removalIcon.SetActive(true);
    }

    public void HideRemovalIcon()
    {
        _removalIcon.SetActive(false);
    }

    protected virtual void Update()
    {
        if (_isRemoving) return;

        if (_cooldownTimer.IsFinished())
        {
            ExecutionStatus status = Execute();
            if (status == ExecutionStatus.Success)
            {
                _cooldownTimer.Start();
            }
        }
    }

    // Failure / Running の場合はクールダウンを開始せず、次フレームも呼び出す。
    protected abstract ExecutionStatus Execute();

    protected void OnDestroy()
    {
        _health.OnDied -= Die;
    }
    protected void Die()
    {
        if (_isRemoving) return;

        _isRemoving = true;
        Destroy(gameObject);
    }
}
