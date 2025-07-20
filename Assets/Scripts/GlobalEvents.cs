using System;

namespace KenneyJam2025
{
    public static class GlobalEvents
    {
        public static Action<IShooter> ShotFired;
        public static Action<IShooter, IDamageable, float> SomethingDamaged;
        public static Action PlayerDied;
        public static Action<int> UpgradeGunWindowActivated;
        public static Action<int> GunUpgraded;
    }
}