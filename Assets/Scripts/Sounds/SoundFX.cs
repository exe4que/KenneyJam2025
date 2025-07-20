using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundFX", menuName = "Audio/Sound Effect")]
public class SoundFX : ScriptableObject
{
    public AudioClip Clip;

    //Public so we can access it in the sound manager
    [Range(0f, 1f)] public float MinVolume = 0.8f;
    [Range(0f, 1f)] public float MaxVolume = 1f;

    public float GetRandomVolume()
    {
        return Random.Range(MinVolume, MaxVolume);
    }
}