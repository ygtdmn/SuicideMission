using UnityEngine;

namespace SuicideMission.Behavior
{
    public class DamageDealer : MonoBehaviour
    {
        private int damage;

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}