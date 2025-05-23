using UnityEngine;

public sealed class KeyboardInputProvider : InputProviderBase
{
    private readonly PlayerInputActions _keyboardActions;

    public KeyboardInputProvider(PlayerInputActions keyboardActions)
    {
        _keyboardActions = keyboardActions;
        _keyboardActions.Enable();
    }

    protected override void UpdateInput()
    {
        var moveInput = _keyboardActions.Gameplay.Keyboard.ReadValue<Vector2>();
        var attackState = _keyboardActions.Gameplay.Attack.IsPressed();
        SetMovement(moveInput);
        SetAttack(attackState);
    }

    public override void Dispose() => _keyboardActions.Dispose();
}