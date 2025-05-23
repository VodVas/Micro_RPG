using UnityEngine;

public sealed class AttackSystem
{
    private readonly float _cooldown;
    private float _lastAttackTime;

    public AttackSystem(float cooldown) => _cooldown = cooldown;

    public bool TryAttack()
    {
        if (Time.time < _lastAttackTime + _cooldown) return false;
        Debug.Log("TryAttack()");
        _lastAttackTime = Time.time;
        return true;
    }
}