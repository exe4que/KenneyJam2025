using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KenneyJam2025
{
    public class GameManager : Singleton<GameManager>
    {
        public Camera MainCamera;
        public LevelSettings[] LevelSettings;
        public InputActionReference UpdradeAction;
        
        private int _weaponIndex = 0;
        private float _timeLeft;
        private bool _levelActive = false;
        private int _currentLevelIndex = 0;
        private bool _upgrade1AlreadyActivated = false;
        private bool _upgrade2AlreadyActivated = false;
        private bool _upgrade3AlreadyActivated = false;
        private void Start()
        {
            StartLevel();
        }

        public void StartLevel()
        {
            _levelActive = true;
            _timeLeft = LevelSettings[_currentLevelIndex].TimerDuration;
        }

        private void Update()
        {
            if (!_levelActive) return;

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0f)
            {
                _timeLeft  = LevelSettings[_currentLevelIndex].TimerDuration;
            }
            
            float timeLeftPercentage = _timeLeft / LevelSettings[_currentLevelIndex].TimerDuration;
            if (UpdradeAction != null && UpdradeAction.action.WasPressedThisFrame())
            {
                Debug.Log($"Time Left Percentage: {timeLeftPercentage}");
                if (timeLeftPercentage >= LevelSettings[_currentLevelIndex].UpgradeWindow1Range.x &&
                    timeLeftPercentage <= LevelSettings[_currentLevelIndex].UpgradeWindow1Range.y)
                {
                    if (_upgrade1AlreadyActivated) return; // Prevent multiple activations
                    Debug.Log("Upgrade Window 1 Activated");
                    GlobalEvents.UpgradeGunWindowActivated?.Invoke(0);
                    _upgrade1AlreadyActivated = true;
                }
                else if (timeLeftPercentage >= LevelSettings[_currentLevelIndex].UpgradeWindow2Range.x &&
                         timeLeftPercentage <= LevelSettings[_currentLevelIndex].UpgradeWindow2Range.y)
                {
                    if (_upgrade2AlreadyActivated) return; // Prevent multiple activations
                    Debug.Log("Upgrade Window 2 Activated");
                    GlobalEvents.UpgradeGunWindowActivated?.Invoke(1);
                    _upgrade2AlreadyActivated = true;
                }
                else if (timeLeftPercentage >= LevelSettings[_currentLevelIndex].UpgradeWindow3Range.x &&
                         timeLeftPercentage <= LevelSettings[_currentLevelIndex].UpgradeWindow3Range.y)
                {
                    if (_upgrade3AlreadyActivated) return; // Prevent multiple activations
                    Debug.Log("Upgrade Window 3 Activated");
                    GlobalEvents.UpgradeGunWindowActivated?.Invoke(2);
                    _upgrade3AlreadyActivated = true;
                }
            }
            
            if (timeLeftPercentage < LevelSettings[_currentLevelIndex].UpgradeWindow1Range.x)
            {
                _upgrade1AlreadyActivated = false;
            }
            if (timeLeftPercentage < LevelSettings[_currentLevelIndex].UpgradeWindow2Range.x)
            {
                _upgrade2AlreadyActivated = false;
            }
            if (timeLeftPercentage < LevelSettings[_currentLevelIndex].UpgradeWindow3Range.x)
            {
                _upgrade3AlreadyActivated = false;
            }
        }


        private void OnGUI()
        {
            if (!_levelActive) return;
            var settings = LevelSettings[_currentLevelIndex];
            // draw timer bar
            float timerPercentage = _timeLeft / LevelSettings[_currentLevelIndex].TimerDuration;
            GUI.Box(new Rect(10, 10, 200, 20), $"Time Left: {Mathf.CeilToInt(_timeLeft)}s");
            GUI.Box(new Rect(10, 40, 200 * timerPercentage, 20), GUIContent.none);
            
            // draw upgrade windows
            GUI.Box(new Rect(settings.UpgradeWindow1Range.x * 200f, 70, 
                (settings.UpgradeWindow1Range.y - settings.UpgradeWindow1Range.x) * 200f, 20), "1");
            GUI.Box(new Rect(settings.UpgradeWindow2Range.x * 200f, 70, 
                (settings.UpgradeWindow2Range.y - settings.UpgradeWindow2Range.x) * 200f, 20), "2");
            GUI.Box(new Rect(settings.UpgradeWindow3Range.x * 200f, 70, 
                (settings.UpgradeWindow3Range.y - settings.UpgradeWindow3Range.x) * 200f, 20), "3");
        }
    }

    [System.Serializable]
    public class LevelSettings
    {
        [Range(0f,60f)] public float TimerDuration = 30f;
        public Vector2 UpgradeWindow1Range = new Vector2(0.2f, 0.25f);
        public Vector2 UpgradeWindow2Range = new Vector2(0.4f, 0.45f);
        public Vector2 UpgradeWindow3Range = new Vector2(0.6f, 0.65f);
    }
}
