using Unity.Cinemachine;
using UnityEngine;

namespace KenneyJam2025
{
    public class BombExplosionShake : MonoBehaviour
    {
        [SerializeField] private float _shakeDelay = 0.25f;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        
        private void Start()
        {
            this.DelayedCallInSeconds(() =>
            {
                _impulseSource.GenerateImpulse();
            }, _shakeDelay);
        }
        
        
    }
}