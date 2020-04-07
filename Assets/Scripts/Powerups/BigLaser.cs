using Interface;
using UnityEngine;

namespace Powerups
{
    public class BigLaser : Powerup
    {
        private Player player;
        [SerializeField] protected float sizeBoost = 2f;
        [SerializeField] protected float duration;

        protected override void Execute(GameObject gameObject)
        {
            player = gameObject.GetComponent<Player>();
            player.GiveLaserSizeBoost(sizeBoost, duration);
            Destroy(this.gameObject);
        }
    }
}