using System;
using UnityEngine;
using Zenject;

public class PlayerController : ITickable, IFixedTickable, IDisposable
{
    private readonly PlayerModel _model;
    private readonly PlayerView _view;
    private readonly PlayerConfig _config;
    private readonly IInputProvider _inputProvider;

    public PlayerController(
        PlayerModel model,
        PlayerView view,
        PlayerConfig config,
        IInputProvider inputProvider)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _inputProvider = inputProvider ?? throw new ArgumentNullException(nameof(inputProvider));
    }

    public void Tick()
    {
        _inputProvider.Tick();

        Move();
        Attack();
    }

    public void FixedTick()
    {
        if (_view == null) return;

        _model.ApplyGravity(Time.fixedDeltaTime, _view.IsGrounded);
    }

    public void Dispose()
    {
        (_inputProvider as IDisposable)?.Dispose();
    }

    private void Move()
    {
        Vector2 input = _inputProvider.GetMovement();

        _model.CalculateMovement(input, Time.deltaTime);
        _model.CalculateRotation(input);

        if (_view != null)
        {
            _view.ApplyMovement(_model.Velocity);
            _view.ApplyRotation(_model.TargetRotation, _config.RotationSpeed);
            _view.SetRunningState(_model.IsRunning);
        }
    }

    private void Attack()
    {
        if (_inputProvider.GetAttack() && _model.TryAttack())
        {
            _view.TriggerAttackAnimation();
        }
    }
}