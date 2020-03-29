using System.Collections;
using Enums;
using SuicideMission.Interface;
using UnityEngine;

public class Player : Spaceship
{
    [Header("Player Specific Specs")]
    [SerializeField] private float firingDelay = 0.1f;
    [SerializeField] private float padding = 0.75f;

    private Coroutine firingCoroutine;

    // Movement Boundaries
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    protected override void Start()
    {
        base.Start();
        SetupMoveBoundaries();
    }

    protected override void Update()
    {
        Move();
        Fire();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        Powerup powerup = other.gameObject.GetComponent<Powerup>();
        if (powerup == null) return;
        powerup.Perform(gameObject);
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    protected override void Fire()
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
            Shoot(Direction.Up);
            yield return new WaitForSeconds(firingDelay);
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

    protected override void Death()
    {
        base.Death();
        FindObjectOfType<Level>().LoadGameOver();
    }

    // Getters
    public float GetFiringDelay() => firingDelay;
    public float SetFiringDelay(float firingDelay) => this.firingDelay = firingDelay;

    // Setters
}