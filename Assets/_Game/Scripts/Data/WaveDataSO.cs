using UnityEngine;
namespace ChickenShooter
{
    public enum WaveKind { Formation, Asteroids, Boss }
    public enum FormationKind { Grid, V, Tight }
    [CreateAssetMenu(menuName = "Chicken Shooter/Wave")]
    public sealed class WaveDataSO : ScriptableObject
    {
        public WaveKind kind;
        public FormationKind formation;
        public GameObject enemyPrefab;
        [Min(1)] public int count = 12;
        [Min(1)] public int columns = 6;
        public float spacing = 1.25f;
        public float health = 1;
        public float moveSpeed = 2;
        public float fireInterval = 3;
        public float diveInterval = 8;
        public float spawnInterval = .15f;
        public float duration = 15;
        public Vector2 formationCenter = new Vector2(0,3);
        public Vector2[] entryPath;
        public GameObject bossPrefab;
    }
}
