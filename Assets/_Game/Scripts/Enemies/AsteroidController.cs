using UnityEngine;
namespace ChickenShooter
{
    [RequireComponent(typeof(Damageable))]
    public class AsteroidController : MonoBehaviour
    {
        public float speed = 2;
        public float rotationSpeed = 60;
        private Damageable health;
        private void Awake() { health = GetComponent<Damageable>(); }
        private void Update()
        {
            if (GameSession.Instance != null && !GameSession.Instance.CanPlay) return;
            transform.position += Vector3.down * (speed * Time.deltaTime); transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            if (PlayfieldBounds.Instance != null && transform.position.y < PlayfieldBounds.Instance.Min.y - 2) health.Resolve(false);
        }
    }
}
