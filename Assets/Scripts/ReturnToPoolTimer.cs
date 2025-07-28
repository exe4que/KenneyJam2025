using UnityEngine;

namespace KenneyJam2025
{
    public class ReturnToPoolTimer : MonoBehaviour
    {
        [SerializeField] private float _returnDelay = 2f;

        private void OnEnable()
        {
            Invoke(nameof(ReturnToPool), _returnDelay);
        }

        private void ReturnToPool()
        {
            PoolManager.Instance.ReturnInstance(gameObject);
        }
        
    }
}