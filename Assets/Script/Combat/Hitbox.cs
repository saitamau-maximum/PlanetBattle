using System;
using UnityEngine;

public class Hitbox : MonoBehaviour //ダメージを与えるのは武器に任せる
{
    [SerializeField] protected bool _isAreaAttack;

    public Action<Health> OnFirstHit;
    public Action<Health> OnContinuousHit;

    private Health _hitTarget;

    private void OnEnable()
    {
        _hitTarget = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        if (!_isAreaAttack && _hitTarget != null) return;// 範囲攻撃の場合

        _hitTarget = health;
        OnFirstHit?.Invoke(health);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        if (!_isAreaAttack && _hitTarget != health) return;

        OnContinuousHit?.Invoke(health);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        if (_hitTarget == health)
            _hitTarget = null;
    }
}
