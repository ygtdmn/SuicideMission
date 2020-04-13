using UnityEngine;

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
            if (FindObjectsOfType(GetType()).Length > 1)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }

        public void SetScore(int score)
        {
            this.score = score;
        }

        public void AddScore(int scoreToGive)
        {
            score += scoreToGive;
        }

        public void RemoveScore(int scoreToTake)
        {
            score -= scoreToTake;
        }

        public int GetScore()
        {
            return score;
        }

        public void ResetGame()
        {
            Destroy(gameObject);
        }
    }
}