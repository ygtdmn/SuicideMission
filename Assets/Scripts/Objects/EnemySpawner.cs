using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<LevelConfig> levelConfigs;
    [SerializeField] private int startingLevel;
    [SerializeField] private GameObject enemyPathingObject;
    [SerializeField] private TextMeshProUGUI waveText;

    private string initialWaveText;

    private void Start()
    {
        initialWaveText = waveText.text.Replace("%totalWave%", levelConfigs.Count.ToString());
        waveText.text =
            initialWaveText.Replace("%wave%", startingLevel.ToString());
        waveText.enabled = false;
        StartCoroutine(SpawnAllLevels());
    }

    private IEnumerator SpawnAllLevels()
    {
        for (int levelIndex = startingLevel; levelIndex < levelConfigs.Count; levelIndex++)
        {
            LevelConfig currentLevel = levelConfigs[levelIndex];
            for (int i = currentLevel.GetStartingWave; i < currentLevel.GetWaveConfigs.Count; i++)
            {
                WaveConfig currentWave = currentLevel.GetWaveConfigs[i];
                var enemyPathing = Instantiate(enemyPathingObject).GetComponent<EnemyPathing>();
                enemyPathing.SetWaveConfig(currentWave);
                StartCoroutine(SpawnAllEnemiesInWave(currentWave, enemyPathing));
                yield return new WaitForSeconds(currentLevel.GetTimeBetweenWaveSpawns);
            }

            // Check if enemies are alive at this point
            yield return new WaitWhile(() => FindObjectsOfType<Enemy>().Length > 0);

            //Enemies are dead. Do your show and lets spawn the new level.
            StartCoroutine(WaveOverShow(levelIndex + 1, levelIndex + 2));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig, EnemyPathing enemyPathing)
    {
        do
        {
            for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
            {
                var enemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypoints()[0].position,
                    Quaternion.identity);
                enemyPathing.AddEnemy(enemy);
                yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
            }
        } while (waveConfig.GetContinuousSpawning);
    }

    private IEnumerator WaveOverShow(int oldLevel, int newLevel)
    {
        waveText.text = initialWaveText.Replace("%wave%", newLevel.ToString());
        waveText.enabled = true;
        yield return new WaitForSeconds(3f);
        waveText.enabled = false;
    }
}