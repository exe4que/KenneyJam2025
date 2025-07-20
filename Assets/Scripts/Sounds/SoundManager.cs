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

     void Start()
     {
          // Subscribes to events from global events manager
          GlobalEvents.ShotFired += OnShotFired;
          GlobalEvents.SomethingDamaged += OnSomethingDamaged;
          GlobalEvents.PlayerDied += OnPlayerDied;

          // If it has, it reproduces music
          if (_sceneMusic != null)
          {
               _musicSource.clip = _sceneMusic;
               _musicSource.Play();
          }
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


    void OnDestroy()
    {
          //Unsuscribes from the events 
          GlobalEvents.ShotFired -= OnShotFired;
          GlobalEvents.SomethingDamaged -= OnSomethingDamaged;
          GlobalEvents.PlayerDied -= OnPlayerDied;
    }


}

