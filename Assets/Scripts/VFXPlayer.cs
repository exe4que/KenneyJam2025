using UnityEngine;
using KenneyJam2025;
using System;
public class VFXPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] bool _isTriggeredByDeath;
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem == null)
        {
            Debug.LogError("No particle system found in " + this + " VFX Player");
        }
    }
    void OnEnable()
    {
        GlobalEvents.GunUpgraded += OnGunUpgrade;
        GlobalEvents.PlayerDied += OnPlayerDeath;
        //GlobalEvents.ShotFired += OnShotFired;

    }

    private void OnPlayerDeath()
    {
        if (!_isTriggeredByDeath) return;
        PlayVFX();
    }

    private void OnGunUpgrade(int obj)
    {
        if (!_isTriggeredByDeath) return;
        PlayVFX();
    }

    private void PlayVFX()
    {
        if (_particleSystem == null) return;
        _particleSystem.Play(true);
    }

    private void OnShotFired(IShooter shooter)
    {
        PlayVFX();
    }
    void OnDisable()
    {
        GlobalEvents.GunUpgraded -= OnGunUpgrade;
        GlobalEvents.PlayerDied -= OnPlayerDeath;
        GlobalEvents.ShotFired -= OnShotFired;
    }

}
