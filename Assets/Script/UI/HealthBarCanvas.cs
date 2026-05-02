using UnityEngine.UI;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private float _speed = 5f; // 大きいほど速い

    private float _targetRatio;

    private void Start()
    {
        _health.OnHealthChanged += OnHealthChanged;
        _targetRatio = _health.HealthRatio;
        _healthBarFill.fillAmount = _targetRatio;
    }

    private void OnDestroy()
    {
        _health.OnHealthChanged -= OnHealthChanged;
    }

    private void Update()
    {
        _healthBarFill.fillAmount = Mathf.Lerp(
            _healthBarFill.fillAmount,
            _targetRatio,
            Time.deltaTime * _speed
        );
    }

    private void OnHealthChanged(float ratio)
    {
        _targetRatio = ratio;
    }
}