using UnityEngine;

namespace ChickenShooter
{
    [RequireComponent(typeof(Damageable))]
    public class EnemyController : MonoBehaviour
    {
        public GameObject eggPrefab;
        public float entryDuration = 2;
        public float eggSpeed = 4;
        public float movementAmplitude = .4f;
        private Damageable health;
        private Vector3 start, control, destination, diveDirection;
        private float elapsed, fireTimer, diveTimer, speed, fireInterval, diveInterval;
        private bool diving, configured;
        private void Awake() { health = GetComponent<Damageable>(); }
        private void OnEnable() { configured = false; diving = false; elapsed = 0; }
        public void Configure(WaveDataSO wave, Vector3 target, Vector3 waypoint)
        {
            if (health == null) health = GetComponent<Damageable>();
            health.Configure(wave.health); start = transform.position; destination = target; control = waypoint;
            speed = wave.moveSpeed; fireInterval = wave.fireInterval; diveInterval = wave.diveInterval;
            fireTimer = Random.Range(.5f, Mathf.Max(.6f, fireInterval)); diveTimer = Random.Range(diveInterval * .7f, diveInterval * 1.3f); configured = true;
        }
        public void ConfigureSummon()
        {
            if (health == null) health = GetComponent<Damageable>();
            start = transform.position; destination = start + Vector3.down * 1.5f; control = start + Vector3.right;
            speed = 3; fireInterval = 2; diveInterval = 5; fireTimer = 1; diveTimer = 4; configured = true;
        }
        private void Update()
        {
            if (!configured || health.Dead || (GameSession.Instance != null && !GameSession.Instance.CanPlay)) return;
            elapsed += Time.deltaTime;
            if (elapsed < entryDuration)
            {
                float t = elapsed / Mathf.Max(.01f, entryDuration); transform.position = (1-t)*(1-t)*start + 2*(1-t)*t*control + t*t*destination; return;
            }
            if (diving)
            {
                transform.position += diveDirection * (speed * 2 * Time.deltaTime);
                if (PlayfieldBounds.Instance != null && !PlayfieldBounds.Instance.Contains(transform.position, 2)) health.Resolve(false);
                return;
            }
            transform.position = destination + Vector3.right * (Mathf.Sin(Time.time * speed + destination.x) * movementAmplitude);
            if (diveInterval > 0 && (diveTimer -= Time.deltaTime) <= 0)
            {
                var player = FindFirstObjectByType<PlayerHealth>();
                Vector3 direction = player != null ? player.transform.position - transform.position : Vector3.down;
                direction = direction.normalized;
                direction.y = Mathf.Min(direction.y, -.25f);
                diveDirection = direction.normalized;
                diving = true;
                return;
            }
            if (eggPrefab != null && fireInterval > 0 && (fireTimer -= Time.deltaTime) <= 0)
            {
                fireTimer = fireInterval;
                var egg = PoolService.Instance.Spawn(eggPrefab, transform.position, Quaternion.Euler(0, 0, 180));
                GameSignals.SoundRequested?.Invoke("cluck");
                var p = egg.GetComponent<Projectile>(); if (p != null) { p.friendly = false; p.speed = eggSpeed; }
            }
        }
    }
}
