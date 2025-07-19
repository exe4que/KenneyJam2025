using System;
using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2025
{
    public class ShootingManager : Singleton<ShootingManager>
    {
        private List<Bullet> _bullets = new List<Bullet>();
        
        public void Shoot(Ray trajectory, float maxRange, IShooter shooter)
        {
            PhysicalBullet physicalBullet = PoolManager.Instance.GetInstance("BulletPrefab").GetComponent<PhysicalBullet>();
            Bullet bullet = new Bullet
            {
                Trajectory = trajectory,
                LastPosition = 0f,
                Position = 0f,
                MaxRange = maxRange,
                Shooter = shooter,
                PhysicalBullet = physicalBullet
            };
            physicalBullet.Init(bullet);
            _bullets.Add(bullet);
        }

        private void FixedUpdate()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];
                bullet.LastPosition = bullet.Position;
                bullet.Position += bullet.Speed * Time.fixedDeltaTime;
                bool reachedMaxRange = bullet.Position >= bullet.MaxRange;
                if (reachedMaxRange)
                {
                    bullet.Position = bullet.MaxRange;
                }

                Ray ray = new Ray(bullet.Trajectory.origin + bullet.Trajectory.direction * bullet.LastPosition, bullet.Trajectory.direction);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, bullet.Position - bullet.LastPosition))
                {
                    IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        float damage = bullet.Position - bullet.LastPosition; // Example damage calculation
                        damageable.OnDamage(damage, bullet.Shooter);
                        if (bullet.Shooter != null)
                        {
                            bullet.Shooter.OnSomethingDamaged(damageable, damage);
                        }
                    }

                    _bullets[i].PhysicalBullet.ReturnToPool();
                    _bullets.RemoveAt(i);
                }
                
                if (reachedMaxRange)
                {
                    _bullets[i].PhysicalBullet.ReturnToPool();
                    _bullets.RemoveAt(i);
                }
            }
        }
    }
    public class Bullet
    {
        public float Speed;
        public Ray Trajectory;
        public float Position;
        public float LastPosition;
        public float MaxRange;
        public IShooter Shooter;
        public PhysicalBullet PhysicalBullet;
    }
}