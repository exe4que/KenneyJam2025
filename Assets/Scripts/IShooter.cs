using UnityEngine;

namespace KenneyJam2025
{
    public interface IShooter
    {
        string Name { get; }
        void EquipGun(int index);
        void StartShooting();
        void StopShooting();
        void OnSomethingDamaged(IDamageable target, float damage);
        Ray GetShootRay();
    }
}