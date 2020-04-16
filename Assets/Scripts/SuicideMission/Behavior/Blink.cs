using UnityEngine;

namespace SuicideMission.Behavior
{
    public class Blink : MonoBehaviour
    {
        [SerializeField] private float minAlpha = 0.25f;
        [SerializeField] private float maxAlpha = 0.75f;
        [SerializeField] private float flashTime = 0.7f;

        private float initialAlpha;

        private bool active;

        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            initialAlpha = spriteRenderer.color.a;
        }

        private void Update()
        {
            if (!active) return;
            var color = spriteRenderer.color;
            var blinkingAlpha = minAlpha + Mathf.PingPong(Time.time / flashTime, maxAlpha - minAlpha);
            spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp(blinkingAlpha, 0.0f, 1.0f));
        }

        public void SetActive(bool active)
        {
            if (!active)
            {
                var color = spriteRenderer.color;
                spriteRenderer.color = new Color(color.r, color.g, color.b, initialAlpha);
            }
            else
            {
                initialAlpha = spriteRenderer.color.a;
            }

            this.active = active;
        }
    }
}