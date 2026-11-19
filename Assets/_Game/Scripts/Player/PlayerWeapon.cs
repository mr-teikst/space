using UnityEngine;
using UnityEngine.InputSystem;

namespace ChickenShooter
{
    public class PlayerWeapon : MonoBehaviour
    {
        public WeaponDataSO weapon;
        public Transform muzzle;
        public float parallelSpacing = .22f;
        public InputAction fire = new InputAction("Fire", InputActionType.Button, "<Keyboard>/space");
        public InputAction missile = new InputAction("Missile", InputActionType.Button, "<Keyboard>/x");
        private float nextShot;
        private void OnEnable() { fire.Enable(); missile.Enable(); nextShot = 0; }
        private void OnDisable() { fire.Disable(); missile.Disable(); }
        private void Update()
        {
            if (GameSession.Instance == null || !GameSession.Instance.CanPlay) return;
            if (missile.WasPressedThisFrame() && GameSession.Instance.TryConsumeMissile()) Detonate();
            if (fire.IsPressed() && weapon != null && Time.time >= nextShot) { nextShot = Time.time + Mathf.Max(.02f, weapon.fireInterval); Fire(); }
        }
        private void Fire()
        {
            int level = Mathf.Clamp(GameSession.Instance.State.powerLevel, 1, 3);
            Vector3 origin = muzzle != null ? muzzle.position : transform.position;
            if (level == 1) Shot(origin, 0);
            else if (level == 2) { Shot(origin + Vector3.left * parallelSpacing, 0); Shot(origin + Vector3.right * parallelSpacing, 0); }
            else { Shot(origin, 0); Shot(origin, weapon.spreadAngle); Shot(origin, -weapon.spreadAngle); }
            GameSignals.SoundRequested?.Invoke("laser");
        }
        private void Shot(Vector3 position, float angle)
        {
            if (weapon.projectilePrefab == null || PoolService.Instance == null) return;
            var obj = PoolService.Instance.Spawn(weapon.projectilePrefab, position, Quaternion.Euler(0, 0, angle));
            var p = obj.GetComponent<Projectile>(); if (p != null) { p.friendly = true; p.speed = weapon.projectileSpeed; p.damage = weapon.damage; }
        }
        private void Detonate()
        {
            var targets = Damageable.Active.ToArray();
            foreach (var target in targets) { if (target == null || target.Dead) continue; if (target.kind == TargetKind.Boss) target.Hit(target.maxHealth * .2f); else if (target.kind == TargetKind.Enemy || target.kind == TargetKind.Egg) target.Hit(float.MaxValue); }
            GameSignals.Blast?.Invoke(transform.position, 1); GameSignals.SoundRequested?.Invoke("missile");
        }
    }
}
