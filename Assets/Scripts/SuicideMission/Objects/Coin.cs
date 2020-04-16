using SuicideMission.Objects;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float dropSpeed = 5f;
    [SerializeField] private float destroyAfterY = -15f;
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private float coinPickupSoundVolume = 0.75f;
    [SerializeField] private int coinsToGive = 1;

    private void Update()
    {
        if (transform.position.y < destroyAfterY) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null) return;
        
        player.AddCoins(coinsToGive);
        AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position, coinPickupSoundVolume);
        Destroy(gameObject);
    }

    public float GetDropSpeed()
    {
        return dropSpeed;
    }
}