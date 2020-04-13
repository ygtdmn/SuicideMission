using SuicideMission.Objects;
using TMPro;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class HealthDisplay : MonoBehaviour
    {
        private TextMeshProUGUI healthText;
        private Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            healthText.text = player.GetHealth().ToString();
        }
    }
}