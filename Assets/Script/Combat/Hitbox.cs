using System;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour //ダメージを与えるのは武器に任せる
{
    private const float HIT_COOLDOWN = 0.1f;

    [SerializeField] protected bool _isAreaAttack;
    private HitTargetSelector _targetSelector;

    public Action<Health> OnFirstHit;
    public Action<Health> OnContinuousHit;

    private Health _singleHitTarget; // 単体攻撃のヒット対象
    private readonly List<Collider2D> _overlappingColliders = new();
    private readonly List<Health> _candidates = new();
    private float _exitTime;

    private void Awake()
    {
        _targetSelector = GetComponent<HitTargetSelector>();
        if (_targetSelector == null)
        {
            _targetSelector = gameObject.AddComponent<RandomHitTargetSelector>();
        }
    }

    private void OnEnable()
    {
        _singleHitTarget = null;
        _overlappingColliders.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Health health)) return;

        if (!_isAreaAttack)
        {
            _overlappingColliders.Add(collision);
        }
        else
        {
            OnFirstHit?.Invoke(health);
        }
    }

    private void LateUpdate()
    {
        if (_isAreaAttack || _singleHitTarget != null) return;

        if (Time.time - _exitTime <= HIT_COOLDOWN) return;

        // 接触通知が揃ってから、選択ルールに従って対象を選ぶ。
        CollectCandidates();
        _singleHitTarget = _targetSelector.SelectTarget(_candidates);
        if (_singleHitTarget != null) OnFirstHit?.Invoke(_singleHitTarget);
    }

    private void CollectCandidates()
    {
        _candidates.Clear();
        foreach (Collider2D collision in _overlappingColliders)
        {
            if (collision == null || !collision.enabled || !collision.gameObject.activeInHierarchy) continue;
            if (!collision.TryGetComponent(out Health health)) continue;

            if (!_candidates.Contains(health)) _candidates.Add(health);
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
        _overlappingColliders.Remove(collision);
        if (!collision.TryGetComponent(out Health health)) return;

        // 単体攻撃の場合、ヒット対象が離れたらリセットする
        if (!_isAreaAttack && _singleHitTarget == health)
        {
            _singleHitTarget = null;
            _exitTime = Time.time;
        }
    }
}
