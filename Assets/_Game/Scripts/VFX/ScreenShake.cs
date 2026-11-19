using UnityEngine;
namespace ChickenShooter
{
    public sealed class ScreenShake : MonoBehaviour
    {
        Vector3 rest;
        float strength;
        void Awake() { rest = transform.localPosition; }
        void OnEnable() { GameSignals.Blast += Blast; GameSignals.PlayerHit += Hit; }
        void OnDisable() { GameSignals.Blast -= Blast; GameSignals.PlayerHit -= Hit; transform.localPosition = rest; }
        void Hit() { strength = .15f; }
        void Blast(Vector3 position, float amount) { strength = .25f; }
        void LateUpdate() { strength = Mathf.MoveTowards(strength,0,Time.unscaledDeltaTime * .6f); transform.localPosition = rest + (Vector3)Random.insideUnitCircle * strength; }
    }
}
