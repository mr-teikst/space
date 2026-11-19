using System;
using System.IO;
using UnityEngine;
namespace ChickenShooter
{
    public sealed class SaveService : MonoBehaviour
    {
        [SerializeField] string fileName = "savegame.json";
        public string SavePath => Path.Combine(Application.persistentDataPath, fileName);
        public string LastError { get; private set; }
        public static bool IsValid(RunState value, LevelDataSO[] levels)
        {
            if (value == null || value.schemaVersion != 1 || levels == null || value.levelIndex < 1 || value.levelIndex > levels.Length) return false;
            var level = levels[value.levelIndex - 1];
            return level != null && level.rounds != null && value.roundIndex >= 1 && value.roundIndex <= level.rounds.Length
                && value.lives >= 1 && value.lives <= 3 && value.score >= 0 && value.missiles >= 0 && value.powerLevel >= 1 && value.powerLevel <= 3 && value.foodProgress >= 0;
        }
        public bool TryRead(LevelDataSO[] levels, out RunState state)
        {
            state = null; LastError = null;
            if (!File.Exists(SavePath)) return false;
            try
            {
                var json = File.ReadAllText(SavePath);
                foreach (string field in new[] {"schemaVersion","levelIndex","roundIndex","score","lives","missiles","powerLevel","foodProgress"})
                    if (!json.Contains("\"" + field + "\"")) return false;
                state = JsonUtility.FromJson<RunState>(json);
                if (IsValid(state, levels)) return true;
                state = null;
            }
            catch (Exception error) when (error is IOException || error is ArgumentException || error is UnauthorizedAccessException)
            { LastError = error.Message; }
            return false;
        }
        public bool Write(RunState state)
        {
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                string temporary = SavePath + ".tmp";
                File.WriteAllText(temporary, JsonUtility.ToJson(state, true));
                if (File.Exists(SavePath)) File.Replace(temporary, SavePath, null);
                else File.Move(temporary, SavePath);
                LastError = null; return true;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            { LastError = error.Message; Debug.LogError("Could not save progress: " + LastError); return false; }
        }
        public void Clear()
        {
            try { if (File.Exists(SavePath)) File.Delete(SavePath); }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            { LastError = error.Message; Debug.LogError("Could not clear progress: " + LastError); }
        }
    }
}
