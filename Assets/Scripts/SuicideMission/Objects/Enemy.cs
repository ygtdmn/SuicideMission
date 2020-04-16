using SuicideMission.Enums;
using UnityEngine;

namespace SuicideMission.Objects
{
    public class Enemy : Spaceship
    {
        [Header("Enemy Specific Specs")]
        [SerializeField] private float minTimeBetweenShots = 0.2f;
        [SerializeField] private float maxTimeBetweenShots = 3f;
        [SerializeField] private int scoreToGive = 10;

        private float firingSpeed;
        private GameSession gameSession;

        protected override void Start()
        {
            base.Start();
            InitializeFiringSpeed();
            gameSession = FindObjectOfType<GameSession>();
        }

        protected override void Update()
        {
            Fire();
        }

        private void InitializeFiringSpeed()
        {
            firingSpeed = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }

        protected override void Fire()
        {
            firingSpeed -= Time.deltaTime;
            if (firingSpeed <= 0f)
            {
                Shoot(Direction.Down);
                InitializeFiringSpeed();
            }
        }

        protected override void Death()
        {
            base.Death();
            gameSession.AddScore(scoreToGive);
        }
    }
}