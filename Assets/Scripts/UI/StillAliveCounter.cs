using System;
using TMPro;
using UnityEngine;

namespace KenneyJam2025.UI
{
    public class StillAliveCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;

        private void Update()
        {
            int count = ShootersManager.Instance.Count;
            _counterText.text = $"STILL ALIVE: {count.ToString("D2")}";
        }
    }
}