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
            
        }

        public override void StartShooting()
        {
            _isShooting = true;
        }

        public override void StopShooting()
        {
            _isShooting = false;
        }
        
        private void FixedUpdate()
        {
            if (Time.time - _lastFireTime >= FireRate)
            {
                // Logic to shoot the gun
                _lastFireTime = Time.time;
                // Here you would typically instantiate a bullet or perform a raycast
            }
        }
    }
}