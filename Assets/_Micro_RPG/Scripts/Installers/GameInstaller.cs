//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using Zenject;

//public class GameInstaller : MonoInstaller
//{
//    [SerializeField] private PlayerConfig _config;
//    [SerializeField] private PlayerView _playerViewPrefab;
//    [SerializeField] private InputActionReference _touchMoveAction;
//    [SerializeField] private InputActionReference _touchAttackAction;

//    public override void InstallBindings()
//    {
//        var keyboardActions = new PlayerInputActions();

//        Container.BindInstance(_config);

//        Container.Bind<PlayerModel>().AsSingle().NonLazy();
//        Container.Bind<MovementSystem>().AsSingle().WithArguments(_config.MoveSpeed, _config.Gravity, _config.GroundOffset);
//        Container.Bind<AttackSystem>().AsSingle().WithArguments(_config.AttackCooldown);

//        Container.Bind<PlayerView>()
//            .FromInstance(_playerViewPrefab)
//            .AsSingle()
//            .OnInstantiated<PlayerView>((ctx, view) => view.Initialize());

//        Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();

//        var touchProvider = new TouchInputProvider(_touchMoveAction, _touchAttackAction);
//        var keyboardProvider = new KeyboardInputProvider(keyboardActions);
//        //var mouseProvider = new MouseInputProvider(keyboardActions);

//        Container.Bind<IInputProvider>()
//            .To<CompositeInputProvider>()
//            .FromNew()
//            .AsSingle()
//            //.WithArguments(new IInputProvider[] { touchProvider, keyboardProvider });
//            .WithArguments(new IInputProvider[] { touchProvider, keyboardProvider });

//        Container.Bind<IMovable>().To<PlayerModel>().FromResolve();
//        Container.Bind<IMovementApplier>().To<PlayerView>().FromResolve();
//        Container.Bind<IAttacker>().To<PlayerModel>().FromResolve();
//        Container.Bind<IGroundedStateProvider>().To<PlayerView>().FromResolve();
//        Container.Bind<IAttackAnimationHandler>().To<PlayerView>().FromResolve();
//    }
//}

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

    [Header("Mouse Control Settings")]
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private LayerMask _clickableLayers = -1;
    [SerializeField] private LayerMask _groundLayer = 1 << 0;
    [SerializeField] private float _mouseStoppingDistance = 0.5f;
    [SerializeField] private float _mouseRotationSpeed = 720f;

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

        // Input providers
        var touchProvider = new TouchInputProvider(_touchMoveAction, _touchAttackAction);
        var keyboardProvider = new KeyboardInputProvider(keyboardActions);
        var mouseProvider = new MouseInputProvider(
            keyboardActions,
            _gameCamera,
            _clickableLayers,
            _groundLayer,
            _mouseStoppingDistance,
            _mouseRotationSpeed
        );

        Container.Bind<IInputProvider>()
            .To<CompositeInputProvider>()
            .FromNew()
            .AsSingle()
            .WithArguments(new IInputProvider[] { mouseProvider, touchProvider, keyboardProvider });

        Container.Bind<IMovable>().To<PlayerModel>().FromResolve();
        Container.Bind<IMovementApplier>().To<PlayerView>().FromResolve();
        Container.Bind<IAttacker>().To<PlayerModel>().FromResolve();
        Container.Bind<IGroundedStateProvider>().To<PlayerView>().FromResolve();
        Container.Bind<IAttackAnimationHandler>().To<PlayerView>().FromResolve();
    }
}
