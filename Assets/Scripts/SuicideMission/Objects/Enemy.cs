using SuicideMission.Enums;
using UnityEngine;

namespace SuicideMission.Objects
{
    public class Enemy : Spaceship
    {
        [Header("Enemy Specific Specs")]
        [SerializeField] private float minTimeBetweenShots = 0.2f;
        [SerializeField] private float maxTimeBetweenShots = 3f;
        [SerializeField] private float shootChance = 1;
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
            firingSpeed = Random.Range(MinTimeBetweenShots, MaxTimeBetweenShots);
        }

        public override void Fire()
        {
            firingSpeed -= Time.deltaTime;
            if (firingSpeed <= 0f)
            {
                if (Random.Range(0f, 1f) <= ShootChance)
                {
                    Shoot(Direction.Down);
                }

                InitializeFiringSpeed();
            }
        }

        protected override void Death()
        {
            base.Death();
            gameSession.AddScore(ScoreToGive);
        }

        public float MinTimeBetweenShots
        {
            get => minTimeBetweenShots;
            set => minTimeBetweenShots = value;
        }
        public float MaxTimeBetweenShots
        {
            get => maxTimeBetweenShots;
            set => maxTimeBetweenShots = value;
        }
        public float ShootChance
        {
            get => shootChance;
            set => shootChance = value;
        }
        public int ScoreToGive
        {
            get => scoreToGive;
            set => scoreToGive = value;
        }
    }
}