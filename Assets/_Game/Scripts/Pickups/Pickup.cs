using UnityEngine;
namespace ChickenShooter
{
    public enum PickupKind { Food, Gift }
    public class Pickup : MonoBehaviour
    {
        public PickupKind kind;
        public int score = 50;
        public int foodAmount = 1;
        public float speed = 2;
        private bool collected;
        private void OnEnable() { collected = false; }
        private void Update()
        {
            if (GameSession.Instance != null && !GameSession.Instance.CanPlay) return;
            transform.position += Vector3.down * (speed * Time.deltaTime);
            if (PlayfieldBounds.Instance != null && transform.position.y < PlayfieldBounds.Instance.Min.y - 2) Despawn();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collected || other.GetComponentInParent<PlayerHealth>() == null || GameSession.Instance == null || !GameSession.Instance.CanPlay) return;
            collected = true;
            if (kind == PickupKind.Food) { GameSession.Instance.AddScore(score); GameSession.Instance.CollectFood(foodAmount); }
            else GameSession.Instance.UpgradeWeapon();
            GameSignals.SoundRequested?.Invoke("pickup"); Despawn();
        }
        private void Despawn() { if (PoolService.Instance != null) PoolService.Instance.Despawn(gameObject); else gameObject.SetActive(false); }
    }
}
