using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ChickenShooter
{
    public sealed class AudioDirector : MonoBehaviour
    {
        [Serializable] public struct Cue { public string key; public AudioClip clip; }
        public static AudioDirector Instance { get; private set; }
        public AudioSource musicSource, effectsSource;
        public AudioClip menuMusic;
        public Cue[] cues;
        readonly Dictionary<string, float> lastPlayed = new Dictionary<string, float>();
        void Awake() { if (Instance && Instance != this) { enabled = false; return; } Instance = this; ApplyVolume(); }
        void OnEnable() { if (Instance != this) return; GameSignals.SoundRequested += Play; SceneManager.sceneLoaded += SceneLoaded; }
        void Start() { if (musicSource && !musicSource.isPlaying) PlayMusic(menuMusic); }
        void OnDisable() { GameSignals.SoundRequested -= Play; SceneManager.sceneLoaded -= SceneLoaded; if (Instance == this) Instance = null; }
        public void ApplyVolume()
        { if (musicSource) musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", .45f); if (effectsSource) effectsSource.volume = PlayerPrefs.GetFloat("EffectsVolume", .65f); }
        void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var controller = FindFirstObjectByType<LevelController>();
            PlayMusic(controller && controller.level ? controller.level.music : menuMusic);
        }
        void PlayMusic(AudioClip clip)
        { if (!musicSource || musicSource.clip == clip) return; musicSource.Stop(); musicSource.clip = clip; musicSource.loop = true; if (clip) musicSource.Play(); }
        public void Play(string key)
        {
            if (!effectsSource || cues == null) return;
            if (lastPlayed.TryGetValue(key, out float time) && Time.unscaledTime - time < .055f) return;
            foreach (var cue in cues) if (cue.key == key && cue.clip)
            { lastPlayed[key] = Time.unscaledTime; effectsSource.PlayOneShot(cue.clip); return; }
        }
    }
}
