using Interface;
using UnityEngine;

namespace Powerups
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
            player.SetHealth(player.GetHealth() + healthToAdd);
            Destroy(this.gameObject);
        }
    }
}