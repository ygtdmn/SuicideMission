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


        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Game") return;

            if (Input.GetButtonDown("Fire1")) FindObjectOfType<Level>().LoadGameScene();
        }

        public void LoadStartMenu()
        {
            ResetSession();
            SceneManager.LoadScene("Start Menu");
            FindObjectOfType<MusicPlayer>().setPitch(startMenuPitch);
        }

        public void LoadGameScene()
        {
            ResetSession();
            SceneManager.LoadScene("Game");
            FindObjectOfType<MusicPlayer>().setPitch(gamePitch);
        }

        public void LoadGameOver()
        {
            StartCoroutine(WaitAndLoadGameOver());
        }

        private IEnumerator WaitAndLoadGameOver()
        {
            yield return new WaitForSeconds(gameOverLoadDelay);
            SceneManager.LoadScene("Game Over");
            FindObjectOfType<MusicPlayer>().setPitch(gameOverPitch);
        }

        public IEnumerator WaitAndLoadGameOver(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("Game Over");
            FindObjectOfType<MusicPlayer>().setPitch(gameOverPitch);
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