using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private float gameOverMusicPitch = 0.3f;
    private AudioSource audioSource;

    private void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void setPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }
}