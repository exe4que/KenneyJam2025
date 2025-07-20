using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2025
{
    public class PlayerShooting : MonoBehaviour, IShooter
    {
        public InputActionReference ShootAction;
        [SerializeField] private Gun _equipedGun;
        
        [SerializeField] private Gun[] _guns;


        private void OnEnable()
        {
            GlobalEvents.UpgradeGunWindowActivated += OnUpgradeGunWindowActivated;
            GlobalEvents.GunUpgraded += OnGunUpgraded;
        }
        
        private void OnDisable()
        {
            GlobalEvents.UpgradeGunWindowActivated -= OnUpgradeGunWindowActivated;
            GlobalEvents.GunUpgraded -= OnGunUpgraded;
        }

        private void OnGunUpgraded(int index)
        {
            EquipGun(index);
            Debug.Log($"Gun upgraded to index: {index}".Color(Color.green));
        }


        private void OnUpgradeGunWindowActivated(int index)
        {
            _equipedGun.ShootSpecialBullet();
        }


        private void Start()
        {
            if (_guns.Length == 0)
            {
                Debug.LogError("No guns assigned to PlayerShooting.");
                return;
            }

            for (int i = 0; i < _guns.Length; i++)
            {
                _guns[i].Init(this);
            }
            EquipGun(0);
            
            ShootersManager.Instance.RegisterShooter(this);
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

        public Vector3 Position => transform.position;
        public GameObject GameObject => gameObject;

        public void EquipGun(int index)
        {
            if (index < 0 || index >= _guns.Length)
            {
                Debug.LogError($"Invalid gun index: {index}. Cannot equip gun.");
                return;
            }

            if (_equipedGun != null)
            {
                _equipedGun.StopShooting(); // Stop the current gun before switching
            }

            _equipedGun = _guns[index];
            _equipedGun.Equip(); // Equip the new gun
            Debug.Log($"Equipped gun: {_equipedGun.name}");
        }

        public void StartShooting()
        {
            _equipedGun.StartShooting();
        }
        
        public void StopShooting()
        {
            _equipedGun.StopShooting();
        }

        public void OnSomethingDamaged(IDamageable target, float damage)
        {
            
        }
    }
}