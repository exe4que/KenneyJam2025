using UnityEngine;

namespace KenneyJam2025
{
    public interface IShooter
    {
        string Name { get; }
        Vector3 Position { get; }
        GameObject GameObject { get; }
        float ImprecisionNoise { get; }
        int WeaponIndex { get; set; }
        void EquipGun(int index);
        void StartShooting();
        void StopShooting();
        void OnSomethingDamaged(IDamageable target, float damage);
    }
}