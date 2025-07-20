using System.Collections.Generic;
using UnityEngine;

namespace KenneyJam2025
{
    public class ShootersManager : Singleton<ShootersManager>
    {
        private List<IShooter> _shooters = new List<IShooter>();
        
        public int Count => _shooters.Count;
        
        public void RegisterShooter(IShooter shooter)
        {
            if (!_shooters.Contains(shooter))
            {
                _shooters.Add(shooter);
            }
        }
        
        public bool TryGetTargetShooter(IShooter shooter, float range, out IShooter targetShooter)
        {
            foreach (var potentialTarget in _shooters)
            {
                // if it's the same shooter, skip it
                if (potentialTarget == shooter)
                {
                    continue;
                }
                // if it's not in range, skip it
                float distance = Vector3.Distance(shooter.Position, potentialTarget.Position);
                if (distance > range)
                {
                    continue;
                }
                // if it's in line of sight, return it
                Ray ray = new Ray(shooter.Position + Vector3.up * 0.3f, potentialTarget.Position - shooter.Position);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, distance))
                {
                    if (hitInfo.collider.gameObject == potentialTarget.GameObject)
                    {
                        targetShooter = potentialTarget;
                        return true;
                    }
                }
            }

            targetShooter = null;
            return false;
        }

        public void UnregisterShooter(IShooter shooter)
        {
            if (_shooters.Contains(shooter))
            {
                _shooters.Remove(shooter);
            }
        }
    }
}