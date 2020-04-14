using System.Collections;
using System.Collections.Generic;
using SuicideMission.Behavior;
using SuicideMission.Objects;
using SuicideMission.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace SuicideMission.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<LevelConfig> levelConfigs;
        [SerializeField] private int startingLevel;
        [SerializeField] private GameObject enemyPathingObject;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private float timeBeforeWaves = 5f;
        [SerializeField] private float waveIndicatorDuration = 4f;
        [SerializeField] private float gameOverDelay = 4f;

        private string initialWaveText;
        private AudioSource audioSource;
        private AudioClip defaultClip;

        private void Start()
        {
            InitAudioSource();
            StartCoroutine(InitWaveText());
            StartCoroutine(SpawnAllLevels());
        }

        private void InitAudioSource()
        {
            var musicPlayer = FindObjectOfType<MusicPlayer>();
            if (musicPlayer == null) return;

            audioSource = musicPlayer.GetComponent<AudioSource>();
            defaultClip = audioSource.clip;
        }

        private IEnumerator InitWaveText()
        {
            initialWaveText = waveText.text.Replace("$2", levelConfigs.Count.ToString());
            waveText.text = initialWaveText.Replace("$1", (startingLevel + 1).ToString());

            waveText.enabled = true;
            yield return new WaitForSeconds(waveIndicatorDuration);
            waveText.enabled = false;
        }

        private IEnumerator SpawnAllLevels()
        {
            for (var levelIndex = startingLevel; levelIndex < levelConfigs.Count; levelIndex++)
            {
                var currentLevel = levelConfigs[levelIndex];
                StartCoroutine(SpawnLevel(currentLevel));
                // Check if enemies are alive at this point
                yield return new WaitWhile(() => FindObjectsOfType<Enemy>().Length > 0);
                NextLevel(levelIndex);
                yield return new WaitForSeconds(timeBeforeWaves);
            }
        }

        private IEnumerator SpawnLevel(LevelConfig currentLevel)
        {
            ChangeMusic(currentLevel);
            for (var i = currentLevel.GetStartingWave(); i < currentLevel.GetWaveConfigs().Count; i++)
            {
                SpawnWave(currentLevel, i);
                yield return new WaitForSeconds(currentLevel.GetTimeBetweenWaveSpawns());
            }
        }

        private void ChangeMusic(LevelConfig currentLevel)
        {
            if (audioSource == null) return;
            var currentClip = audioSource.clip;
            var nextClip = defaultClip;

            if (currentLevel.GetLevelMusic() != null) nextClip = currentLevel.GetLevelMusic();

            if (currentClip == nextClip) return;
            audioSource.clip = nextClip;
            audioSource.Play();
        }

        private void SpawnWave(LevelConfig currentLevel, int i)
        {
            var currentWave = currentLevel.GetWaveConfigs()[i];
            var enemyPathing = Instantiate(enemyPathingObject).GetComponent<EnemyPathing>();
            enemyPathing.SetWaveConfig(currentWave);
            StartCoroutine(SpawnAllEnemiesInWave(currentWave, enemyPathing));
        }

        private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig, EnemyPathing enemyPathing)
        {
            do
            {
                for (var i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
                {
                    SpawnEnemy(waveConfig, enemyPathing);
                    yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
                }
            } while (waveConfig.GetContinuousSpawning());
        }

        private static void SpawnEnemy(WaveConfig waveConfig, EnemyPathing enemyPathing)
        {
            var enemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].position,
                Quaternion.identity);
            enemyPathing.AddEnemy(enemy);
        }

        private void NextLevel(int levelIndex)
        {
            //Enemies are dead. Do your show and lets spawn the new level.
            if (levelIndex + 2 <= levelConfigs.Count)
            {
                StartCoroutine(WaveOver(levelIndex + 1, levelIndex + 2));
            }
            else
            {
                GameOver();
            }
        }

        private IEnumerator WaveOver(int oldLevel, int newLevel)
        {
            waveText.text = initialWaveText.Replace("$1", newLevel.ToString());
            waveText.enabled = true;
            yield return new WaitForSeconds(waveIndicatorDuration);
            waveText.enabled = false;
        }

        private void GameOver()
        {
            var level = FindObjectOfType<Level>();
            StartCoroutine(level.WaitAndLoadGameOver(gameOverDelay));
        }
    }
}