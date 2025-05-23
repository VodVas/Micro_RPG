//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.EventSystems;
//using System;

//public class InputProvider : IInputProvider, IDisposable
//{
//    private const float JOYSTICK_DEADZONE = 0.1f;

//    private readonly InputActionReference _moveAction;
//    private readonly InputActionReference _attackAction;
//    private readonly PlayerInputActions _keyboardActions;

//    private Vector2 _currentMovement;
//    private volatile bool _attackPressedByCallback;
//    private bool _isTouchActive;
//    private bool _isBlockedByUI;
//    private bool _isDisposed;

//    public InputProvider(InputActionReference moveAction, InputActionReference attackAction, PlayerInputActions keyboardActions)
//    {
//        _moveAction = moveAction ?? throw new ArgumentNullException(nameof(moveAction));
//        _attackAction = attackAction ?? throw new ArgumentNullException(nameof(attackAction));
//        _keyboardActions = keyboardActions ?? throw new ArgumentNullException(nameof(keyboardActions));

//        EnableActions();
//        SubscribeToEvents();
//    }

//    public Vector2 GetMovement() => _currentMovement;

//    public bool GetAttack() => (!_isBlockedByUI && _attackPressedByCallback) || (_keyboardActions?.Gameplay.Attack.IsPressed() ?? false);

//    public void Tick()
//    {
//        if (_isDisposed) return;

//        _isBlockedByUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

//        UpdateMovement();
//    }

//    private void UpdateMovement()
//    {
//        var touchMovement = Vector2.zero;

//        if (_moveAction?.action != null && _moveAction.action.enabled)
//        {
//            touchMovement = _moveAction.action.ReadValue<Vector2>();
//        }

//        _isTouchActive = touchMovement.sqrMagnitude > JOYSTICK_DEADZONE * JOYSTICK_DEADZONE;

//        _currentMovement = _isTouchActive
//            ? touchMovement.normalized
//            : _keyboardActions?.Gameplay.Keyboard.ReadValue<Vector2>() ?? Vector2.zero;
//    }

//    private void EnableActions()
//    {
//        try
//        {
//            _moveAction?.action?.Enable();
//            _attackAction?.action?.Enable();
//            _keyboardActions?.Enable();
//        }
//        catch (Exception exception)
//        {
//            Debug.LogError($"Error enabling input actions: {exception.Message}");
//        }
//    }

//    private void SubscribeToEvents()
//    {
//        if (_attackAction?.action != null)
//        {
//            try
//            {
//                _attackAction.action.performed += OnAttack;
//                _attackAction.action.canceled += OnAttackEnd;
//            }
//            catch (Exception exception)
//            {
//                Debug.LogError($"Error subscribing to input events: {exception.Message}");
//            }
//        }
//    }

//    private void OnAttack(InputAction.CallbackContext context)
//    {
//        _attackPressedByCallback = true;
//    }

//    private void OnAttackEnd(InputAction.CallbackContext _)
//    {
//        _attackPressedByCallback = false;
//    }

//    public void Dispose()
//    {
//        if (_isDisposed) return;

//        _isDisposed = true;

//        try
//        {
//            if (_attackAction?.action != null)
//            {
//                _attackAction.action.performed -= OnAttack;
//                _attackAction.action.canceled -= OnAttackEnd;
//            }

//            _moveAction?.action?.Dispose();
//            _attackAction?.action?.Dispose();
//            _keyboardActions?.Dispose();
//        }
//        catch (Exception exception)
//        {
//            Debug.LogWarning($"Error during InputProvider cleanup: {exception.Message}");
//        }
//    }
//}