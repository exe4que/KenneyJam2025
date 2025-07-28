using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KenneyJam2025.UI
{
    public class InGamUI : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public GameObject gameWonPanel;
        
        private void OnEnable()
        {
            GlobalEvents.PlayerDied += OnPlayerDied;
            GlobalEvents.GameWon += OnGameWon;
        }
        
        private void OnDisable()
        {
            GlobalEvents.PlayerDied -= OnPlayerDied;
            GlobalEvents.GameWon -= OnGameWon;
        }

        private void OnGameWon()
        {
            gameOverPanel.SetActive(false);
            gameWonPanel.SetActive(true);
            Debug.Log("Game Won".Color(Color.green));
        }

        private void OnPlayerDied()
        {
            gameOverPanel.SetActive(true);
            gameWonPanel.SetActive(false);
            Debug.Log("Game Over".Color(Color.red));
        }
        
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MenuScene"); 
        }

        public void Retry()
        {
            SceneManager.LoadScene("ReloadLevel"); 
        }
    }
}