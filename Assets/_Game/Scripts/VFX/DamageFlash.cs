using UnityEngine;
namespace ChickenShooter
{
    [RequireComponent(typeof(Damageable), typeof(SpriteRenderer))]
    public sealed class DamageFlash : MonoBehaviour
    {
        SpriteRenderer target;
        Damageable health;
        MaterialPropertyBlock properties;
        float remaining;
        static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        void Awake() { target = GetComponent<SpriteRenderer>(); health = GetComponent<Damageable>(); properties = new MaterialPropertyBlock(); }
        void OnEnable() { if (!health) health = GetComponent<Damageable>(); health.HitReceived += Flash; }
        void OnDisable() { health.HitReceived -= Flash; SetFlash(0); remaining = 0; }
        void SetFlash(float value)
        { if (!target || properties == null) return; target.GetPropertyBlock(properties); properties.SetFloat(FlashAmount,value);target.SetPropertyBlock(properties); }
        void Flash() { remaining = .1f; SetFlash(1); }
        void Update() { if (remaining <= 0) return; remaining -= Time.deltaTime; if (remaining <= 0) SetFlash(0); }
    }
}
