using System;
using UnityEngine;

namespace KenneyJam2025
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public float currentHealth = 0f;

        public string Name
        {
            get
            {
                return gameObject.name;
            }
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (this.transform.position.y < -6f)
            {
                // If the player falls below a certain height, they die
                Die(null);
            }
        }

        public void OnDamage(float damage, IShooter shooter)
        {
            currentHealth -= damage;
            Debug.Log($"Player damaged by {damage}. Current health: {currentHealth}");
            GlobalEvents.SomethingDamaged?.Invoke(shooter, this, damage);
            if (currentHealth <= 0)
            {
                Die(shooter);
            }
        }

        public void Die(IShooter shooter)
        {
            currentHealth = 0f;
            GlobalEvents.PlayerDied?.Invoke();
            Debug.Log("Player has died.");
            
            var vfxGo = PoolManager.Instance.GetInstance("vfx_MineExplosion");
            vfxGo.transform.position = transform.position;
        }

        public void OnHeal(float heal)
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth; // Ensure health does not exceed max health
            }
            Debug.Log($"Player healed. Current health: {currentHealth}");
        }
    }
}