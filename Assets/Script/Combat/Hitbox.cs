using System;
using UnityEngine;

public class Hitbox : MonoBehaviour //ダメージを与えるのは武器に任せる
{
    [SerializeField] protected bool _isAreaAttack;

    public Action<Health> OnFirstHit;
    public Action<Health> OnContinuousHit;

    private Health _singleHitTarget; // 単体攻撃のヒット対象
    private float _exitTime;

    private void OnEnable()
    {
        _singleHitTarget = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        if (!_isAreaAttack)
        {
            // 単体攻撃の場合、最初のヒット対象を記録する            
            // 前回のヒットから少し時間が経過している場合のみ新しい対象を記録する（2回ヒットを防止）
            const float HIT_COOLDOWN = 0.1f;
            if (_singleHitTarget == null && Time.time - _exitTime > HIT_COOLDOWN)
            {
                _singleHitTarget = health;
                OnFirstHit?.Invoke(_singleHitTarget);
            }
        }
        else
        {
            OnFirstHit?.Invoke(health);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        // 単体攻撃の場合、最初のヒット対象以外は無視する
        if (!_isAreaAttack && _singleHitTarget != health) return;

        OnContinuousHit?.Invoke(health);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        // 単体攻撃の場合、ヒット対象が離れたらリセットする
        if (!_isAreaAttack && _singleHitTarget == health)
        {
            _singleHitTarget = null;
            _exitTime = Time.time;
        }
    }
}
