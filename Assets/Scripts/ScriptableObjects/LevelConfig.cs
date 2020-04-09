using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private List<WaveConfig> waveConfigs = null;
    [SerializeField] private int startingWave = 0;
}