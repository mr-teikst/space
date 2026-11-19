using UnityEngine;
namespace ChickenShooter
{
    [CreateAssetMenu(menuName = "Chicken Shooter/Weapon")]
    public sealed class WeaponDataSO : ScriptableObject
    {
        public string displayName;
        public Sprite icon;
        public GameObject projectilePrefab;
        [Min(1)] public int damage = 1;
        public float projectileSpeed = 14;
        public float fireInterval = .17f;
        public float spreadAngle = 15;
    }
}
