using UnityEngine;

namespace ChickenShooter
{
    public class PlayerHealth : MonoBehaviour
    {
        public float invulnerabilityDuration = 1.5f;
        public SpriteRenderer spriteRenderer;
        private float invulnerable;
        private void OnEnable() { invulnerable = 0; if (spriteRenderer != null) spriteRenderer.enabled = true; }
        private void OnDisable() { if (spriteRenderer != null) spriteRenderer.enabled = true; }
        private void Update()
        {
            if (GameSession.Instance != null && !GameSession.Instance.CanPlay) return;
            if (invulnerable > 0) { invulnerable -= Time.deltaTime; if (spriteRenderer != null) spriteRenderer.enabled = invulnerable <= 0 || Mathf.FloorToInt(invulnerable * 12) % 2 == 0; }
        }
        public void TakeHit()
        {
            if (invulnerable > 0 || GameSession.Instance == null || !GameSession.Instance.CanPlay) return;
            if (GameSession.Instance.DamagePlayer()) { invulnerable = invulnerabilityDuration; }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.GetComponentInParent<Damageable>();
            if (target != null && !target.Dead && (target.kind == TargetKind.Enemy || target.kind == TargetKind.Asteroid || target.kind == TargetKind.Boss)) TakeHit();
        }
    }
}
