using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;

    public float HealthRatio { get => Mathf.Clamp(_maxHealth == 0 ? 0 : (float)_currentHealth / _maxHealth, 0, 1); }

    public event Action<float> OnChangeHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

#if UNITY_EDITOR
    // インスペクターで値が変更されたときに呼ばれる
    private void OnValidate()
    {
        // ゲーム実行中のみ、Healthバーの表示を更新する
        if (Application.isPlaying)
        {
            OnChangeHealth?.Invoke(HealthRatio);
        }
    }
#endif

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        OnChangeHealth?.Invoke(HealthRatio);
        Debug.Log($"{gameObject.name} が{amount}ダメージを受けた");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} が倒れました");
        Destroy(gameObject);
    }
}