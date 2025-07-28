using UnityEngine;

namespace KenneyJam2025.UI
{
    public class UIFaceCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = GameManager.Instance.MainCamera;
        }

        private void LateUpdate()
        {
            if (_mainCamera != null)
            {
                transform.LookAt(_mainCamera.transform);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }
    }
}