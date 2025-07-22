using System;
using UnityEngine;
using UnityEngine.UI;

namespace KenneyJam2025.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private EnemyAI _target;

        private void Update()
        {
            _slider.value = _target.CurrentHealth / _target.MaxHealth;
        }
    }
}