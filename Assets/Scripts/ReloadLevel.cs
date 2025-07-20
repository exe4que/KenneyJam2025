using System;
using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2025
{
    public class ReloadLevel : MonoBehaviour
    {
        [SerializeField] private string _mainScene;  //Scenes that get assigned in the inspectorr
        [SerializeField] private List<string> _scenesAdditive = new List<string>();
        private void Start()
        {
            GlobalEvents.OnSceneChangeRequested?.Invoke(_mainScene, _scenesAdditive);
        }
    }
}