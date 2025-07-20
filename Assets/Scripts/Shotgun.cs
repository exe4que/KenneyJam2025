using System;
using UnityEngine;

namespace KenneyJam2025
{
    public class Shotgun : Gun
    {
        [SerializeField] private float AngleSpread = 40f; // Angle spread for shotgun pellets
        [SerializeField] private int PelletCount = 5; // Number of pellets fired per shot
        [SerializeField] private float RecoilForce = 5f; // Recoil force applied to the shooter
        
        private float _lastFireTime;
        private bool _isShooting;
        private IShooter _shooter;
        private Rigidbody _rb;

        public override void Init(IShooter shooter)
        {
            _shooter = shooter;
            _rb = _shooter.GameObject.GetComponent<Rigidbody>();
        }

        public override void Equip()
        {
            _lastFireTime = Time.time - FireRate - 1f;
            _isShooting = false;
            Debug.Log("Shotgun equipped.");
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
                    //Ray shootRay = new Ray(this.transform.position, this.transform.forward);
                    // Call the shooting manager to handle the bullet logic
                    //ShootingManager.Instance.Shoot(shootRay, Range, Damage, _shooter, "BulletPrefab", false);
                    for (int i = 0; i < PelletCount; i++)
                    {
                        // Calculate the angle for each pellet
                        float angle = UnityEngine.Random.Range(-AngleSpread / 2, AngleSpread / 2);
                        Quaternion rotation = Quaternion.Euler(0, angle, 0);
                        Vector3 direction = rotation * transform.forward;

                        Ray shootRay = new Ray(transform.position, direction);
                        // Call the shooting manager to handle the bullet logic
                        ShootingManager.Instance.Shoot(shootRay, Range, Damage / PelletCount, _shooter, "ShotgunBulletPrefab", false);
                    }
                    
                    //apply recoil force to the shooter
                    if (_rb != null)
                    {
                        Vector3 recoilDirection = -transform.forward * RecoilForce;
                        _rb.AddForce(recoilDirection, ForceMode.Impulse);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the angle spread for the shotgun in the editor
            Gizmos.color = Color.yellow;
            Vector3 forward = transform.forward;
            Quaternion leftRotation = Quaternion.Euler(0, -AngleSpread / 2, 0);
            Quaternion rightRotation = Quaternion.Euler(0, AngleSpread / 2, 0);
            
            Gizmos.DrawRay(transform.position, leftRotation * forward * Range);
            Gizmos.DrawRay(transform.position, rightRotation * forward * Range);
        }
    }
}