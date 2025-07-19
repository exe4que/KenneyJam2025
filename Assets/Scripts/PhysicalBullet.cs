using System;
using UnityEngine;

namespace KenneyJam2025
{
    public class PhysicalBullet : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        private Bullet _bullet;
        
        public void Init(Bullet bullet)
        {
            _bullet = bullet;
            _lineRenderer.positionCount = 2;
            _lineRenderer.widthMultiplier = 1f;
            _lineRenderer.startWidth = 0.25f;
            _lineRenderer.endWidth = 0.25f;
            
            _lineRenderer.transform.position = _bullet.Trajectory.origin;
            _lineRenderer.transform.rotation = Quaternion.LookRotation(_bullet.Trajectory.direction, Vector3.up);
            Debug.Log("Bullet fired!");
        }

        private void Update()
        {
            if (_bullet.Position >= _bullet.MaxRange) return;

            Vector3 startPosition = new Vector3(0, 0, _bullet.LastPosition);
            Vector3 endPosition = new Vector3(0, 0, _bullet.Position);

            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);
        }
        
        public bool ReturnToPool(bool impacted)
        {
            Debug.Log($"Returning bullet to pool. Impacted: {impacted}");
            _bullet = null;
            PoolManager.Instance.ReturnInstance(this.gameObject);
            return true;
        }
    }
}