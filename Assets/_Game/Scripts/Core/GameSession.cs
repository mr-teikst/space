using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ChickenShooter
{
    [DefaultExecutionOrder(-1000)]
    [RequireComponent(typeof(SaveService))]
    public sealed class GameSession : MonoBehaviour
    {
        public static GameSession Instance { get; private set; }
        [SerializeField] LevelDataSO[] levels;
        [SerializeField] SaveService saveService;
        [SerializeField, Min(1)] int foodPerMissile = 10;
        [SerializeField] string menuScene = "MainMenu";
        public RunState State { get; private set; } = new RunState();
        public GamePhase Phase { get; private set; } = GamePhase.Menu;
        public bool CanPlay => Phase == GamePhase.Playing;
        public int FoodPerMissile => foodPerMissile;
        public LevelDataSO[] Levels => levels;
        public bool HasSave => saveService != null && saveService.TryRead(levels, out _);
        public string SavePath => saveService.SavePath;
        public event Action Changed;
        public event Action<GamePhase> PhaseChanged;
        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            if (!saveService) saveService = GetComponent<SaveService>();
            DontDestroyOnLoad(gameObject);
        }
        void OnDestroy() { if (Instance == this) { Instance = null; Time.timeScale = 1; } }
        void SetPhase(GamePhase phase)
        {
            Phase = phase; Time.timeScale = phase == GamePhase.Paused || phase == GamePhase.GameOver || phase == GamePhase.Victory ? 0 : 1;
            PhaseChanged?.Invoke(phase); Changed?.Invoke();
        }
        public void NewGame()
        {
            State = new RunState(); saveService.Write(State); LoadLevel(levels[0]);
        }
        public void ContinueGame()
        {
            if (!saveService.TryRead(levels, out var restored)) return;
            State = restored; LoadLevel(levels[State.levelIndex - 1]);
        }
        void LoadLevel(LevelDataSO level)
        {
            SetPhase(GamePhase.Transition);
            SceneManager.LoadScene(level.sceneName);
        }
        public void EnterLevel(LevelDataSO level)
        {
            if (State.levelIndex != level.levelIndex) { State.levelIndex = level.levelIndex; State.roundIndex = 1; }
            State.roundIndex = Mathf.Clamp(State.roundIndex, 1, level.rounds.Length);
            SetPhase(GamePhase.Playing); GameSignals.BossNameChanged?.Invoke("");
        }
        public void SetRound(int level, int round)
        {
            if (!CanPlay) return;
            State.levelIndex = level; State.roundIndex = round; saveService.Write(State); Changed?.Invoke();
        }
        public void CompleteLevel(LevelDataSO level)
        {
            if (!CanPlay) return;
            if (level.nextLevel != null)
            {
                State.levelIndex = level.nextLevel.levelIndex; State.roundIndex = 1;
                saveService.Write(State); LoadLevel(level.nextLevel);
            }
            else { saveService.Clear(); SetPhase(GamePhase.Victory); GameSignals.SoundRequested?.Invoke("victory"); }
        }
        public void AddScore(int score) { if (!CanPlay) return; State.score += Mathf.Max(0, score); Changed?.Invoke(); }
        public void CollectFood(int count = 1)
        {
            if (!CanPlay) return;
            State.foodProgress += Mathf.Max(0, count);
            while (State.foodProgress >= foodPerMissile) { State.foodProgress -= foodPerMissile; State.missiles++; }
            Changed?.Invoke(); GameSignals.SoundRequested?.Invoke("pickup");
        }
        public void UpgradeWeapon()
        { if (!CanPlay) return; State.powerLevel = Mathf.Min(3, State.powerLevel + 1); Changed?.Invoke(); GameSignals.SoundRequested?.Invoke("upgrade"); }
        public bool TryConsumeMissile()
        {
            if (!CanPlay || State.missiles <= 0) return false;
            State.missiles--; Changed?.Invoke(); return true;
        }
        public bool DamagePlayer()
        {
            if (!CanPlay) return false;
            State.lives--; State.powerLevel = Mathf.Max(1, State.powerLevel - 1);
            GameSignals.PlayerHit?.Invoke(); GameSignals.SoundRequested?.Invoke("hit");
            if (State.lives <= 0) ShowGameOver(); else Changed?.Invoke();
            return true;
        }
        public void ShowGameOver()
        {
            if (!CanPlay) return;
            var retryState = State.Copy(); retryState.roundIndex = 1; retryState.lives = 3;
            saveService.Write(retryState); SetPhase(GamePhase.GameOver); GameSignals.SoundRequested?.Invoke("gameover");
        }
        public void Retry()
        {
            if (Phase != GamePhase.GameOver) return;
            if (saveService.TryRead(levels, out var retry)) State = retry;
            else { State.roundIndex = 1; State.lives = 3; }
            LoadLevel(levels[State.levelIndex - 1]);
        }
        public void ReturnToMenu() { SetPhase(GamePhase.Menu); SceneManager.LoadScene(menuScene); }
        public void TogglePause()
        {
            if (Phase == GamePhase.Playing) SetPhase(GamePhase.Paused);
            else if (Phase == GamePhase.Paused) SetPhase(GamePhase.Playing);
        }
    }
}
