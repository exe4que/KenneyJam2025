namespace KenneyJam2025
{
    public interface IDamageable
    {
        string Name { get; }
        void OnDamage(float damage, IShooter shooter);
        void OnHeal(float heal);
    }
}