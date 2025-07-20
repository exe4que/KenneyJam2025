using UnityEngine;
using UnityEngine.UI;
using KenneyJam2025;
using System;
public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _healthSlider;

    [SerializeField] private PlayerHealth playerHealth;

    private void Start()
    {
        _healthSlider = GetComponent<Slider>();

        if (_healthSlider == null)
        {
            Debug.LogError("No Slider Found in health bar");
        }

        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("No player health component found");
        }
        else
        {
            Debug.Log("Player found");
        }

        GlobalEvents.SomethingDamaged += OnPlayerDamage;

        InitializeHealthBar();//Start with right value
    }

    private void OnPlayerDamage(IShooter shooter, IDamageable damageable, float damage)
    {
        PlayerHealth playerHealth = damageable as PlayerHealth;
        if (playerHealth != null)
        {
            //f it has a player health component
            UpdateHealthBar();
        }
    }

    private void OnDestroy()
    {
        GlobalEvents.SomethingDamaged -= OnPlayerDamage;
    }

    private void UpdateHealthBar()
    {
        if (playerHealth == null) return;

        float current = playerHealth.currentHealth;

        if (_healthSlider != null)
        {
            _healthSlider.value = current;
        }
    }

    private void InitializeHealthBar()
    {

        if (_healthSlider != null)
        {
            _healthSlider.value = 100f;
        }
     
    }


    }
