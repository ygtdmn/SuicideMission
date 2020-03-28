using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private float startMenuPitch = 1f;
    [SerializeField] private float gamePitch = 1f;
    [SerializeField] private float gameOverPitch = .5f;
    [SerializeField] private int gameOverLoadDelay = 2;

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

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetSession()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null)
        {
            gameSession.ResetGame();
        }
    }
}