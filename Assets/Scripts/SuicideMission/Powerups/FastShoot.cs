using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Powerups
{
    public class FastShoot : Powerup
    {
        private Player player;
        [SerializeField] protected float boost = 2f;
        [SerializeField] protected float duration;

        protected override void Execute(GameObject gameObject)
        {
            player = gameObject.GetComponent<Player>();
            player.GiveSpeedBoost(boost, duration);
            Destroy(this.gameObject);
        }
    }
}