using UnityEngine;

public sealed class MovementSystem
{
    private const float DeadZoneSqr = 0.0025f;

    private readonly float _moveSpeed;
    private readonly float _gravity;
    private readonly float _groundOffset;

    private Vector3 _velocity;
    private Quaternion _targetRotation = Quaternion.identity;
    private bool _isRunning;

    public MovementSystem(float moveSpeed, float gravity, float groundOffset)
    {
        _moveSpeed = moveSpeed;
        _gravity = gravity;
        _groundOffset = groundOffset;
    }

    public Vector3 Velocity => _velocity;
    public Quaternion TargetRotation => _targetRotation;
    public bool IsRunning => _isRunning;

    public void CalculateMovement(Vector2 input, float deltaTime)
    {
        if (input.sqrMagnitude <= DeadZoneSqr)
        {
            SetRunning(false);
            UpdateVelocity(new Vector3(0f, _velocity.y, 0f));
            return;
        }

        input = input.sqrMagnitude > 1f ? input.normalized : input;
        SetRunning(true);

        var speed = _moveSpeed * deltaTime;
        UpdateVelocity(new Vector3(
            input.x * speed,
            _velocity.y,
            input.y * speed
        ));
    }

    public void CalculateRotation(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f) return;

        var newRotation = Quaternion.LookRotation(
            new Vector3(input.x, 0, input.y),
            Vector3.up
        );

        if (_targetRotation != newRotation)
        {
            _targetRotation = newRotation;
        }
    }

    public void ApplyGravity(float deltaTime, bool isGrounded)
    {
        var newY = isGrounded && _velocity.y < 0
            ? -_groundOffset
            : _velocity.y + _gravity * deltaTime;

        UpdateVelocity(new Vector3(_velocity.x, newY, _velocity.z));
    }

    private void UpdateVelocity(Vector3 newVelocity)
    {
        if (Vector3.SqrMagnitude(_velocity - newVelocity) < 0.0001f) return;

        _velocity = newVelocity;
    }

    private void SetRunning(bool state)
    {
        if (_isRunning == state) return;
        _isRunning = state;
    }
}

/*using UnityEngine;

public sealed class MovementSystem
{
    private const float DeadZoneSqr = 0.0025f;
    private readonly float _moveSpeed;
    private readonly float _gravity;
    private readonly float _groundOffset;

    private Vector3 _velocity;
    private Quaternion _targetRotation;
    private bool _isRunning;

    public MovementSystem(float moveSpeed, float gravity, float groundOffset)
    {
        _moveSpeed = moveSpeed;
        _gravity = gravity;
        _groundOffset = groundOffset;
    }

    public void CalculateMovement(Vector2 input, float deltaTime)
    {
        if (input.sqrMagnitude <= DeadZoneSqr)
        {
            SetRunning(false);
            UpdateVelocity(new Vector3(0f, _velocity.y, 0f));
            return;
        }

        input = input.sqrMagnitude > 1f ? input.normalized : input;
        SetRunning(true);

        var speed = _moveSpeed * deltaTime;
        UpdateVelocity(new Vector3(
            input.x * speed,
            _velocity.y,
            input.y * speed
        ));
    }

    public void CalculateRotation(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f) return;

        var newRotation = Quaternion.LookRotation(
            new Vector3(input.x, 0, input.y),
            Vector3.up
        );

        if (_targetRotation != newRotation)
        {
            _targetRotation = newRotation;
        }
    }

    public void ApplyGravity(float deltaTime, bool isGrounded)
    {
        var newY = isGrounded && _velocity.y < 0
            ? -_groundOffset
            : _velocity.y + _gravity * deltaTime;

        UpdateVelocity(new Vector3(_velocity.x, newY, _velocity.z));
    }

    private void UpdateVelocity(Vector3 newVelocity)
    {
        if (Vector3.SqrMagnitude(_velocity - newVelocity) < 0.0001f) return;

        _velocity = newVelocity;
    }

    private void SetRunning(bool state)
    {
        if (_isRunning == state) return;
        _isRunning = state;
    }
}*/