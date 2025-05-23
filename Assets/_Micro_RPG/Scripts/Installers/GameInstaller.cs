using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private PlayerView _playerViewPrefab;
    [SerializeField] private InputActionReference _touchMoveAction;
    [SerializeField] private InputActionReference _touchAttackAction;

    public override void InstallBindings()
    {
        var keyboardActions = new PlayerInputActions();

        Container.BindInstance(_config);

        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        Container.Bind<MovementSystem>().AsSingle().WithArguments(_config.MoveSpeed, _config.Gravity, _config.GroundOffset);
        Container.Bind<AttackSystem>().AsSingle().WithArguments(_config.AttackCooldown);

        Container.Bind<PlayerView>()
            .FromInstance(_playerViewPrefab)
            .AsSingle()
            .OnInstantiated<PlayerView>((ctx, view) => view.Initialize());

        Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();

        var touchProvider = new TouchInputProvider(_touchMoveAction, _touchAttackAction);
        var keyboardProvider = new KeyboardInputProvider(keyboardActions);

        Container.Bind<IInputProvider>()
            .To<CompositeInputProvider>()
            .FromNew()
            .AsSingle()
            .WithArguments(new IInputProvider[] { touchProvider, keyboardProvider });

        Container.Bind<IMovable>().To<PlayerModel>().FromResolve();
        Container.Bind<IMovementApplier>().To<PlayerView>().FromResolve();
        Container.Bind<IAttacker>().To<PlayerModel>().FromResolve();
        Container.Bind<IGroundedStateProvider>().To<PlayerView>().FromResolve();
        Container.Bind<IAttackAnimationHandler>().To<PlayerView>().FromResolve();
    }
}

public interface IMovable
{
    Vector3 Velocity { get; }
    Quaternion TargetRotation { get; }
    bool IsRunning { get; }

    void CalculateMovement(Vector2 input, float deltaTime);
    void CalculateRotation(Vector2 input);
    void ApplyGravity(float deltaTime, bool isGrounded);
}

public interface IAttacker
{
    bool TryAttack();
}

public interface IAttackAnimationHandler
{
    void TriggerAttackAnimation();
    void SetRunningState(bool isRunning);
    void ApplyRotation(Quaternion targetRotation, float speed);
}

public interface IGroundedStateProvider
{
    bool IsGrounded { get; }
}

public interface IMovementApplier
{
    void ApplyMovement(Vector3 velocity);
}