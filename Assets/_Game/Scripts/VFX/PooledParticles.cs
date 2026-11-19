using UnityEngine;
namespace ChickenShooter
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class PooledParticles : MonoBehaviour, IPoolable
    {
        ParticleSystem particles;
        void Awake() { particles=GetComponent<ParticleSystem>(); }
        public void OnSpawned() { if(!particles)particles=GetComponent<ParticleSystem>();particles.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);particles.Play(true); }
        public void OnDespawned() { particles.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear); }
    }
}
