using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour, IAttackAnimationHandler, IGroundedStateProvider, IMovementApplier
{
    [SerializeField] private Transform _visualRoot;
    [SerializeField] private int _animationLayer = 0;
    [SerializeField] private float _attackAnimationEarlyExit = 0.95f;

    private CharacterController _controller;
    private CancellationTokenSource _attackCts;
    private Animator _animator;

    private readonly int _runningHash = Animator.StringToHash("IsRunning");
    private readonly int _attackHash = Animator.StringToHash("AttackTrigger");
    private readonly int _attackStateHash = Animator.StringToHash("Attack");

    public bool IsGrounded => _controller != null && _controller.isGrounded;

    public void Initialize()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _visualRoot ??= transform;
    }

    public void TriggerAttackAnimation()
    {
        _attackCts?.Cancel();
        _attackCts?.Dispose();

        _attackCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        _animator.SetTrigger(_attackHash);
        AttackProcess(_attackCts.Token).Forget();
    }

    private async UniTaskVoid AttackProcess(CancellationToken token)
    {
        try
        {
            await UniTask.WaitUntil(() =>
                _animator.GetCurrentAnimatorStateInfo(_animationLayer).shortNameHash == _attackStateHash,
                cancellationToken: token);

            await UniTask.WaitUntil(() =>
                _animator.GetCurrentAnimatorStateInfo(_animationLayer).normalizedTime >= _attackAnimationEarlyExit ||
                !_animator.GetCurrentAnimatorStateInfo(_animationLayer).IsName("Attack"),
                cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            _animator.ResetTrigger(_attackHash);
        }
    }

    private void OnDestroy()
    {
        _attackCts?.Cancel();
        _attackCts?.Dispose();
        _attackCts = null;
    }

    public void ApplyMovement(Vector3 motion)
    {
        if (_controller.enabled)
            _controller.Move(motion);
    }

    public void ApplyRotation(Quaternion targetRotation, float rotationSpeed)
    {
        if (_visualRoot != null)
        {
            _visualRoot.rotation = Quaternion.RotateTowards(
                _visualRoot.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void SetRunningState(bool isRunning)
    {
        _animator.SetBool(_runningHash, isRunning);
    }
}