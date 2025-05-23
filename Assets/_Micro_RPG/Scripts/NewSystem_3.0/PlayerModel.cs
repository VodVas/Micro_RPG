using UnityEngine;

public class PlayerModel
{
    private readonly MovementSystem _movement;
    private readonly AttackSystem _attack;

    public Vector3 Velocity { get; private set; }
    public Quaternion TargetRotation => _movement.TargetRotation;
    public bool IsRunning => _movement.IsRunning;

    public PlayerModel(PlayerConfig config, MovementSystem movement, AttackSystem attack)
    {
        _movement = movement ?? throw new System.ArgumentNullException(nameof(movement));
        _attack = attack ?? throw new System.ArgumentNullException(nameof(attack));

        Velocity = Vector3.zero;
    }

    public void CalculateMovement(Vector2 input, float deltaTime)
    {
        _movement.CalculateMovement(input, deltaTime);
        Velocity = _movement.Velocity;
    }

    public bool TryAttack() => _attack.TryAttack();

    public void CalculateRotation(Vector2 input) => _movement.CalculateRotation(input);

    public void ApplyGravity(float deltaTime, bool isGrounded)
    {
        _movement.ApplyGravity(deltaTime, isGrounded);
        Velocity = _movement.Velocity;
    }
}

/*using System;
using UnityEngine;

public class PlayerModel
{
    private const float DeadZoneSqr = 0.0025f;

    private readonly PlayerConfig _config;

    private float _lastAttackTime;
    private Vector3 _velocity;
    private Quaternion _targetRotation;
    private bool _isRunning;

    public PlayerModel(PlayerConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        Velocity = Vector3.zero;
        TargetRotation = Quaternion.identity;
    }

    public Vector3 Velocity
    {
        get => _velocity;
        private set
        {
            if (_velocity == value) return;

            _velocity = value;
        }
    }

    public Quaternion TargetRotation
    {
        get => _targetRotation;
        private set
        {
            if (_targetRotation == value) return;

            _targetRotation = value;
        }
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            if (_isRunning == value) return;

            _isRunning = value;
        }
    }

    public void CalculateMovement(Vector2 input, float deltaTime)
    {
        if (input.sqrMagnitude <= DeadZoneSqr)
        {
            IsRunning = false;
            UpdateVelocity(new Vector3(0f, Velocity.y, 0f));
            return;
        }

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        IsRunning = true;
        float speed = _config.MoveSpeed * deltaTime;

        UpdateVelocity(new Vector3(input.x * speed, Velocity.y, input.y * speed));
    }

    public bool TryAttack()
    {
        if (Time.time < _lastAttackTime + _config.AttackCooldown)
            return false;

        _lastAttackTime = Time.time;

        return true;
    }

    public void CalculateRotation(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f) return;

        TargetRotation = Quaternion.LookRotation(
            new Vector3(input.x, 0, input.y),
            Vector3.up
        );
    }

    public void ApplyGravity(float deltaTime, bool isGrounded)
    {
        float newY = isGrounded && Velocity.y < 0
            ? -_config.GroundOffset
            : Velocity.y + _config.Gravity * deltaTime;

        UpdateVelocity(new Vector3(Velocity.x, newY, Velocity.z));
    }

    private void UpdateVelocity(Vector3 newVelocity)
    {
        if (!float.IsNaN(newVelocity.x) && !float.IsNaN(newVelocity.y) && !float.IsNaN(newVelocity.z))
            Velocity = newVelocity;
        else
            Debug.LogWarning("Attempted to set velocity with NaN values");
    }
}*/