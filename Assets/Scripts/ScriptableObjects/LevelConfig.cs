using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private List<WaveConfig> waveConfigs;
    [SerializeField] private int startingWave;
    [SerializeField] private float timeBetweenWaveSpawns = 1f;

    public List<WaveConfig> GetWaveConfigs => waveConfigs;
    public int GetStartingWave => startingWave;
    public float GetTimeBetweenWaveSpawns => timeBetweenWaveSpawns;
}