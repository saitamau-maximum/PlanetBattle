using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private float _firstSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _maxMultiJumpCount;

    private Rigidbody2D _rigidbody;
    private PlayerAnimator _playerAnimator;
    private PlayerWeaponManager _weaponManager;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _attackAction2;
    private float _moveInputX;
    private float _currentSpeed;
    private int _currentJumpCount = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _weaponManager = GetComponent<PlayerWeaponManager>();

        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _attackAction2 = InputSystem.actions.FindAction("Attack2");
    }

    private void OnEnable()
    {
        _inputActions.FindActionMap("Player").Enable();

        _attackAction.performed += OnPrimaryWeaponAttack;
        _attackAction2.performed += OnSecondaryWeaponAttack;
        _jumpAction.performed += Jump;
    }
    private void OnDisable()
    {
        _inputActions.FindActionMap("Player").Disable();

        _attackAction.performed -= OnPrimaryWeaponAttack;
        _attackAction2.performed -= OnSecondaryWeaponAttack;
        _jumpAction.performed -= Jump;
    }

    private void Update()
    {
        //武器使用中でなければ移動処理を行う
        if (_weaponManager.CurrentWeaponState != WeaponBase.WeaponState.Attacking)
        {
            _weaponManager.UnequipCurrentWeapon();

            //移動処理
            _moveInputX = _moveAction.ReadValue<Vector2>().x;
            if (_moveInputX != 0)
            {
                Rotate();

                // 加速
                if (Mathf.Abs(_currentSpeed) < _firstSpeed)
                    _currentSpeed = _moveInputX * _firstSpeed;
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _moveInputX * _maxSpeed, _acceleration * Time.deltaTime);
            }
            else
            {
                _currentSpeed = 0f;
            }
        }

        const float EPILISON = 0.01f;
        if (Mathf.Abs(_rigidbody.linearVelocityY) < EPILISON)
        {
            _currentJumpCount = 0;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocityX = _currentSpeed;
    }

    private void Rotate()
    {
        //移動する向きによってキャラクターを反転させる
        if (_moveInputX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_moveInputX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (_moveInputX * _currentSpeed < 0)
        {
            _currentSpeed = 0f;
        }

    }

    private void Jump(InputAction.CallbackContext callbackContext)
    {
        if (_currentJumpCount >= _maxMultiJumpCount) return;

        _rigidbody.linearVelocityY = 0;
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        if (_weaponManager.CurrentWeaponState != WeaponBase.WeaponState.Attacking)
            _playerAnimator.JumpAnimation();

        _currentJumpCount++;
    }

    private void OnPrimaryWeaponAttack(InputAction.CallbackContext callbackContext)
    {
        if (_weaponManager.TryUsePrimaryWeapon())
            PlayWeaponAnimation();
    }

    private void OnSecondaryWeaponAttack(InputAction.CallbackContext callbackContext)
    {
        if (_weaponManager.TryUseSecondaryWeapon())
            PlayWeaponAnimation();
    }
    private void PlayWeaponAnimation()
    {
        _currentSpeed = 0f;
        _playerAnimator.AttackAnimation(_weaponManager.GetCurrentWeaponName);
    }
}

