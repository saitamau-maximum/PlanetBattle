using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth;
    public float HealthRatio => _maxHealth == 0 ? 0 : Mathf.Clamp01((float)_currentHealth / _maxHealth);

    public event Action<float> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        OnDied += Die;
    }

    private void OnDestroy()
    {
        OnDied -= Die;
    }

    // インスペクターで値が変更されたときに呼ばれる
    private void OnValidate()
    {
        // ゲーム実行中のみ、Healthバーの表示を更新する
        if (Application.isPlaying)
        {
            NotifyHealthChanged();
        }
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0 || _currentHealth <= 0) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        NotifyHealthChanged();
        Debug.Log($"{gameObject.name} が{amount}ダメージを受けた");

        if (_currentHealth == 0)
        {
            OnDied?.Invoke();
        }
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(HealthRatio);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} が倒れました");
        Destroy(gameObject);
    }
}