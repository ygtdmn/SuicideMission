using System.Collections;
using SuicideMission.Behavior;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuicideMission.Objects
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private int gameOverLoadDelay = 2;

        [Header("Scenes")]
        [SerializeField] private string startupScene;
        [SerializeField] private string levelChooseScene;
        [SerializeField] private string levelOverScene;
        [SerializeField] private string loseScene;

        [Header("Game Levels")]
        [SerializeField] private string[] levels;

        [Header("Scene Pitches")]
        [SerializeField] private float startMenuPitch = 1f;
        [SerializeField] private float gamePitch = 1f;
        [SerializeField] private float gameOverPitch = .5f;

        [Header("Game View")]
        [SerializeField] private float minX = -5.6f;
        [SerializeField] private float maxX = 5.6f;
        [SerializeField] private float minY = -10f;
        [SerializeField] private float maxY = 10f;

        [Header("Pause")]
        [SerializeField] private GameObject pauseIndicator;

        public float MinX => minX;
        public float MaxX => maxX;
        public float MinY => minY;
        public float MaxY => maxY;

        public void Input() // Todo change name
        {
            if (SceneManager.GetActiveScene().name == startupScene
                || SceneManager.GetActiveScene().name == loseScene
                || SceneManager.GetActiveScene().name == levelOverScene)
            {
                LoadLevelChooseScene();
            }
        }

        public void LoadLevelOverScene()
        {
            StartCoroutine(WaitAndLoadLevelOver());
        }

        private IEnumerator WaitAndLoadLevelOver()
        {
            yield return new WaitForSeconds(gameOverLoadDelay);
            SceneManager.LoadScene(levelOverScene);
        }

        public void LoadStartupScene()
        {
            ResetSession();
            SceneManager.LoadScene(startupScene);
            SetPitch(startMenuPitch);
        }

        public void LoadLevelChooseScene()
        {
            ResetSession();
            SceneManager.LoadScene(levelChooseScene);
            SetPitch(startMenuPitch);
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadFirstLevel()
        {
            ResetSession();
            SceneManager.LoadScene(levels[0]);
            SetPitch(gamePitch);
        }

        public void LoadGameOver()
        {
            StartCoroutine(WaitAndLoadGameOver());
        }

        private IEnumerator WaitAndLoadGameOver()
        {
            yield return new WaitForSeconds(gameOverLoadDelay);
            SceneManager.LoadScene(loseScene);
            SetPitch(gameOverPitch);
        }

        public IEnumerator WaitAndLoadGameOver(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(loseScene);
            SetPitch(gameOverPitch);
        }

        private void SetPitch(float pitch)
        {
            var musicPlayer = FindObjectOfType<MusicPlayer>();
            if (musicPlayer != null)
            {
                musicPlayer.setPitch(pitch);
            }
        }

        private void ResetSession()
        {
            var gameSession = FindObjectOfType<GameSession>();
            if (gameSession != null) gameSession.ResetGame();
        }

        public void PauseGame()
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                pauseIndicator.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pauseIndicator.SetActive(false);
            }
        }

        public string StartupScene => startupScene;
        public string LevelChooseScene => levelChooseScene;
        public string LevelOverScene => levelOverScene;
        public string LoseScene => loseScene;

        public string[] Levels => levels;
    }
}