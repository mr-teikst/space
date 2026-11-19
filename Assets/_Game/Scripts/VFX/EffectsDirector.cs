using UnityEngine;
namespace ChickenShooter
{
    public sealed class EffectsDirector : MonoBehaviour
    {
        public GameObject featherBurst, eggBreak, shockwave, asteroidBurst;
        void OnEnable() { GameSignals.AsteroidBroken += Asteroid; GameSignals.EnemyKilled += Killed; GameSignals.EggBroken += Broken; GameSignals.Blast += Blast; }
        void OnDisable() { GameSignals.AsteroidBroken -= Asteroid; GameSignals.EnemyKilled -= Killed; GameSignals.EggBroken -= Broken; GameSignals.Blast -= Blast; }
        void Spawn(GameObject prefab, Vector3 position) { if (prefab && PoolService.Instance) PoolService.Instance.Spawn(prefab, position, Quaternion.identity); }
        void Killed(Vector3 position) { Spawn(featherBurst, position); GameSignals.SoundRequested?.Invoke("cluck"); }
        void Asteroid(Vector3 position) { Spawn(asteroidBurst, position); GameSignals.SoundRequested?.Invoke("egg"); }
        void Broken(Vector3 position) { Spawn(eggBreak, position); GameSignals.SoundRequested?.Invoke("egg"); }
        void Blast(Vector3 position, float strength) { Spawn(shockwave, position); }
    }
}
