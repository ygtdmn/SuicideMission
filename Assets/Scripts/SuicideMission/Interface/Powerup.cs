using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Interface
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] protected float spawnChance;
        [SerializeField] protected float dropSpeed = 5f;
        [SerializeField] private float destroyAfterY = -15f;
        [SerializeField] private AudioClip powerupSound;
        [SerializeField] private float powerupSoundVolume = 0.75f;

        private void Update()
        {
            if (transform.position.y < destroyAfterY) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>() != null)
            {
                HidePowerup();
                AudioSource.PlayClipAtPoint(powerupSound, Camera.main.transform.position, powerupSoundVolume);
                Execute(other.gameObject);
            }
        }

        private void HidePowerup()
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
        }

        protected abstract void Execute(GameObject gameObject);

        public float GetSpawnChance()
        {
            return spawnChance;
        }

        public float GetDropSpeed()
        {
            return dropSpeed;
        }
    }
}