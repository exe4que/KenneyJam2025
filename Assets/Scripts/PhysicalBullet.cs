using System;
using UnityEngine;

namespace KenneyJam2025
{
    public class PhysicalBullet : MonoBehaviour
    {
        [SerializeField] private float Length = 1f;
        [SerializeField] private float StartThickness = 0.25f;
        [SerializeField] private float EndThickness = 0f;
        [SerializeField] private Color StartColor = Color.red;
        [SerializeField] private Color EndColor = Color.red;
        [SerializeField] private LineRenderer _lineRenderer;
        private Bullet _bullet;
        private bool _impacted;
        private bool _active = true;
        private float _impactDistance;
        
        public void Init(Bullet bullet)
        {
            _impacted = false;
            _active = true;
            _bullet = bullet;
            _lineRenderer.positionCount = 2;
            _lineRenderer.widthMultiplier = 1f;
            _lineRenderer.startWidth = StartThickness;
            _lineRenderer.endWidth = EndThickness;
            _lineRenderer.startColor = StartColor;
            _lineRenderer.endColor = EndColor;
            
            _lineRenderer.transform.position = _bullet.Trajectory.origin;
            _lineRenderer.transform.rotation = Quaternion.LookRotation(_bullet.Trajectory.direction, Vector3.up);
            Debug.Log("Bullet fired!");
            GlobalEvents.ShotFired?.Invoke(_bullet.Shooter);
        }

        private void Update()
        {
            if (_bullet == null) return;

            Vector3 startPosition;
            Vector3 endPosition;

            if (_active)
            {
                float minLength = Mathf.Min(Length, _bullet.Position);
                startPosition = new Vector3(0, 0, _bullet.Position - minLength);
                endPosition = new Vector3(0, 0, _bullet.Position);
            }
            else
            {
                _bullet.Position += ShootingManager.Instance.BulletSpeed * Time.deltaTime;
                if (_impacted)
                {
                    startPosition = new Vector3(0, 0, Mathf.Min(_impactDistance, _bullet.Position - Length));
                    endPosition = new Vector3(0, 0, _impactDistance);
                }
                else
                {
                    float minLength = Mathf.Min(Length, _bullet.Position);
                    startPosition = new Vector3(0, 0, _bullet.Position - minLength);
                    endPosition = new Vector3(0, 0, _bullet.Position);
                }
            }

            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);
            
        }
        
        public void ReturnToPool(bool impacted, float impactDistance)
        {
            _active = false;
            _impacted = impacted;
            _impactDistance = impactDistance;
            this.DelayedCallInSeconds(() =>
            {
                Debug.Log($"Returning bullet to pool. Impacted: {impacted}");
                _bullet = null;
                PoolManager.Instance.ReturnInstance(this.gameObject);
            }, 1);
        }
    }
}