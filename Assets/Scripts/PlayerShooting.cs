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
            if (Input.GetKeyDown(KeyCode.F1))
            {
                EquipGun(0);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                EquipGun(1);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
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