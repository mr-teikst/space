using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenShooter
{
    public class LevelController : MonoBehaviour
    {
        public LevelDataSO level;
        public float collectionDelay = 1.5f;
        private readonly HashSet<Damageable> remaining = new HashSet<Damageable>();
        private bool spawning, finishing;
        private int generation;
        private void Start()
        {
            if (level == null || GameSession.Instance == null) { enabled = false; return; }
            GameSession.Instance.EnterLevel(level);
            StartRound();
        }
        private void OnDisable()
        {
            generation++;
            foreach (var target in remaining) if (target != null) target.Resolved -= OnResolved;
            remaining.Clear();
        }
        private void StartRound()
        {
            int index = GameSession.Instance.State.roundIndex - 1;
            if (index < 0 || index >= level.rounds.Length) { Debug.LogError("Invalid round index for " + level.name); return; }
            finishing = false; generation++; StartCoroutine(SpawnRound(level.rounds[index], generation));
        }
        private IEnumerator SpawnRound(WaveDataSO wave, int token)
        {
            spawning = true;
            GameSignals.Announcement?.Invoke("Round " + GameSession.Instance.State.roundIndex);
            if (wave == null) { Debug.LogError("Missing wave data"); spawning = false; yield break; }
            int count = wave.kind == WaveKind.Boss ? 1 : Mathf.Max(1, wave.count);
            for (int i = 0; i < count; i++)
            {
                while (GameSession.Instance != null && GameSession.Instance.Phase == GamePhase.Paused) yield return null;
                if (token != generation || GameSession.Instance == null || !GameSession.Instance.CanPlay) yield break;
                GameObject prefab = wave.kind == WaveKind.Boss ? wave.bossPrefab : wave.enemyPrefab;
                if (prefab == null || PoolService.Instance == null) { Debug.LogError("Wave is missing a prefab or pool service"); spawning = false; yield break; }
                Vector3 target = PositionFor(wave, i, count);
                if (PlayfieldBounds.Instance != null) target.x = Mathf.Clamp(target.x, PlayfieldBounds.Instance.Min.x + .5f, PlayfieldBounds.Instance.Max.x - .5f);
                Vector3 start = wave.kind == WaveKind.Asteroids ? new Vector3(target.x, 7, 0) : new Vector3(i % 2 == 0 ? -8 : 8, 7, 0);
                if (wave.kind == WaveKind.Boss) start = new Vector3(0, 7, 0);
                var obj = PoolService.Instance.Spawn(prefab, start, Quaternion.identity);
                var hp = obj.GetComponent<Damageable>();
                if (hp != null) { if (wave.kind != WaveKind.Boss) hp.Configure(wave.health); hp.Resolved += OnResolved; remaining.Add(hp); }
                else Debug.LogError("Wave prefab needs Damageable", obj);
                var asteroid = obj.GetComponent<AsteroidController>();
                if (asteroid != null) asteroid.speed = wave.moveSpeed;
                var enemy = obj.GetComponent<EnemyController>();
                if (enemy != null) { Vector3 control = wave.entryPath != null && wave.entryPath.Length > 0 ? (Vector3)wave.entryPath[i % wave.entryPath.Length] : new Vector3(-start.x * .4f, 5, 0); enemy.Configure(wave, target, control); }
                if (wave.spawnInterval > 0) yield return new WaitForSeconds(wave.spawnInterval);
            }
            spawning = false; TryFinish();
        }
        private static Vector3 PositionFor(WaveDataSO wave, int i, int count)
        {
            int columns = Mathf.Max(1, wave.columns); float spacing = wave.spacing * (wave.formation == FormationKind.Tight ? .7f : 1);
            if (wave.kind == WaveKind.Asteroids) return new Vector3((i % columns - (columns - 1) * .5f) * spacing, 5, 0);
            if (wave.formation == FormationKind.V)
            {
                int col = i % columns, row = i / columns;
                float centeredX = (col - (Mathf.Min(columns, count) - 1) * .5f) * spacing;
                float y = -Mathf.Abs(col - (columns - 1) * .5f) * spacing * .5f - row * spacing * .95f;
                return (Vector3)wave.formationCenter + new Vector3(centeredX, y, 0);
            }
            return (Vector3)wave.formationCenter + new Vector3((i % columns - (Mathf.Min(columns, count) - 1) * .5f) * spacing, -(i / columns) * spacing, 0);
        }
        private void OnResolved(Damageable target, bool killed) { target.Resolved -= OnResolved; remaining.Remove(target); TryFinish(); }
        private void TryFinish()
        {
            if (!spawning && !finishing && remaining.Count == 0 && GameSession.Instance != null && GameSession.Instance.CanPlay) { finishing = true; StartCoroutine(FinishRound(generation)); }
        }
        private IEnumerator FinishRound(int token)
        {
            int roundIndex = GameSession.Instance.State.roundIndex - 1;
            if (roundIndex >= 0 && roundIndex < level.rounds.Length && level.rounds[roundIndex] != null && level.rounds[roundIndex].kind == WaveKind.Boss)
                GameSignals.Announcement?.Invoke("SECTOR CLEAR");
            yield return new WaitForSeconds(collectionDelay);
            if (token != generation || GameSession.Instance == null || !GameSession.Instance.CanPlay) yield break;
            PoolService.Instance.ClearActive();
            GameSignals.BossNameChanged?.Invoke("");
            int next = GameSession.Instance.State.roundIndex + 1;
            if (next > level.rounds.Length) GameSession.Instance.CompleteLevel(level);
            else { GameSession.Instance.SetRound(level.levelIndex, next); StartRound(); }
        }
    }
}
