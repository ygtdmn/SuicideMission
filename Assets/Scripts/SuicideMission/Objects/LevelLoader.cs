using System;
using System.Collections;
using SuicideMission.Behavior;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SuicideMission.Objects
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private int gameOverLoadDelay = 2;

        [Header("Scenes")]
        [SerializeField] private Object startupScene;
        [SerializeField] private Object levelChooseScene;
        [SerializeField] private Object levelOverScene;
        [SerializeField] private Object loseScene;

        [Header("Game Levels")]
        [SerializeField] private Object[] levels;

        [Header("Scene Pitches")]
        [SerializeField] private float startMenuPitch = 1f;
        [SerializeField] private float gamePitch = 1f;
        [SerializeField] private float gameOverPitch = .5f;

        [Header("Game View")]
        [SerializeField] private float minX = -5.6f;
        [SerializeField] private float maxX = 5.6f;
        [SerializeField] private float minY = -10f;
        [SerializeField] private float maxY = 10f;

        public float MinX => minX;
        public float MaxX => maxX;
        public float MinY => minY;
        public float MaxY => maxY;

        public void Input() // Todo change name
        {
            if (SceneManager.GetActiveScene().name == startupScene.name
                || SceneManager.GetActiveScene().name == loseScene.name
                || SceneManager.GetActiveScene().name == levelOverScene.name)
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
            SceneManager.LoadScene(levelOverScene.name);
        }

        public void LoadStartupScene()
        {
            ResetSession();
            SceneManager.LoadScene(startupScene.name);
            SetPitch(startMenuPitch);
        }

        public void LoadLevelChooseScene()
        {
            ResetSession();
            SceneManager.LoadScene(levelChooseScene.name);
            SetPitch(startMenuPitch);
        }

        public void LoadLevel(String levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadFirstLevel()
        {
            ResetSession();
            SceneManager.LoadScene(levels[0].name);
            SetPitch(gamePitch);
        }

        public void LoadGameOver()
        {
            StartCoroutine(WaitAndLoadGameOver());
        }

        private IEnumerator WaitAndLoadGameOver()
        {
            yield return new WaitForSeconds(gameOverLoadDelay);
            SceneManager.LoadScene(loseScene.name);
            SetPitch(gameOverPitch);
        }

        public IEnumerator WaitAndLoadGameOver(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(loseScene.name);
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
        
        public Object StartupScene => startupScene;
        public Object LevelChooseScene => levelChooseScene;
        public Object LevelOverScene => levelOverScene;
        public Object LoseScene => loseScene;

        public Object[] Levels => levels;
    }
}