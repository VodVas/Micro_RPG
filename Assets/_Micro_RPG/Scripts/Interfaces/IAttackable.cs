public interface IAttackable : IClickable
{
    bool IsAlive { get; }
    void TakeDamage(float damage);
}
