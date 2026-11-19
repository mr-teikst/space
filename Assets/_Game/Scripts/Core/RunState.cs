using System;
namespace ChickenShooter
{
    [Serializable]
    public sealed class RunState
    {
        public int schemaVersion = 1;
        public int levelIndex = 1;
        public int roundIndex = 1;
        public int score;
        public int lives = 3;
        public int missiles = 2;
        public int powerLevel = 1;
        public int foodProgress;
        public RunState Copy() => (RunState)MemberwiseClone();
    }
}
