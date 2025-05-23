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
    }
}