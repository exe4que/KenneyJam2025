using UnityEngine;

namespace KenneyJam2025
{
    public interface IShooter
    {
        void EquipGun(int index);
        void StartShooting();
        void StopShooting();
        void OnSomethingDamaged(IDamageable target, float damage);
        Ray GetShootRay();
    }
}