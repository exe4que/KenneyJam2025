using System;
using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2025
{
    public class ShootingManager : Singleton<ShootingManager>
    {
        [Range(0,200)] public float BulletSpeed = 50f;
        [Range(0,200)] public float SpecialBulletSpeed = 25f;
        public float SpecialBulletDamage = 100f;
        private List<Bullet> _bullets = new List<Bullet>();
        
        public void Shoot(Ray trajectory, float maxRange, float damage, IShooter shooter, string bulletName, bool special)
        {
            PhysicalBullet physicalBullet = PoolManager.Instance.GetInstance(bulletName).GetComponent<PhysicalBullet>();
            Bullet bullet = new Bullet
            {
                Trajectory = trajectory,
                LastPosition = 0f,
                Position = 0f,
                MaxRange = maxRange,
                Damage = damage,
                Shooter = shooter,
                PhysicalBullet = physicalBullet,
                IsSpecial = special
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
                float speed = bullet.IsSpecial ? SpecialBulletSpeed : BulletSpeed;
                bullet.Position += speed * Time.fixedDeltaTime;
                bool reachedMaxRange = bullet.Position >= bullet.MaxRange;
                if (reachedMaxRange)
                {
                    bullet.Position = bullet.MaxRange;
                }

                Ray ray = new Ray(bullet.Trajectory.origin + bullet.Trajectory.direction * bullet.LastPosition, bullet.Trajectory.direction);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, bullet.Position - bullet.LastPosition))
                {
                    IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
                    if (damageable != null && damageable.Name != bullet.Shooter.Name)
                    {
                        damageable.OnDamage(bullet.Damage, bullet.Shooter);
                        if (bullet.Shooter != null)
                        {
                            bullet.Shooter.OnSomethingDamaged(damageable, bullet.Damage);
                            if (bullet.IsSpecial)
                            {
                                GameManager.Instance.OnSpecialBulletHit();
                            }
                        }
                    }

                    bullet.PhysicalBullet.ReturnToPool(true, Vector3.Distance(bullet.Trajectory.origin, hitInfo.point));
                    _bullets.Remove(bullet);
                    GameManager.Instance.OnSpecialBulletMiss();
                }
                
                if (reachedMaxRange)
                {
                    _bullets[i].PhysicalBullet.ReturnToPool(false, 0);
                    _bullets.RemoveAt(i);
                }
            }
        }
    }
    public class Bullet
    {
        public Ray Trajectory;
        public float Position;
        public float LastPosition;
        public float MaxRange;
        public float Damage;
        public IShooter Shooter;
        public PhysicalBullet PhysicalBullet;
        public bool IsSpecial;
    }
}