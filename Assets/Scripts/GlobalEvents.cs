using System;
using System.Collections.Generic;
namespace KenneyJam2025
{
    public static class GlobalEvents
    {
        public static Action<IShooter> ShotFired;
        public static Action<IShooter, IDamageable, float> SomethingDamaged;
        public static Action PlayerDied;
        public static Action<int> UpgradeGunWindowActivated;
        public static Action<IShooter, int> GunUpgraded;
        public static Action<string, List<string>> OnSceneChangeRequested;
        public static Action<float> MainMechanicTimerTicked;
        public static Action<int> UpgradeWindowOpen;
        public static Action<int> UpgradeWindowClosed;
        public static Action GameWon;
    }
}
