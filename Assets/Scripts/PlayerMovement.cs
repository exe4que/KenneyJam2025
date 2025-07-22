using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace KenneyJam2025
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference _movement;
        [SerializeField] private InputActionReference _mouseLook;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] [Range(0, 20)] private float _speed = 5f;

        private float _speedMultiplier = 1f;
        private Vector2 _moveInput;
        private Vector3 _mouse3DPosition;

        private Vector3 _forward;
        private Vector3 _right;
        private Camera _camera;
        private Ray _ray;
        private float _runAnimationParameter = 0.0f;
        private float _runAnimationFillDuration = 0.3f;

        private void OnEnable()
        {
            GlobalEvents.PlayerDied += OnPlayerDied;
        }
        
        private void OnDisable()
        {
            GlobalEvents.PlayerDied -= OnPlayerDied;
        }
        
        private void OnPlayerDied()
        {
            this.DelayedCallInSeconds(() =>
            {
                Vector3 randomPosition = transform.position + Random.insideUnitSphere;
                _rigidbody.AddExplosionForce(10f, randomPosition, 5f, 1f, ForceMode.Impulse);
            }, 0.25f);
        }

        private void Start()
        {
            _camera = GameManager.Instance.MainCamera;
        }

        private void Update()
        {
            if (GameManager.Instance.GameOver) return;
            _moveInput = _movement.action.ReadValue<Vector2>();
            
            // handle run animation parameter
            if (_moveInput.magnitude > 0.1f)
            {
                _runAnimationParameter = Mathf.MoveTowards(_runAnimationParameter, 1.0f, Time.deltaTime / _runAnimationFillDuration);
            }
            else
            {
                _runAnimationParameter = Mathf.MoveTowards(_runAnimationParameter, 0.0f, Time.deltaTime / _runAnimationFillDuration);
            }
            _animator.SetFloat("Run", _runAnimationParameter);
            
            if (_mouseLook != null && _mouseLook.action != null)
            {
                Vector2 mousePosition = _mouseLook.action.ReadValue<Vector2>();
                if (_camera != null)
                {
                    _ray = _camera.ScreenPointToRay(mousePosition);
                    Plane plane = new Plane(Vector3.up, this.transform.position);
                    if (plane.Raycast(_ray, out float enter))
                    {
                        _mouse3DPosition = _ray.GetPoint(enter);
                    }
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (GameManager.Instance.GameOver) return;
            Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
            if (move.magnitude > 1f)
            {
                move.Normalize();
            }
            var camera = GameManager.Instance.MainCamera;
            _right = Vector3.Cross(this.transform.up, camera.transform.up);
            _forward = Vector3.Cross(_right, this.transform.up);
            
            Vector3 moveDirection = _right * move.x + _forward * move.z;
            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (_speed * _speedMultiplier * Time.fixedDeltaTime));
            // Rotate the player to face the mouse position
            if (_mouse3DPosition != Vector3.zero)
            {
                Vector3 directionToMouse = (_mouse3DPosition - _rigidbody.position).normalized;
                _rigidbody.rotation = Quaternion.LookRotation(directionToMouse, Vector3.up);
            }
            //Debug.Log($"Speed: {moveDirection.magnitude:F2} | IsShooting: {_animator.GetBool("IsShooting")}");
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = multiplier;
        }

        private void OnDrawGizmos()
        {
            if (_rigidbody == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_rigidbody.position, _rigidbody.position + _right * 2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_rigidbody.position, _rigidbody.position + _forward * 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_mouse3DPosition, 0.1f);
            Gizmos.DrawRay(_ray.origin, _ray.direction * 100f);
        }
    }
}
