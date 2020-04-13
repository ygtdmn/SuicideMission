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
    [SerializeField] private float timeBeforeWaves = 5f;
    [SerializeField] private float waveIndicatorDuration = 4f;

    private string initialWaveText;
    private AudioSource audioSource;
    private AudioClip defaultClip;

    private IEnumerator Start()
    {
        audioSource = FindObjectOfType<MusicPlayer>().gameObject.GetComponent<AudioSource>();
        defaultClip = audioSource.clip;

        StartCoroutine(SpawnAllLevels());
        initialWaveText = waveText.text.Replace("$2", levelConfigs.Count.ToString());
        waveText.text =
            initialWaveText.Replace("$1", (startingLevel + 1).ToString());

        waveText.enabled = true;
        yield return new WaitForSeconds(waveIndicatorDuration);
        waveText.enabled = false;
    }

    private IEnumerator SpawnAllLevels()
    {
        for (int levelIndex = startingLevel; levelIndex < levelConfigs.Count; levelIndex++)
        {
            LevelConfig currentLevel = levelConfigs[levelIndex];
            if (currentLevel.GetLevelMusic() != null)
            {
                if (audioSource.clip != currentLevel.GetLevelMusic())
                {
                    audioSource.clip = currentLevel.GetLevelMusic();
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.clip != defaultClip)
                {
                    audioSource.clip = defaultClip;
                    audioSource.Play();
                }
            }

            for (int i = currentLevel.GetStartingWave(); i < currentLevel.GetWaveConfigs().Count; i++)
            {
                WaveConfig currentWave = currentLevel.GetWaveConfigs()[i];
                var enemyPathing = Instantiate(enemyPathingObject).GetComponent<EnemyPathing>();
                enemyPathing.SetWaveConfig(currentWave);
                StartCoroutine(SpawnAllEnemiesInWave(currentWave, enemyPathing));
                yield return new WaitForSeconds(currentLevel.GetTimeBetweenWaveSpawns());
            }

            // Check if enemies are alive at this point
            yield return new WaitWhile(() => FindObjectsOfType<Enemy>().Length > 0);

            //Enemies are dead. Do your show and lets spawn the new level.
            if (levelIndex + 2 <= levelConfigs.Count)
            {
                StartCoroutine(WaveOverShow(levelIndex + 1, levelIndex + 2));
                yield return new WaitForSeconds(timeBeforeWaves);
            }
            else
            {
                // Game over.
            }
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
        waveText.text = initialWaveText.Replace("$1", newLevel.ToString());
        waveText.enabled = true;
        yield return new WaitForSeconds(waveIndicatorDuration);
        waveText.enabled = false;
    }
}