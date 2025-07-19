using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2025
{
    public class PlayerShooting : MonoBehaviour, IShooter
    {
        public InputActionReference ShootAction;
        public Gun EquipedGun;
        
        public Gun[] Guns;
        
        private void Awake()
        {
            if (Guns.Length == 0)
            {
                Debug.LogError("No guns assigned to PlayerShooting.");
                return;
            }

            for (int i = 0; i < Guns.Length; i++)
            {
                Guns[i].Init(this);
            }
            EquipGun(0);
        }
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

        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        public void EquipGun(int index)
        {
            if (index < 0 || index >= Guns.Length)
            {
                Debug.LogError($"Invalid gun index: {index}. Cannot equip gun.");
                return;
            }

            if (EquipedGun != null)
            {
                EquipedGun.StopShooting(); // Stop the current gun before switching
            }

            EquipedGun = Guns[index];
            EquipedGun.Equip(); // Equip the new gun
            Debug.Log($"Equipped gun: {EquipedGun.name}");
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