using UnityEngine;
namespace ChickenShooter
{
    [RequireComponent(typeof(Camera))]
    public sealed class PlayfieldBounds : MonoBehaviour
    {
        public static PlayfieldBounds Instance { get; private set; }
        [SerializeField] Camera view;
        [SerializeField] float margin = .35f;
        public Vector2 Min { get { var v = view.ViewportToWorldPoint(Vector3.zero); return new Vector2(v.x + margin, v.y + margin); } }
        public Vector2 Max { get { var v = view.ViewportToWorldPoint(Vector3.one); return new Vector2(v.x - margin, v.y - margin); } }
        void Awake() { if (!view) view = GetComponent<Camera>(); Instance = this; }
        public bool Contains(Vector2 point, float padding = 0) => point.x >= Min.x-padding && point.x <= Max.x+padding && point.y >= Min.y-padding && point.y <= Max.y+padding;
        void OnDestroy() { if (Instance == this) Instance = null; }
    }
}
