using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

namespace Impaler.Score
{
    public class ScoreFileHandler : MonoBehaviour, IScoreFileHandler
    {
        private GameMaster gameMaster;
        public string FilePath { get; private set; }
        [SerializeField] private Text scoreText;
        
        private int _score;
        private static ScoreFileHandler scoreFileHandler;
        private static bool _wasInitialized = false;

        void Awake()
        {
            if(_wasInitialized)
            {
                Destroy(gameObject);
                return;
            }

            _wasInitialized = true;

            DontDestroyOnLoad(gameObject);
            scoreFileHandler = this;
            FilePath = Application.persistentDataPath + "/HighScore.json";

            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                _score = JsonUtility.FromJson<int>(json);
            }
            else _score = 0;

            gameMaster = null;
        }


        public static ScoreFileHandler GetScoreFileHandler()
        {
            if (scoreFileHandler == null)
            {
                Debug.LogError("Panic! No score file handler was found!");
                Application.Quit();
            }
            return scoreFileHandler;
        }

        private void OnApplicationFocus(bool focus)
        {
            //If the game lost focus while the game scene is active
            if (!focus && SceneManager.GetActiveScene().buildIndex > 0)
                UpdateScoreFile();
        }

        public void UpdateScoreFile()
        {
            if (gameMaster == null)//To prevent unnecessary calls for the GetGameMaster function
                gameMaster = GameMaster.GetGameMaster();

            if (gameMaster.score > _score)
            {
                _score = gameMaster.score;
                File.WriteAllText(FilePath, JsonUtility.ToJson(_score));
            }
        }

        public void ReadScoreFile()
        {
            Debug.Log("Score file read");
            scoreText.text = "High score: " + _score;
        }
    }
}



