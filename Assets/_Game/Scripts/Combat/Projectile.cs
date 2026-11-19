using UnityEngine;

namespace ChickenShooter
{
    public class Projectile : MonoBehaviour
    {
        public bool friendly = true;
        public float speed = 8;
        public float damage = 1;
        public float lifetime = 6;
        private float age;
        private bool spent;
        private void OnEnable() { age = 0; spent = false; }
        private void Update()
        {
            if (GameSession.Instance != null && !GameSession.Instance.CanPlay) return;
            transform.position += transform.up * (speed * Time.deltaTime);
            age += Time.deltaTime;
            if (age >= lifetime || (PlayfieldBounds.Instance != null && !PlayfieldBounds.Instance.Contains(transform.position, 2))) Despawn();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (spent) return;
            if (friendly)
            {
                var target = other.GetComponentInParent<Damageable>();
                if (target == null || target.kind == TargetKind.Player || target.Dead) return;
                spent = true; target.Hit(damage); Despawn();
            }
            else
            {
                var player = other.GetComponentInParent<PlayerHealth>();
                if (player == null) return;
                spent = true; player.TakeHit(); Despawn();
            }
        }
        private void Despawn() { if (spent && !gameObject.activeSelf) return; spent = true; if (PoolService.Instance != null) PoolService.Instance.Despawn(gameObject); else gameObject.SetActive(false); }
    }
}
