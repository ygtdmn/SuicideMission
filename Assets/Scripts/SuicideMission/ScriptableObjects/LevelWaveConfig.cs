using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SuicideMission.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Level Config")]
    public class LevelWaveConfig : ScriptableObject
    {
        [SerializeField] private List<EnemyWaveConfig> enemyWaveConfigs;
        [SerializeField] private int startingEnemyWave;
        [SerializeField] private float timeBetweenEnemyWaveSpawns = 1f;
        [SerializeField] private AudioClip levelWaveMusic;
        [SerializeField] private bool isFinalLevelWave;

        public List<EnemyWaveConfig> GetEnemyWaveConfigs()
        {
            return enemyWaveConfigs;
        }

        public int GetStartingEnemyWave()
        {
            return startingEnemyWave;
        }

        public float GetTimeBetweenEnemyWaveSpawns()
        {
            return timeBetweenEnemyWaveSpawns;
        }

        public AudioClip GetLevelWaveMusic()
        {
            return levelWaveMusic;
        }

        public bool IsFinalLevel() => isFinalLevelWave;
    }
}