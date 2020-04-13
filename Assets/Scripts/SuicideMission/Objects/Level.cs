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

        public Vector3 BottomLeft { get; private set; }
        public Vector3 BottomRight { get; private set; }
        public Vector3 TopLeft { get; private set; }
        public Vector3 TopRight { get; private set; }

        public float MinX => minX;
        public float MaxX => maxX;
        public float MinY => minY;
        public float MaxY => maxY;

        private void Start()
        {
            BottomLeft = new Vector3(minX, minY, 0);
            BottomRight = new Vector3(maxX, minY, 0);
            TopLeft = new Vector3(minX, maxY, 0);
            TopRight = new Vector3(maxX, maxY, 0);
        }

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