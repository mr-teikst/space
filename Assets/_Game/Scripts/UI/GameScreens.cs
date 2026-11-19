using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace ChickenShooter
{
    public sealed class GameScreens : MonoBehaviour
    {
        public GameObject pausePanel, gameOverPanel, victoryPanel;
        public Text gameOverScore, victoryScore;
        public InputAction pause = new InputAction("Pause", InputActionType.Button, "<Keyboard>/escape");
        void OnEnable() { pause.Enable(); if (GameSession.Instance) GameSession.Instance.PhaseChanged += Display; }
        void Start() { Display(GameSession.Instance.Phase); }
        void OnDisable() { pause.Disable(); if (GameSession.Instance) GameSession.Instance.PhaseChanged -= Display; }
        void Update() { if (pause.WasPressedThisFrame() && GameSession.Instance) GameSession.Instance.TogglePause(); }
        void Display(GamePhase phase)
        {
            pausePanel.SetActive(phase == GamePhase.Paused); gameOverPanel.SetActive(phase == GamePhase.GameOver); victoryPanel.SetActive(phase == GamePhase.Victory);
            gameOverScore.text = "SCORE  " + GameSession.Instance.State.score.ToString("N0") + "\nRETRY FROM WAVE 01 OF THIS SECTOR";
            victoryScore.text = "ALL THREE SECTORS SECURED\nFINAL SCORE  " + GameSession.Instance.State.score.ToString("N0");
        }
        public void Resume() { if (GameSession.Instance.Phase == GamePhase.Paused) GameSession.Instance.TogglePause(); }
        public void Retry() => GameSession.Instance.Retry();
        public void Menu() => GameSession.Instance.ReturnToMenu();
    }
}
