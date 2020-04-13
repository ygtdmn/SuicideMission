using TMPro;
using UnityEngine;

public class GlowingEffect : MonoBehaviour
{
    [SerializeField] private float minAlpha = 0.25f;
    [SerializeField] private float maxAlpha = 0.75f;
    [SerializeField] private float flashTime = 0.7f;

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float blinkingAlpha = minAlpha + Mathf.PingPong(Time.time / flashTime, (maxAlpha - minAlpha));

        var color = text.color;
        text.color = new Color(color.r, color.g, color.b, Mathf.Clamp(blinkingAlpha, 0.0f, 1.0f));
    }
}