using System.Collections;
using SuicideMission.Behavior;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuicideMission.Objects
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float startMenuPitch = 1f;
        [SerializeField] private float gamePitch = 1f;
        [SerializeField] private float gameOverPitch = .5f;
        [SerializeField] private int gameOverLoadDelay = 2;

        [Header("Game View")]
        [SerializeField] private float minX = -5.6f;
        [SerializeField] private float maxX = 5.6f;
        [SerializeField] private float minY = -10f;
        [SerializeField] private float maxY = 10f;

        public float MinX => minX;
        public float MaxX => maxX;
        public float MinY => minY;
        public float MaxY => maxY;

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "1. Startup Scene")
            {
                if (Input.GetButtonDown("Fire1")) LoadLevelChooseScene();
            }
            
            if (SceneManager.GetActiveScene().name == "2. Level Choose Scene")
            {
                if (Input.GetButtonDown("Fire1")) LoadFirstLevel();
            }
            
            if (SceneManager.GetActiveScene().name == "4. Lose Scene")
            {
                if (Input.GetButtonDown("Fire1")) LoadLevelChooseScene();
            }
        }

        public void LoadLevelOverScene()
        {
            StartCoroutine(WaitAndLoadLevelOver());
        }
        
        private IEnumerator WaitAndLoadLevelOver()
        {
            yield return new WaitForSeconds(gameOverLoadDelay);
            SceneManager.LoadScene("3. Level Over");
        }

        public void LoadStartupScene()
        {
            ResetSession();
            SceneManager.LoadScene("1. Startup Scene");
            SetPitch(startMenuPitch);
        }
        
        public void LoadLevelChooseScene()
        {
            ResetSession();
            SceneManager.LoadScene("2. Level Choose Scene");
            SetPitch(startMenuPitch);
        }

        public void LoadFirstLevel()
        {
            ResetSession();
            SceneManager.LoadScene("Level 1");
            SetPitch(gamePitch);
        }

        public void LoadGameOver()
        {
            StartCoroutine(WaitAndLoadGameOver());
        }

        private IEnumerator WaitAndLoadGameOver()
        {
            yield return new WaitForSeconds(gameOverLoadDelay);
            SceneManager.LoadScene("4. Lose Scene");
            SetPitch(gameOverPitch);
        }

        public IEnumerator WaitAndLoadGameOver(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("4. Lose Scene");
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

        public void QuitGame()
        {
            Application.Quit();
        }

        private void ResetSession()
        {
            var gameSession = FindObjectOfType<GameSession>();
            if (gameSession != null) gameSession.ResetGame();
        }
    }
}