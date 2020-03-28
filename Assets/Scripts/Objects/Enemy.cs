using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private float health = 100;
    [SerializeField] private float minTimeBetweenShots = 0.2f;
    [SerializeField] private float maxTimeBetweenShots = 3f;
    [SerializeField] private int scoreToGive = 10;
    
    [Header("Projectile")]
    [SerializeField] private GameObject laser;
    [SerializeField] private float projectileSpeed = 20f;
    
    [Header("Particles")]
    [SerializeField] private GameObject explosion;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0,1)] private float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0,1)] private float shootSoundVolume = 0.5f;
    
    private float destroyBulletAfterSeconds;
    private float shotCounter;
    private GameSession gameSession;

    void Start()
    {
        initializeShotCounter();
        destroyBulletAfterSeconds = Camera.main.orthographicSize * 2 / projectileSpeed;
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        CountdownAndShoot();
    }

    private void CountdownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void initializeShotCounter()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Fire()
    {
        GameObject laserBean = Instantiate(laser, transform.position, Quaternion.identity);
        laserBean.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        initializeShotCounter();
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        Destroy(laserBean, destroyBulletAfterSeconds);
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
            Death();
        }
    }

    private void Death()
    {
        var expl = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(expl, expl.GetComponent<ParticleSystem>().main.duration);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        gameSession.AddScore(scoreToGive);
    }
}