using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Micro_RPG/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField, Range(0f, 20f)] public float MoveSpeed { get; private set; } = 5f;
    [field: SerializeField, Range(-20f, 0f)] public float Gravity { get; private set; } = -9.81f;
    [field: SerializeField, Range(0f, 5f)] public float GroundOffset { get; private set; } = 0.5f;
    [field: SerializeField, Range(0, 1000)] public float RotationSpeed { get; private set; } = 450f;

    [field: SerializeField, Range(0.1f, 5f)] public float AttackCooldown { get; private set; } = 0.5f;
    [field: SerializeField, Range(0, 100)] public int AttackDamage { get; private set; } = 10;
}