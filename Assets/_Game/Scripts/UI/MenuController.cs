using UnityEngine;
using UnityEngine.UI;
namespace ChickenShooter
{
    public sealed class MenuController : MonoBehaviour
    {
        public Button continueButton;
        public Text checkpointLabel;
        public Slider musicSlider;
        public Slider effectsSlider;
        void Start()
        {
            var session = GameSession.Instance;
            continueButton.interactable = session && session.HasSave;
            checkpointLabel.text = session && session.HasSave ? "CHECKPOINT AVAILABLE  /  CONTINUE YOUR FLIGHT" : "NO CHECKPOINT YET  /  START A NEW FLIGHT";
            musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", .45f));
            effectsSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("EffectsVolume", .65f));
        }
        public void NewGame() => GameSession.Instance.NewGame();
        public void Continue() => GameSession.Instance.ContinueGame();
        public void Quit() => Application.Quit();
        public void SetMusic(float value) { PlayerPrefs.SetFloat("MusicVolume", value); AudioDirector.Instance?.ApplyVolume(); }
        public void SetEffects(float value) { PlayerPrefs.SetFloat("EffectsVolume", value); AudioDirector.Instance?.ApplyVolume(); }
    }
}
