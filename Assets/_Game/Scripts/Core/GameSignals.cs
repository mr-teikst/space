using System;
using UnityEngine;
namespace ChickenShooter
{
    public static class GameSignals
    {
        public static Action<Vector3> EnemyKilled;
        public static Action<Vector3> EggBroken;
        public static Action<Vector3> AsteroidBroken;
        public static Action<Vector3, float> Blast;
        public static Action PlayerHit;
        public static Action<string> SoundRequested;
        public static Action<float, float> BossHealthChanged;
        public static Action<string> BossNameChanged;
        public static Action<string> Announcement;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Reset()
        {
            EnemyKilled = null; EggBroken = null; AsteroidBroken = null; Blast = null; PlayerHit = null;
            SoundRequested = null; BossHealthChanged = null; BossNameChanged = null; Announcement = null;
        }
    }
}
