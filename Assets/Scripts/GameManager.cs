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
        public LevelSettings CurrentLevelSettings => LevelSettings[_currentLevelIndex];
        public int WeaponIndex => _weaponIndex;
        
        private int _weaponIndex = 0;
        private float _timeLeft;
        private bool _levelActive = false;
        private int _currentLevelIndex = 0;
        private bool _upgrade1Open = false;
        private bool _upgrade2Open = false;
        private bool _upgrade3Open = false;
        
        private bool _upgrade1AlreadyActivated = false;
        private bool _upgrade2AlreadyActivated = false;
        private bool _upgrade3AlreadyActivated = false;
        private int _pendingUpgrade = -1;
        private bool _gameOver = false;
        
        public bool GameOver => _gameOver;

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
            _levelActive = false;
            _upgrade1Open = false;
            _upgrade2Open = false;
            _upgrade3Open = false;
            _upgrade1AlreadyActivated = false;
            _upgrade2AlreadyActivated = false;
            _upgrade3AlreadyActivated = false;
            _weaponIndex = 0;
            _pendingUpgrade = -1;
            _gameOver = true;
        }

        private void Start()
        {
            StartLevel();
        }

        public void StartLevel()
        {
            _levelActive = true;
            _timeLeft = LevelSettings[_currentLevelIndex].TimerDuration;
            _weaponIndex = 0;
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
            GlobalEvents.MainMechanicTimerTicked?.Invoke(timeLeftPercentage);
            
            //Debug.Log($"Time Left Percentage: {timeLeftPercentage}");
            if (_weaponIndex >= 0 && timeLeftPercentage >= LevelSettings[_currentLevelIndex].UpgradeWindow1Range.x &&
                timeLeftPercentage <= LevelSettings[_currentLevelIndex].UpgradeWindow1Range.y)
            {
                if(!_upgrade1Open)
                {
                    Debug.Log("Upgrade Window 1 Open");
                    GlobalEvents.UpgradeWindowOpen?.Invoke(0);
                }
                _upgrade1Open = true;
                if (_upgrade1AlreadyActivated) return; // Prevent multiple activations
                if (UpdradeAction != null && UpdradeAction.action.WasPressedThisFrame())
                {
                    Debug.Log("Upgrade Window 1 Activated");
                    GlobalEvents.UpgradeGunWindowActivated?.Invoke(0);
                    _upgrade1AlreadyActivated = true;
                    _pendingUpgrade = 1; // Store the pending upgrade index
                }
            }
            else if (_weaponIndex >= 1 && timeLeftPercentage >= LevelSettings[_currentLevelIndex].UpgradeWindow2Range.x &&
                     timeLeftPercentage <= LevelSettings[_currentLevelIndex].UpgradeWindow2Range.y)
            {
                if(!_upgrade2Open)
                {
                    Debug.Log("Upgrade Window 2 Open");
                    GlobalEvents.UpgradeWindowOpen?.Invoke(1);
                }
                _upgrade2Open = true;
                if (_upgrade2AlreadyActivated) return; // Prevent multiple activations
                if (UpdradeAction != null && UpdradeAction.action.WasPressedThisFrame())
                {
                    Debug.Log("Upgrade Window 2 Activated");
                GlobalEvents.UpgradeGunWindowActivated?.Invoke(1);
                _upgrade2AlreadyActivated = true;
                _pendingUpgrade = 2; // Store the pending upgrade index
                }
                                     
                                     
            }
            else if (_weaponIndex == 2 && timeLeftPercentage >= LevelSettings[_currentLevelIndex].UpgradeWindow3Range.x &&
                     timeLeftPercentage <= LevelSettings[_currentLevelIndex].UpgradeWindow3Range.y)
            {
                if(!_upgrade3Open)
                {
                    Debug.Log("Upgrade Window 3 Open");
                    GlobalEvents.UpgradeWindowOpen?.Invoke(2);
                }
                _upgrade3Open = true;
                if (_upgrade3AlreadyActivated) return; // Prevent multiple activations
                if (UpdradeAction != null && UpdradeAction.action.WasPressedThisFrame())
                {
                    Debug.Log("Upgrade Window 3 Activated");
                    GlobalEvents.UpgradeGunWindowActivated?.Invoke(2);
                    _upgrade3AlreadyActivated = true;
                    _pendingUpgrade = 2;
                }
            }
            
            
            if (timeLeftPercentage < LevelSettings[_currentLevelIndex].UpgradeWindow1Range.x)
            {
                /*if (_upgrade1Open && !_upgrade1AlreadyActivated && _weaponIndex > 0)
                {
                    GlobalEvents.GunUpgraded?.Invoke(_weaponIndex - 1);
                }*/

                if (_upgrade1Open)
                {
                    Debug.Log("Upgrade Window 1 Closed");
                    GlobalEvents.UpgradeWindowClosed?.Invoke(0);
                }
                _upgrade1AlreadyActivated = false;
                _upgrade1Open = false;
                
            }
            if (timeLeftPercentage < LevelSettings[_currentLevelIndex].UpgradeWindow2Range.x)
            {
                /*if (_upgrade2Open && !_upgrade2AlreadyActivated && _weaponIndex > 1)
                {
                    GlobalEvents.GunUpgraded?.Invoke(_weaponIndex - 1);
                }*/
                if (_upgrade2Open)
                {
                    Debug.Log("Upgrade Window 2 Closed");
                    GlobalEvents.UpgradeWindowClosed?.Invoke(1);
                }
                _upgrade2AlreadyActivated = false;
                _upgrade2Open = false;
            }
            if (timeLeftPercentage < LevelSettings[_currentLevelIndex].UpgradeWindow3Range.x)
            {
                /*if (_upgrade3Open && !_upgrade3AlreadyActivated && _weaponIndex > 2)
                {
                    GlobalEvents.GunUpgraded?.Invoke(_weaponIndex - 1);
                }*/
                if (_upgrade3Open)
                {
                    Debug.Log("Upgrade Window 3 Closed");
                    GlobalEvents.UpgradeWindowClosed?.Invoke(2);
                }
                _upgrade3AlreadyActivated = false;
                _upgrade3Open = false;
            }
        }


        private void OnGUI()
        {
            if (!_levelActive) return;
            var settings = LevelSettings[_currentLevelIndex];
            // draw timer bar
            float timerPercentage = _timeLeft / LevelSettings[_currentLevelIndex].TimerDuration;
            GUI.Box(new Rect(10, 10, 200, 20), $"Time Left: {Mathf.CeilToInt(_timeLeft)}s");
            //green box
            GUI.Box(new Rect(10, 40, 200 * timerPercentage, 20), GUIContent.none);
            
            // draw upgrade windows
            if (_upgrade1Open && !_upgrade1AlreadyActivated)
            {
                GUIStyle styleBox1 = new GUIStyle
                    { normal = new GUIStyleState { background = Texture2D.whiteTexture, textColor = Color.green } };
                GUI.Box(new Rect(settings.UpgradeWindow1Range.x * 200f, 70, 
                    (settings.UpgradeWindow1Range.y - settings.UpgradeWindow1Range.x) * 200f, 20), "1", styleBox1);
            }
            else
            {
                GUI.Box(new Rect(settings.UpgradeWindow1Range.x * 200f, 70, 
                    (settings.UpgradeWindow1Range.y - settings.UpgradeWindow1Range.x) * 200f, 20), "1");
            }
            
            if (_upgrade2Open && !_upgrade2AlreadyActivated)
            {
                GUIStyle styleBox2 = new GUIStyle
                    { normal = new GUIStyleState { background = Texture2D.whiteTexture, textColor = Color.green } };
                GUI.Box(new Rect(settings.UpgradeWindow2Range.x * 200f, 70, 
                    (settings.UpgradeWindow2Range.y - settings.UpgradeWindow2Range.x) * 200f, 20), "2", styleBox2);
            }
            else
            {
                GUI.Box(new Rect(settings.UpgradeWindow2Range.x * 200f, 70, 
                    (settings.UpgradeWindow2Range.y - settings.UpgradeWindow2Range.x) * 200f, 20), "2");
            }
            
            if (_upgrade3Open && !_upgrade3AlreadyActivated)
            {
                GUIStyle styleBox3 = new GUIStyle
                    { normal = new GUIStyleState { background = Texture2D.whiteTexture, textColor = Color.green } };
                GUI.Box(new Rect(settings.UpgradeWindow3Range.x * 200f, 70, 
                    (settings.UpgradeWindow3Range.y - settings.UpgradeWindow3Range.x) * 200f, 20), "3", styleBox3);
            }
            else
            {
                GUI.Box(new Rect(settings.UpgradeWindow3Range.x * 200f, 70, 
                    (settings.UpgradeWindow3Range.y - settings.UpgradeWindow3Range.x) * 200f, 20), "3");
            }
            
        }

        public void OnSpecialBulletHit()
        {
            GlobalEvents.GunUpgraded.Invoke(_pendingUpgrade);
            _weaponIndex = _pendingUpgrade;
            _pendingUpgrade = -1;
        }

        public void OnSpecialBulletMiss()
        {
            //GlobalEvents.GunUpgraded.Invoke(_pendingUpgrade - 1);
            _pendingUpgrade = -1;
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
