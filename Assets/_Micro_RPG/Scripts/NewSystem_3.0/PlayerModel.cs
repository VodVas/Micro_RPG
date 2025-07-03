using UnityEngine;

public class PlayerModel : IMovable, IAttacker
{
    private readonly MovementSystem _movement;
    private readonly AttackSystem _attack;

    public Vector3 Velocity => _movement.Velocity;
    public Quaternion TargetRotation => _movement.TargetRotation;
    public bool IsRunning => _movement.IsRunning;

    public PlayerModel(MovementSystem movement, AttackSystem attack)
    {
        _movement = movement;
        _attack = attack;
    }

    public void CalculateMovement(Vector2 input, float deltaTime)
        => _movement.CalculateMovement(input, deltaTime);

    public void CalculateRotation(Vector2 input)
        => _movement.CalculateRotation(input);

    public void ApplyGravity(float deltaTime, bool isGrounded)
        => _movement.ApplyGravity(deltaTime, isGrounded);

    public bool TryAttack()
        => _attack.TryAttack();
}

/*using UnityEngine;

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
}*/