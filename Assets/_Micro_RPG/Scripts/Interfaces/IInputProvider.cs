using System;
using UnityEngine;

public interface IInputProvider
{
    Vector2 GetMovement();
    bool GetAttack();
    void Tick();
    //void Enable();

    //event Action<Vector2> OnMovement;
    //event Action OnAttack;
}