using System;
using UnityEngine;

public abstract class InputProviderBase : IInputProvider, IDisposable
{
    private Vector2 _currentMovement;
    private bool _attackPressed;

    public Vector2 GetMovement() => _currentMovement;
    public bool GetAttack() => _attackPressed;

    public void Tick()
    {
        UpdateInput();
    }

    protected abstract void UpdateInput();
    protected void SetMovement(Vector2 movement) => _currentMovement = movement;
    protected void SetAttack(bool state) => _attackPressed = state;

    public abstract void Dispose();
}