using System;
using UnityEngine;

namespace Interface
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] protected float spawnChance;
        [SerializeField] protected float dropSpeed = 5f;
        [SerializeField] private float destroyAfterY = -15f;

        private void Update()
        {
            if (transform.position.y < destroyAfterY)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>() != null)
            {
                HidePowerup();
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
        public float GetSpawnChance() => spawnChance;
        public float GetDropSpeed() => dropSpeed;
    }
}