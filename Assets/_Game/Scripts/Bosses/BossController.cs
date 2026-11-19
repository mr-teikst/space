using UnityEngine;
namespace ChickenShooter
{
    [RequireComponent(typeof(Damageable))]
    public class BossController : MonoBehaviour
    {
        public enum BossState { Entry, Barrage, Spread, Dive, Enraged, Dead }
        public string bossName = "Giant Chicken";
        public GameObject eggPrefab;
        public GameObject featherPrefab;
        public GameObject summonPrefab;
        public float attackInterval = 1.2f;
        public float phaseDuration = 6;
        public float moveSpeed = 2;
        public Vector2 arenaCenter = new Vector2(0, 1.8f);
        public float diveDepth = 5f;
        public float eggSpeed = 4;
        public float summonInterval = 4;
        public BossState State { get; private set; }
        private Damageable health;
        private float timer, shotTimer, summonTimer;
        private Vector3 home;
        private void Awake() { health = GetComponent<Damageable>(); }
        private void OnEnable() { State = BossState.Entry; timer = shotTimer = summonTimer = 0; home = arenaCenter; }
        private void Start() { Announce(); }
        private void Announce() { GameSignals.BossNameChanged?.Invoke(bossName); GameSignals.BossHealthChanged?.Invoke(health.Health, health.maxHealth); }
        private void Update()
        {
            if (GameSession.Instance != null && !GameSession.Instance.CanPlay) return;
            if (health.Dead) { State = BossState.Dead; return; }
            if (health.Health / health.maxHealth < .3f && State != BossState.Enraged) { State = BossState.Enraged; timer = shotTimer = summonTimer = 0; }
            timer += Time.deltaTime; shotTimer -= Time.deltaTime;
            if (State == BossState.Entry) { transform.position = Vector3.MoveTowards(transform.position, home, moveSpeed * Time.deltaTime); if (Vector3.Distance(transform.position, home) < .05f) { Announce(); Switch(BossState.Barrage); } return; }
            if (State == BossState.Enraged)
            {
                transform.position = home + Vector3.right * Mathf.Sin(Time.time * 2) * 2;
                if (shotTimer <= 0) { ShootSpread(5); shotTimer = attackInterval * .45f; }
                summonTimer -= Time.deltaTime; if (summonTimer <= 0) { Summon(); summonTimer = summonInterval; }
                return;
            }
            if (State == BossState.Dive)
            {
                if (timer < 1) transform.position = home + Vector3.right * Mathf.Sin(timer * 25) * .15f;
                else transform.position = home + Vector3.down * (Mathf.Sin(Mathf.Clamp01((timer-1)/3) * Mathf.PI) * diveDepth);
                if (shotTimer <= 0) { ShootSpread(3, featherPrefab != null ? featherPrefab : eggPrefab); shotTimer = attackInterval; }
            }
            else
            {
                transform.position = home + Vector3.right * Mathf.Sin(Time.time * moveSpeed) * 2;
                if (shotTimer <= 0) { ShootSpread(State == BossState.Barrage ? 1 : 3); shotTimer = attackInterval; }
            }
            if (timer >= phaseDuration) Switch(State == BossState.Barrage ? BossState.Spread : State == BossState.Spread ? BossState.Dive : BossState.Barrage);
        }
        private void Switch(BossState state) { State = state; timer = 0; shotTimer = 0; }
        private void ShootSpread(int count, GameObject projectilePrefab = null)
        {
            if (projectilePrefab == null) projectilePrefab = eggPrefab;
            if (projectilePrefab == null || PoolService.Instance == null) return;
            for (int i = 0; i < count; i++)
            {
                float angle = 180 + (i - (count - 1) * .5f) * 18;
                var obj = PoolService.Instance.Spawn(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
                var p = obj.GetComponent<Projectile>(); if (p != null) { p.friendly = false; p.speed = eggSpeed; }
            }
        }
        private void Summon()
        {
            if (summonPrefab == null || PoolService.Instance == null) return;
            var obj = PoolService.Instance.Spawn(summonPrefab, transform.position + Vector3.down, Quaternion.identity);
            var enemy = obj.GetComponent<EnemyController>();
            if (enemy != null) enemy.ConfigureSummon();
        }
    }
}
