using UnityEngine;
namespace ChickenShooter
{
    public sealed class PooledObject : MonoBehaviour
    {
        [System.NonSerialized] public GameObject sourcePrefab;
        [System.NonSerialized] public bool released;
        IPoolable[] listeners;
        public void Spawned()
        {
            released = false;
            if (listeners == null) listeners = GetComponentsInChildren<IPoolable>(true);
            foreach (var item in listeners) item.OnSpawned();
        }
        public void Despawned()
        { if (listeners != null) foreach (var item in listeners) item.OnDespawned(); }
    }
}
