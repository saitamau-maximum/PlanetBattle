using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animatorOverlay;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer[] _spriteRenderers;
    private Color[] _originalColors;

    readonly int _hashSpeed = Animator.StringToHash("Speed");
    readonly int _hashJump = Animator.StringToHash("Jump");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        // スプライトレンダラーと初期色を取得
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        if (_spriteRenderers != null && _spriteRenderers.Length > 0)
        {
            _originalColors = new Color[_spriteRenderers.Length];
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _originalColors[i] = _spriteRenderers[i].color;
            }
        }
    }

    private void Update()
    {
        _animator.SetFloat(_hashSpeed, Mathf.Abs(_rigidbody.linearVelocityX));
    }

    public void JumpAnimation()
    {
        _animator.SetTrigger(_hashJump);
    }

    public void AttackAnimation(string weaponName)
    {
        _animator.SetTrigger(weaponName + "Attack");
        _animatorOverlay.SetTrigger(weaponName + "Attack");
    }

    /// <summary>
    /// 死亡時のブリンク処理を開始します
    /// </summary>
    public IEnumerator PlayDeathBlinking(float duration)
    {
        float elapsed = 0f;
        float remainingTime = duration;

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

            yield return null;
        }

        // スプライトのアルファを元に戻す
        RestoreSpriteColors();
    }

    /// <summary>
    /// スプライトの色を元の状態に復元します
    /// </summary>
    public void RestoreSpriteColors()
    {
        if (_spriteRenderers != null && _originalColors != null)
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                if (_spriteRenderers[i] == null) continue;
                _spriteRenderers[i].color = _originalColors[i];
            }
        }
    }
}
