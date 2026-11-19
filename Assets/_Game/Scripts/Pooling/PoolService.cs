using System;
using System.Collections.Generic;
using UnityEngine;
namespace ChickenShooter
{
    [DefaultExecutionOrder(-800)]
    public sealed class PoolService : MonoBehaviour
    {
        [Serializable] public struct Entry { public GameObject prefab; public int prewarm; }
        public static PoolService Instance { get; private set; }
        [SerializeField] Entry[] entries;
        readonly Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();
        readonly HashSet<GameObject> active = new HashSet<GameObject>();
        void Awake()
        {
            Instance = this;
            if (entries != null) foreach (var entry in entries)
                if (entry.prefab != null)
                {
                    if (!pools.ContainsKey(entry.prefab)) pools.Add(entry.prefab, new Queue<GameObject>());
                    for (int i = 0; i < entry.prewarm; i++) pools[entry.prefab].Enqueue(Create(entry.prefab));
                }
        }
        GameObject Create(GameObject prefab)
        {
            var item = Instantiate(prefab, transform); item.SetActive(false);
            var member = item.GetComponent<PooledObject>();
            if (!member) member = item.AddComponent<PooledObject>();
            member.sourcePrefab = prefab; member.released = true; return item;
        }
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation = default)
        {
            if (!prefab) return null;
            if (!pools.TryGetValue(prefab, out var queue)) { queue = new Queue<GameObject>(); pools.Add(prefab, queue); }
            GameObject item = null;
            while (queue.Count > 0 && !item) item = queue.Dequeue();
            if (!item) item = Create(prefab);
            item.transform.SetPositionAndRotation(position, rotation == default ? Quaternion.identity : rotation);
            item.SetActive(true); active.Add(item); item.GetComponent<PooledObject>().Spawned(); return item;
        }
        public void Despawn(GameObject item)
        {
            if (!item) return;
            var member = item.GetComponent<PooledObject>();
            if (!member) { Destroy(item); return; }
            if (member.released) return;
            member.released = true; active.Remove(item); member.Despawned(); item.SetActive(false);
            if (member.sourcePrefab && pools.TryGetValue(member.sourcePrefab, out var queue)) queue.Enqueue(item);
        }
        public void ClearActive()
        { foreach (var item in new List<GameObject>(active)) Despawn(item); }
        void OnDestroy() { if (Instance == this) Instance = null; }
    }
}
