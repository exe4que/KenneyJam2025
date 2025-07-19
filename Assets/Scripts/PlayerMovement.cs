using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2025
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference _movement;
        [SerializeField] private InputActionReference _mouseLook;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] [Range(0, 20)] private float _speed = 5f;
        private Vector2 _moveInput;
        private Vector3 _mouse3DPosition;

        private Vector3 _forward;
        private Vector3 _right;
        private Camera _camera;
        private Ray _ray;

        private void Start()
        {
            _camera = GameManager.Instance.MainCamera;
        }

        private void Update()
        {
            _moveInput = _movement.action.ReadValue<Vector2>();
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
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (_speed * Time.fixedDeltaTime));
            // Rotate the player to face the mouse position
            if (_mouse3DPosition != Vector3.zero)
            {
                Vector3 directionToMouse = (_mouse3DPosition - _rigidbody.position).normalized;
                _rigidbody.rotation = Quaternion.LookRotation(directionToMouse, Vector3.up);
            }
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
