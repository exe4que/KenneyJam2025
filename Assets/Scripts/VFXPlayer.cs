using UnityEngine;
using KenneyJam2025;
using System;
public class VFXPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    void OnEnable()
    {
        GlobalEvents.GunUpgraded += OnGunUpgrade;
    }

    private void OnGunUpgrade(int obj)
    {
        if (i != GetComponent<IShooter>())
        {
            return; // Only play VFX for the player
        }
        PlayVFX();
    }

    private void PlayVFX()
    {
        if (_particleSystem == null) return;
        _particleSystem.Play(true);
    }

    void OnDisable()
    {
        GlobalEvents.GunUpgraded -= OnGunUpgrade;
    }

}
