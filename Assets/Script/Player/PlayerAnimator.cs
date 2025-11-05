using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetBool("Walk", Mathf.Abs(_rigidbody.linearVelocityX) > 0);
        _animator.SetBool("Jump", _rigidbody.linearVelocityY > 0);
        _animator.SetBool("Fall", _rigidbody.linearVelocityY < 0);
    }

    public void JumpAnimation()
    {
        _animator.SetBool("Jump", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //_animator.SetBool("Jump", false);
        _animator.SetBool("Fall", false);
    }
}
