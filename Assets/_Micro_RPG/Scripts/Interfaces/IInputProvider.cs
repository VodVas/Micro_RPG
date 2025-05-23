using UnityEngine;

public interface IInputProvider
{
    Vector2 GetMovement();
    bool GetAttack();
    void Tick();
}