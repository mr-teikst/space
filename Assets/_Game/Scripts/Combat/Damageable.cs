using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenShooter
{
    public enum TargetKind { Enemy, Egg, Asteroid, Boss, Player }

    public class Damageable : MonoBehaviour
    {
        public TargetKind kind = TargetKind.Enemy;
        public float maxHealth = 1;
        public int score = 100;
        public GameObject dropPrefab;
        [Range(0, 1)] public float dropChance = .25f;
        public GameObject giftPrefab;
        [Range(0, 1)] public float giftChance = .08f;
        public float Health { get; private set; }
        public bool Dead { get; private set; }
        public event Action<Damageable, bool> Resolved;
        public event Action HitReceived;
        public static readonly List<Damageable> Active = new List<Damageable>();

        private void OnEnable() { Health = Mathf.Max(1, maxHealth); Dead = false; if (!Active.Contains(this)) Active.Add(this); }
        private void OnDisable() { Active.Remove(this); }
        public void Configure(float health) { maxHealth = health; Health = Mathf.Max(1, health); Dead = false; }
        public void Hit(float damage)
        {
            if (Dead || damage <= 0) return;
            Health = Mathf.Max(0, Health - damage);
            HitReceived?.Invoke();
            if (kind == TargetKind.Boss) GameSignals.BossHealthChanged?.Invoke(Health, maxHealth);
            if (Health <= 0) Resolve(true);
        }
        public void Resolve(bool killed)
        {
            if (Dead) return;
            Dead = true;
            if (killed)
            {
                if (score > 0 && GameSession.Instance != null) GameSession.Instance.AddScore(score);
                if (kind == TargetKind.Egg) GameSignals.EggBroken?.Invoke(transform.position);
                else if (kind == TargetKind.Asteroid) GameSignals.AsteroidBroken?.Invoke(transform.position);
                else GameSignals.EnemyKilled?.Invoke(transform.position);
                if (PoolService.Instance != null)
                {
                    float roll = UnityEngine.Random.value;
                    if (giftPrefab != null && roll < giftChance) PoolService.Instance.Spawn(giftPrefab, transform.position, Quaternion.identity);
                    else if (dropPrefab != null && roll < giftChance + dropChance) PoolService.Instance.Spawn(dropPrefab, transform.position, Quaternion.identity);
                }
            }
            Resolved?.Invoke(this, killed);
            if (PoolService.Instance != null) PoolService.Instance.Despawn(gameObject); else gameObject.SetActive(false);
        }
    }
}
