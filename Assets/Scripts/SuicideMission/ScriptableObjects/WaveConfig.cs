﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuicideMission.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Enemy Wave Config")]
    public class WaveConfig : ScriptableObject
    {
        [SerializeField] private GameObject enemyPrefab = null;
        [SerializeField] private GameObject pathPrefab = null;
        [SerializeField] private float timeBetweenSpawns = 0.5f;
        [SerializeField] private float spawnRandomFactor = 0.3f;
        [SerializeField] private int numberOfEnemies = 5;
        [SerializeField] private bool continuousSpawning = false;
        [SerializeField] private bool pathLooping = true;
        [SerializeField] private bool destroyAfterPathEnded = false;

        public GameObject GetEnemyPrefab()
        {
            return enemyPrefab;
        }

        public GameObject GetPathPrefab()
        {
            return pathPrefab;
        }

        public float GetTimeBetweenSpawns()
        {
            return timeBetweenSpawns;
        }

        public float GetSpawnRandomFactor()
        {
            return spawnRandomFactor;
        }

        public int GetNumberOfEnemies()
        {
            return numberOfEnemies;
        }

        public bool GetContinuousSpawning()
        {
            return continuousSpawning;
        }

        public bool GetDestroyAfterPathEnded()
        {
            return destroyAfterPathEnded;
        }

        public bool GetPathLooping()
        {
            return pathLooping;
        }

        public List<Transform> GetWaypoints()
        {
            return (from Transform child in pathPrefab.transform select child.transform).ToList();
        }
    }
}