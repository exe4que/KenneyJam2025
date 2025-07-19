namespace KenneyJam2025
{
    public interface IDamageable
    {
        void OnDamage(float damage, IShooter shooter);
        void OnDeath(IShooter shooter);
        void OnHeal(float heal);
    }
}