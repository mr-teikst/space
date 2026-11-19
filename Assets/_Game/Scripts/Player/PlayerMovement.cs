using UnityEngine;
using UnityEngine.InputSystem;

namespace ChickenShooter
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 7;
        public Vector2 padding = new Vector2(.5f, .5f);
        public InputAction move = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");
        private void Awake()
        {
            if (move.bindings.Count == 0) { move.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d"); move.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow").With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow"); }
        }
        private void OnEnable() => move.Enable();
        private void OnDisable() => move.Disable();
        private void Update()
        {
            if (GameSession.Instance != null && !GameSession.Instance.CanPlay) return;
            Vector3 p = transform.position + (Vector3)(move.ReadValue<Vector2>().normalized * speed * Time.deltaTime);
            if (PlayfieldBounds.Instance != null) { var b = PlayfieldBounds.Instance; p.x = Mathf.Clamp(p.x, b.Min.x + padding.x, b.Max.x - padding.x); p.y = Mathf.Clamp(p.y, b.Min.y + padding.y, b.Max.y - padding.y); }
            transform.position = p;
        }
    }
}
