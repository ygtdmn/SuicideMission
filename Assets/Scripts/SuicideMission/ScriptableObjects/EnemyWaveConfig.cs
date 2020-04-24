using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuicideMission.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Enemy Wave Config")]
    public class EnemyWaveConfig : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject pathPrefab;

        [Header("Wave Settings")]
        [SerializeField] private float minTimeBetweenSpawns = 0.5f;
        [SerializeField] private float maxTimeBetweenSpawns = 1.5f;
        [SerializeField] private int numberOfEnemies = 5;
        [SerializeField] private float spawnDelay;
        [SerializeField] private bool continuousSpawning;
        [SerializeField] private bool pathLooping = true;
        [SerializeField] private bool destroyAfterPathEnded;

        [Header("Enemy Settings")]
        [SerializeField] private int health;
        [SerializeField] private float moveSpeed;
        [SerializeField] protected int projectileDamage;
        [SerializeField] protected int collisionDamage;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] private float minTimeBetweenShots;
        [SerializeField] private float maxTimeBetweenShots;
        [SerializeField] private float shootChance;
        [SerializeField] private int scoreToGive;

        // ******* REFERENCES *******
        public GameObject EnemyPrefab => enemyPrefab;
        // ******* WAVE SETTINGS *******
        public float MinTimeBetweenSpawns => minTimeBetweenSpawns;
        public float MaxTimeBetweenSpawns => maxTimeBetweenSpawns;
        public int NumberOfEnemies => numberOfEnemies;
        public float SpawnDelay => spawnDelay;
        public bool ContinuousSpawning => continuousSpawning;
        public bool PathLooping => pathLooping;
        public bool DestroyAfterPathEnded => destroyAfterPathEnded;
        public List<Transform> GetWaypoints()
        {
            return (from Transform child in pathPrefab.transform select child.transform).ToList();
        }
        // ******* ENEMY SETTINGS *******
        public int Health => health;
        public float MoveSpeed => moveSpeed;
        public int ProjectileDamage => projectileDamage;
        public int CollisionDamage => collisionDamage;
        public float ProjectileSpeed => projectileSpeed;
        public float MinTimeBetweenShots => minTimeBetweenShots;
        public float MaxTimeBetweenShots => maxTimeBetweenShots;
        public float ShootChance => shootChance;
        public int ScoreToGive => scoreToGive;
    }
}