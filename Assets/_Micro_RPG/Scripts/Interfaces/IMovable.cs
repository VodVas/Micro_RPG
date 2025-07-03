using UnityEngine;

public interface IMovable
{
    Vector3 Velocity { get; }
    Quaternion TargetRotation { get; }
    bool IsRunning { get; }

    void CalculateMovement(Vector2 input, float deltaTime);
    void CalculateRotation(Vector2 input);
    void ApplyGravity(float deltaTime, bool isGrounded);
}
