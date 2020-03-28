using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource = null;

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