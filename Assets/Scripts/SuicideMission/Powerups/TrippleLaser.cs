using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Powerups
{
    public class TrippleLaser : Powerup
    {
        private Player player;
        [SerializeField] protected float duration = 5f;

        protected override void Execute(GameObject gameObject)
        {
            player = gameObject.GetComponent<Player>();
            player.GiveTrippleLaserBoost(duration);
            Destroy(this.gameObject);
        }
    }
}