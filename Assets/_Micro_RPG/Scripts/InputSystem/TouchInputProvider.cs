using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class TouchInputProvider : InputProviderBase
{
    private readonly InputActionReference _moveAction;
    private readonly InputActionReference _attackAction;

    private readonly Action<InputAction.CallbackContext> _attackPerformedDelegate;
    private readonly Action<InputAction.CallbackContext> _attackCanceledDelegate;

    public TouchInputProvider(InputActionReference moveAction, InputActionReference attackAction)
    {
        _moveAction = moveAction ?? throw new ArgumentNullException(nameof(moveAction));
        _attackAction = attackAction ?? throw new ArgumentNullException(nameof(attackAction));

        _attackPerformedDelegate = _ => SetAttack(true);
        _attackCanceledDelegate = _ => SetAttack(false);

        EnableActions();
        SubscribeToEvents();
    }

    protected override void UpdateInput()
    {
        var input = _moveAction.action.ReadValue<Vector2>();
        SetMovement(input.normalized);
    }

    private void EnableActions()
    {
        _moveAction.action.Enable();
        _attackAction.action.Enable();
    }

    private void SubscribeToEvents()
    {
        if (_attackAction?.action != null)
        {
            _attackAction.action.performed += _attackPerformedDelegate;
            _attackAction.action.canceled += _attackCanceledDelegate;
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (_attackAction?.action != null)
        {
            _attackAction.action.performed -= _attackPerformedDelegate;
            _attackAction.action.canceled -= _attackCanceledDelegate;
        }
    }

    public override void Dispose()
    {
        UnsubscribeFromEvents();

        _moveAction?.action?.Dispose();
        _attackAction?.action?.Dispose();
    }
}

/*using UnityEngine;
using UnityEngine.InputSystem;

public sealed class TouchInputProvider : InputProviderBase
{
    private readonly InputActionReference _moveAction;
    private readonly InputActionReference _attackAction;

    public TouchInputProvider(InputActionReference moveAction, InputActionReference attackAction)
    {
        _moveAction = moveAction;
        _attackAction = attackAction;
        EnableActions();
        SubscribeToEvents();
    }

    protected override void UpdateInput()
    {
        var input = _moveAction.action.ReadValue<Vector2>();
        SetMovement(input.normalized);
    }

    private void EnableActions()
    {
        _moveAction.action.Enable();
        _attackAction.action.Enable();
    }

    private void SubscribeToEvents()
    {
        _attackAction.action.performed += _ => SetAttack(true);
        _attackAction.action.canceled += _ => SetAttack(false);
    }

    public override void Dispose()
    {
        _attackAction.action.performed -= _ => SetAttack(true);
        _attackAction.action.canceled -= _ => SetAttack(false);
        _moveAction.action.Dispose();
        _attackAction.action.Dispose();
    }
}*/