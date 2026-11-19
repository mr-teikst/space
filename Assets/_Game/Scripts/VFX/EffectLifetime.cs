using UnityEngine;
namespace ChickenShooter
{
    public sealed class EffectLifetime : MonoBehaviour
    {
        public float seconds = 1;
        public bool expand;
        float age;
        Vector3 initialScale;
        SpriteRenderer sprite;
        Color color;
        void Awake() { initialScale = transform.localScale; sprite = GetComponent<SpriteRenderer>(); if (sprite) color = sprite.color; }
        void OnEnable() { age = 0; transform.localScale = initialScale; if (sprite) sprite.color = color; }
        void Update()
        {
            age += Time.deltaTime;
            float fraction = Mathf.Clamp01(age/seconds);
            if (expand) transform.localScale = initialScale * (1 + fraction * 18);
            if (sprite) { var c = color; c.a *= 1-fraction; sprite.color = c; }
            if (age >= seconds && PoolService.Instance) PoolService.Instance.Despawn(gameObject);
        }
    }
}
