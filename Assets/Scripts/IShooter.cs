namespace KenneyJam2025
{
    public interface IShooter
    {
        void StartShooting();
        void StopShooting();
        void OnSomethingDamaged(IDamageable target, float damage);
    }
}