using UnityEngine.UI;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _healthBarFill;

    private void Start()
    {
        _health.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(_health.HealthRatio);
    }
    private void OnDestroy()
    {
        _health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float ratio)
    {
        _healthBarFill.fillAmount = ratio;
    }
}
