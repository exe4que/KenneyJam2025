using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2025
{
    public class PlayerShooting : MonoBehaviour, IShooter
    {
        public InputActionReference ShootAction;
        [ReadOnly] public Gun EquipedGun;
        private void Update()
        {
            if (ShootAction.action.WasPressedThisFrame())
            {
                StartShooting();
            }
            else if (ShootAction.action.WasReleasedThisFrame())
            {
                StopShooting();
            }
        }

        public void StartShooting()
        {
            EquipedGun.StartShooting();
        }
        
        public void StopShooting()
        {
            EquipedGun.StopShooting();
        }

        public void OnSomethingDamaged(IDamageable target, float damage)
        {
            
        }
    }
}