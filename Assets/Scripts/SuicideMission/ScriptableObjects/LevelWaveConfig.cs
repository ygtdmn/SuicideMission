using System.Collections.Generic;
using UnityEngine;

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

        // State
        public bool WaveSpawning { get; set; }

        public List<EnemyWaveConfig> EnemyWaveConfigs => enemyWaveConfigs;

        public int StartingEnemyWave => startingEnemyWave;

        public float TimeBetweenEnemyWaveSpawns => timeBetweenEnemyWaveSpawns;

        public AudioClip LevelWaveMusic => levelWaveMusic;

        public bool IsFinalLevel() => isFinalLevelWave;
    }
}