using UnityEngine;

namespace KenneyJam2025
{
    public abstract class Gun : MonoBehaviour
    {
        public float Damage = 10f;
        public float FireRate = 1f;
        public float Range = 20f;
        public abstract void Init(IShooter shooter);
        public abstract void Equip();
        public abstract void StartShooting();
        public abstract void StopShooting();
        public abstract void ShootSpecialBullet();
    }
}