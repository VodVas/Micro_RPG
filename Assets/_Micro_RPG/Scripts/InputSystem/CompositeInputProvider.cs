using UnityEngine;

public sealed class CompositeInputProvider : IInputProvider
{
    private readonly IInputProvider[] _providers;

    public CompositeInputProvider(IInputProvider[] providers) => _providers = providers;

    public Vector2 GetMovement()
    {
        foreach (var provider in _providers)
        {
            var input = provider.GetMovement();

            if (input.sqrMagnitude > 0.01f)
                return input.normalized;
        }

        return Vector2.zero;
    }

    public bool GetAttack()
    {
        foreach (var provider in _providers)
            if (provider.GetAttack()) return true;

        return false;
    }

    public void Tick()
    {
        foreach (var provider in _providers)
            provider.Tick();
    }
}