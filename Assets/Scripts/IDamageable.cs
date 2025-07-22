namespace KenneyJam2025
{
    public interface IDamageable
    {
        string Name { get; }
        float MaxHealth { get; }
        float CurrentHealth { get; }
        void OnDamage(float damage, IShooter shooter);
        void OnHeal(float heal);
    }
}