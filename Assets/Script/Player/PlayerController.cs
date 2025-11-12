using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rigidbody;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private float _moveInputX;

    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        _moveInputX = _moveAction.ReadValue<Vector2>().x;

        if (_jumpAction.WasPerformedThisFrame() && Mathf.Abs(_rigidbody.linearVelocityY) == 0)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = _moveInputX * _speed;
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}

