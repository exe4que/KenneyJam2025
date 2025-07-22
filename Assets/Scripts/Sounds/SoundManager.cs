using System;
using KenneyJam2025;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
     private AudioSource _audioFXSource;   // SoundFX with random volume
     private AudioSource _musicSource;   // Music

     [Header("Music Sounds")]

     [SerializeField] private AudioClip _sceneMusic; //each scene hass its own music 


     //sound effects per event
     [Header("Events Sounds")]
     public SoundFX ShotFired;
     public SoundFX SomethingDamaged;
     public SoundFX PlayerDied;
     public SoundFX GunUpgraded;


     public static SoundManager Instance => _instance;
     private static SoundManager _instance;
     void Awake()
     {
          if (Instance != null && Instance != this)
          {
               Destroy(gameObject);
               return;
          }

          _instance = this;


          var audioSources = GetComponents<AudioSource>(); //Sets an array of sources 

          if (audioSources.Length < 2)
          {
               Debug.LogError("A sound source is missing ");
          }

          _audioFXSource = audioSources[0];
          _musicSource = audioSources[1];
     }

     private void OnEnable()
     {
          GlobalEvents.ShotFired += OnShotFired;
          GlobalEvents.SomethingDamaged += OnSomethingDamaged;
          GlobalEvents.PlayerDied += OnPlayerDied;
          GlobalEvents.GunUpgraded += OnGunUpgraded;
     }

     private void OnDisable()
     {
          GlobalEvents.ShotFired -= OnShotFired;
          GlobalEvents.SomethingDamaged -= OnSomethingDamaged;
          GlobalEvents.PlayerDied -= OnPlayerDied;
          GlobalEvents.GunUpgraded -= OnGunUpgraded;
     }

     void Start()
     {
          // If it has, it reproduces music
          if (_sceneMusic != null)
          {
               _musicSource.clip = _sceneMusic;
               _musicSource.Play();
               _musicSource.loop = true;
          }
     }

    private void OnGunUpgraded(IShooter i, int obj)
    {
          obj = 3;
          PlayClip(GunUpgraded);
    }

    private void OnShotFired(IShooter shooter)
     {
          PlayClip(ShotFired);
     }

     private void OnSomethingDamaged(IShooter shooter, IDamageable target, float damage)
     {
          PlayClip(SomethingDamaged);
     }

     private void OnPlayerDied()
     {
          PlayClip(PlayerDied);
     }

     private void PlayClip(SoundFX fx)
     {
          if (fx != null && fx.Clip != null)
          {
               _audioFXSource.PlayOneShot(fx.Clip, fx.GetRandomVolume());
          }
     }
}

