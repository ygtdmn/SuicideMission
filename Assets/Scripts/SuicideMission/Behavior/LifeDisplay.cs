using SuicideMission.Objects;
using TMPro;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class LifeDisplay : MonoBehaviour
    {
        private TextMeshProUGUI lifeText;
        private Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            lifeText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            lifeText.text = player.GetLifes().ToString();
        }
    }
}