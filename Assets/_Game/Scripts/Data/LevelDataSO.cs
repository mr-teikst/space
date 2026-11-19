using UnityEngine;
namespace ChickenShooter
{
    [CreateAssetMenu(menuName = "Chicken Shooter/Level")]
    public sealed class LevelDataSO : ScriptableObject
    {
        public int levelIndex = 1;
        public string sceneName;
        public string displayName;
        public AudioClip music;
        public Color backgroundColor = new Color(.025f,.045f,.10f);
        public WaveDataSO[] rounds;
        public LevelDataSO nextLevel;
    }
}
