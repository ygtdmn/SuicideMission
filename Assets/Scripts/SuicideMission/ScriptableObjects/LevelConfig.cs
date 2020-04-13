using System.Collections.Generic;
using UnityEngine;

namespace SuicideMission.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<WaveConfig> waveConfigs;
        [SerializeField] private int startingWave;
        [SerializeField] private float timeBetweenWaveSpawns = 1f;
        [SerializeField] private AudioClip levelMusic;

        public List<WaveConfig> GetWaveConfigs()
        {
            return waveConfigs;
        }

        public int GetStartingWave()
        {
            return startingWave;
        }

        public float GetTimeBetweenWaveSpawns()
        {
            return timeBetweenWaveSpawns;
        }

        public AudioClip GetLevelMusic()
        {
            return levelMusic;
        }
    }
}