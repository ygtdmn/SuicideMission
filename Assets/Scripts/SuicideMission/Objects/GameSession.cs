using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuicideMission.Objects
{
    public class GameSession : MonoBehaviour
    {
        private int score;

        private void Awake()
        {
            SetupSingleton();
        }

        private void SetupSingleton()
        {
            var levelLoader = FindObjectOfType<LevelLoader>();
            var sceneName = SceneManager.GetActiveScene().name;

            if (FindObjectsOfType(GetType()).Length > 1 &&
                !sceneName.Equals(levelLoader.LoseScene.name) && !sceneName.Equals(levelLoader.LevelOverScene.name))
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void SetScore(int score)
        {
            this.score = score;
            
            var highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HighScore");
            if (score > highScore)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
            }
        }

        public void AddScore(int scoreToGive)
        {
            SetScore(score + scoreToGive);
        }

        public void RemoveScore(int scoreToTake)
        {
            SetScore(score - scoreToTake);
        }

        public int GetScore() => score;

        public void ResetGame()
        {
            Destroy(gameObject);
        }
    }
}