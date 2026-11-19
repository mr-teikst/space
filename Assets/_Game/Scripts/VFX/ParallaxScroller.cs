using UnityEngine;
namespace ChickenShooter
{
    public sealed class ParallaxScroller : MonoBehaviour
    {
        [SerializeField] float speed = .2f;
        [SerializeField] float wrapHeight = 24;
        void Update()
        { transform.position += Vector3.down * (speed * Time.deltaTime); if (transform.position.y < -wrapHeight/2) transform.position += Vector3.up * wrapHeight; }
    }
}
