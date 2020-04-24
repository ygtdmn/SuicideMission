using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Powerups
{
    public class HealthIncreaser : Powerup
    {
        [SerializeField] private int healthToAdd;
        private Player player;

        protected override void Execute(GameObject gameObject)
        {
            player = gameObject.GetComponent<Player>();
            AddHealth();
        }

        private void AddHealth()
        {
            player.Health += healthToAdd;
            Destroy(gameObject);
        }
    }
}