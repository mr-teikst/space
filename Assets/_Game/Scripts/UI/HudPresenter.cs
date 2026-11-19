using UnityEngine;
using UnityEngine.UI;
namespace ChickenShooter
{
    public sealed class HudPresenter : MonoBehaviour
    {
        public LevelDataSO level;
        public Text stageLabel, roundLabel, scoreLabel, livesLabel, weaponLabel, missileLabel, foodLabel, bossLabel, announcement;
        public Image foodFill, bossFill;
        public GameObject bossGroup;
        float announcementTime;
        void OnEnable()
        {
            if (GameSession.Instance) GameSession.Instance.Changed += Refresh;
            GameSignals.BossNameChanged += BossName;
            GameSignals.BossHealthChanged += BossHealth;
            GameSignals.Announcement += Announce;
        }
        void Start() { bossGroup.SetActive(false); Refresh(); }
        void OnDisable()
        {
            if (GameSession.Instance) GameSession.Instance.Changed -= Refresh;
            GameSignals.BossNameChanged -= BossName; GameSignals.BossHealthChanged -= BossHealth; GameSignals.Announcement -= Announce;
        }
        void Refresh()
        {
            if (!GameSession.Instance) return;
            var s = GameSession.Instance.State;
            stageLabel.text = level.displayName.ToUpperInvariant();
            roundLabel.text = "WAVE " + s.roundIndex.ToString("00") + " / " + level.rounds.Length.ToString("00");
            scoreLabel.text = s.score.ToString("N0");
            livesLabel.text = "HULL  " + new string('|', Mathf.Max(0, s.lives)) + new string('.', Mathf.Max(0, 3-s.lives));
            weaponLabel.text = "LASER  " + s.powerLevel + "/3";
            missileLabel.text = "MISSILES  " + s.missiles + "   [X]";
            foodLabel.text = "SUPPLY  " + s.foodProgress + "/" + GameSession.Instance.FoodPerMissile;
            foodFill.fillAmount = (float)s.foodProgress / GameSession.Instance.FoodPerMissile;
        }
        void BossName(string name) { bossGroup.SetActive(!string.IsNullOrEmpty(name)); bossLabel.text = name.ToUpperInvariant(); }
        void BossHealth(float health, float maximum) { bossFill.fillAmount = maximum > 0 ? health/maximum : 0; if (health <= 0) bossGroup.SetActive(false); }
        void Announce(string message) { announcement.text = message.ToUpperInvariant(); announcementTime = 2.2f; }
        void Update() { if (announcementTime > 0) { announcementTime -= Time.deltaTime; if (announcementTime <= 0) announcement.text = ""; } }
    }
}
