using System;
using UnityEngine;

namespace KenneyJam2025
{
    public class PistolGun : Gun
    {
        private float _lastFireTime;
        private bool _isShooting;
        private IShooter _shooter;

        public override void Init(IShooter shooter)
        {
            _shooter = shooter;
        }

        public override void Equip()
        {
            _lastFireTime = Time.time - FireRate - 1f;
            _isShooting = false;
            Debug.Log("PistolGun equipped.");
        }

        public override void StartShooting()
        {
            _isShooting = true;
            //Debug.Log("Started shooting with PistolGun");
        }

        public override void StopShooting()
        {
            _isShooting = false;
            //Debug.Log("Stopped shooting with PistolGun");
        }

        public override void ShootSpecialBullet()
        {
            Ray shootRay = new Ray(this.transform.position, this.transform.forward);
            // Call the shooting manager to handle the bullet logic
            ShootingManager.Instance.Shoot(shootRay, Range, ShootingManager.Instance.SpecialBulletDamage, _shooter, "BulletSpecialPrefab", true);
        }

        private void FixedUpdate()
        {
            if (Time.time - _lastFireTime >= 1 / FireRate)
            {
                // Logic to shoot the gun
                _lastFireTime = Time.time;
                // Here you would typically instantiate a bullet or perform a raycast
                if (_isShooting)
                {
                    Ray shootRay = new Ray(this.transform.position, this.transform.forward);
                    // Call the shooting manager to handle the bullet logic
                    ShootingManager.Instance.Shoot(shootRay, Range, Damage, _shooter, "BulletPrefab", false);
                }
            }
        }
    }
}