using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class HealthDisplay : MonoBehaviour
    {
        private SpriteRenderer lifeSpriteRenderer;
        private Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            lifeSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            float health = player.Health;
            float totalHealth = player.InitialHealth;

            lifeSpriteRenderer.transform.localScale =
                new Vector3(1, Mathf.Clamp(health / totalHealth, 0, 1), 1);
        }
    }
}