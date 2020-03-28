using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 0.75f;
    [SerializeField] private int health = 200;

    [Header("Projectile")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0,1)] private float deathSoundVolume = 1f;
    [SerializeField] [Range(0,1)] private float shootSoundVolume = 0.1f;

    private float destroyBulletAfterSeconds;
    private Coroutine firingCoroutine;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    void Start()
    {
        SetupMoveBoundaries();
        destroyBulletAfterSeconds = Camera.main.orthographicSize * 2 / projectileSpeed;
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laserBean = Instantiate(laser, transform.position, Quaternion.identity);
            laserBean.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            Destroy(laserBean, destroyBulletAfterSeconds);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer == null) return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<Level>().LoadGameOver();
    }

    public int GetHealth() => health;
}