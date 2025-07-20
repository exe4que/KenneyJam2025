using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2025
{
    public class PlayerShooting : MonoBehaviour, IShooter
    {
        public InputActionReference ShootAction;
        [SerializeField] private Gun _equipedGun;

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private float _shootingSpeedMultiplier = 0.4f;

        
        [SerializeField] private Gun[] _guns;
        [SerializeField] private GameObject[] _visualGuns;

        [SerializeField] private InputActionReference _Gun0Action;
        [SerializeField] private InputActionReference _Gun1Action;
        [SerializeField] private InputActionReference _Gun2Action;


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

            // only for testing purposes, comment out in production
            if (_Gun0Action != null && _Gun0Action.action.WasPressedThisFrame())
            {
                EquipGun(0);
            }
            else if (_Gun1Action != null && _Gun1Action.action.WasPressedThisFrame())
            {
                EquipGun(1);
            }
            else if (_Gun2Action != null && _Gun2Action.action.WasPressedThisFrame())
            {
                EquipGun(2);
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
        public float ImprecisionNoise => 0f;

        public void EquipGun(int index)
        {
            if (index < 0 || index >= _guns.Length)
            {
                Debug.LogError($"Invalid gun index: {index}. Cannot equip gun.");
                return;
            }

            if (_equipedGun != null)
            {
                _equipedGun.StopShooting();
            }

            _equipedGun = _guns[index];
            _equipedGun.Equip(); 
            
            for (int i = 0; i < _visualGuns.Length; i++)
            {
                _visualGuns[i].SetActive(i == index);
            }
            Debug.Log($"Equipped gun: {_equipedGun.name}");
        }

        public void StartShooting()
        {
            _equipedGun.StartShooting();
            _animator.SetBool("isShooting", true);
            if (_movement != null)
            {
                _movement.SetSpeedMultiplier(_shootingSpeedMultiplier);
            }
        }
        
        public void StopShooting()
        {
            _equipedGun.StopShooting();
            if (_movement != null)
                _movement.SetSpeedMultiplier(1f);

            _animator.SetBool("isShooting", false); 
            //Debug.Log("Dej� de disparar");
        }

        public void OnSomethingDamaged(IDamageable target, float damage)
        {
            
        }
    }
}