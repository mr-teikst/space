using UnityEngine;
namespace ChickenShooter
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class SpriteFlipbook : MonoBehaviour
    {
        public Sprite[] frames;
        public float framesPerSecond = 8;
        public bool loop = true;
        public bool releaseOnComplete;
        float age;
        SpriteRenderer target;
        void Awake() { target = GetComponent<SpriteRenderer>(); }
        void OnEnable() { age = 0; if (!target) target = GetComponent<SpriteRenderer>(); if (frames != null && frames.Length > 0) target.sprite = frames[0]; }
        void Update()
        {
            if (GameSession.Instance && !GameSession.Instance.CanPlay) return;
            if (frames == null || frames.Length == 0) return;
            age += Time.deltaTime;
            int index = Mathf.FloorToInt(age * framesPerSecond);
            if (index >= frames.Length && !loop)
            { if (releaseOnComplete && PoolService.Instance) PoolService.Instance.Despawn(gameObject); return; }
            target.sprite = frames[loop ? index % frames.Length : Mathf.Min(index, frames.Length-1)];
        }
    }
}
