using System;
using UnityEngine;

namespace KenneyJam2025
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        
        private float currentHealth;
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
            GlobalEvents.PlayerDied?.Invoke();
            Debug.Log("Player has died.");
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