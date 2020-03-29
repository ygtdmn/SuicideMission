using System;
using SuicideMission;
using SuicideMission.Interface;
using UnityEngine;
using UnityEngine.UI;

public class HitIndicator : MonoBehaviour
{
    [Header("Particle Effect")]
    [SerializeField] private GameObject hitParticle;

    [Header("Sprites")]
    [SerializeField] private Sprite[] hitSprites;

    [Header("Color")]
    [SerializeField] private bool colorActive = false;
    [SerializeField] private Gradient gradient;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] [Range(0, 1)] private float soundEffectVolume = 0.5f;

    [Header("Hit Background")]
    [SerializeField] private GameObject hitBackgroundObject;
    [SerializeField] private float minAlpha = 0f;
    [SerializeField] private float maxAlpha = 0.3f;

    private SpriteRenderer spriteRenderer;
    private Spaceship spaceship;
    private Image image;

    private int initialHealth;

    private bool backgroundChangeInvoked = false;
    private float backgroundChangeTimer = 0;

    private void Start()
    {
        spaceship = GetComponent<Spaceship>();
        initialHealth = spaceship.GetHealth();

        if (hitSprites.Length > 0 || colorActive)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (hitBackgroundObject != null)
        {
            image = hitBackgroundObject.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (backgroundChangeInvoked)
            backgroundChangeTimer += Time.deltaTime;
    }

    public void IndicateHit()
    {
        if (hitParticle != null)
        {
            Utils.PlayParticle(hitParticle, transform, transform);
        }

        if (hitSprites.Length > 0)
        {
            ChangeSprite();
        }

        if (colorActive)
        {
            ChangeColor();
        }

        if (soundEffect != null)
        {
            AudioSource.PlayClipAtPoint(soundEffect, Camera.main.transform.position, soundEffectVolume);
        }

        if (hitBackgroundObject != null)
        {
            if (!backgroundChangeInvoked)
            {
                backgroundChangeInvoked = true;
                InvokeRepeating(nameof(ChangeIndicatorBackgroundColor), 0, Time.deltaTime);
            }
        }
    }

    private void ChangeSprite()
    {
        var health = spaceship.GetHealth();
        var index = hitSprites.Length - Mathf.CeilToInt(health / (initialHealth / (hitSprites.Length + 1)));
        index = Mathf.Min(index, hitSprites.Length);
        spriteRenderer.sprite = hitSprites[index];
    }

    private void ChangeColor()
    {
        var health = spaceship.GetHealth();
        spriteRenderer.color = gradient.Evaluate((float) health / initialHealth);
    }

    private void ChangeIndicatorBackgroundColor()
    {
        float blinkingAlpha = Mathf.PingPong(backgroundChangeTimer, maxAlpha);
        if (backgroundChangeTimer > maxAlpha && Math.Round(blinkingAlpha, 1) == Math.Round(minAlpha, 1))
        {
            CancelInvoke(nameof(ChangeIndicatorBackgroundColor));
            blinkingAlpha = 0; // Put there in case minAlpha is more than 0.
            backgroundChangeInvoked = false;
            backgroundChangeTimer = 0;
        }

        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, blinkingAlpha);
    }
}