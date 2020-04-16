using SuicideMission.Objects;
using TMPro;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class CoinDisplay : MonoBehaviour
    {
        private TextMeshProUGUI coinText;
        private Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            coinText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            coinText.text = player.GetCoins().ToString();
        }
    }
}