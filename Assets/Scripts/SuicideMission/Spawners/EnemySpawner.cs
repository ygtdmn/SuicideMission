using System;
using System.Collections;
using System.Collections.Generic;
using SuicideMission.Behavior;
using SuicideMission.Objects;
using SuicideMission.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// ReSharper disable Unity.InefficientPropertyAccess
namespace SuicideMission.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<LevelWaveConfig> levelWaveConfigs;
        [FormerlySerializedAs("startingLevel")] [SerializeField]
        private int startingLevelWave;
        [SerializeField] private GameObject enemyPathingObject;
        [SerializeField] private TextMeshProUGUI waveText;
        [FormerlySerializedAs("timeBeforeWaves")] [SerializeField]
        private float timeBeforeLevelWaves = 5f;
        [SerializeField] private float waveIndicatorDuration = 4f;
        [SerializeField] private float gameOverDelay = 4f;

        private string initialWaveText;
        private AudioSource audioSource;
        private AudioClip defaultClip;

        private void Start()
        {
            InitAudioSource();
            StartCoroutine(InitWaveText());
            StartCoroutine(SpawnAllLevelWaves());
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
            initialWaveText = waveText.text.Replace("$2", levelWaveConfigs.Count.ToString());
            waveText.text = initialWaveText.Replace("$1", (startingLevelWave + 1).ToString());

            waveText.enabled = true;
            yield return new WaitForSeconds(waveIndicatorDuration);
            waveText.enabled = false;
        }

        private IEnumerator SpawnAllLevelWaves()
        {
            for (var levelWaveIndex = startingLevelWave; levelWaveIndex < levelWaveConfigs.Count; levelWaveIndex++)
            {
                var currentLevelWave = levelWaveConfigs[levelWaveIndex];
                StartCoroutine(SpawnLevelWave(currentLevelWave));

                // Check if enemies are alive at this point
                yield return new WaitWhile(() => FindObjectsOfType<Enemy>().Length > 0 || currentLevelWave.WaveSpawning);

                if (currentLevelWave.IsFinalLevel())
                {
                    LevelOver();
                }
                else
                {
                    NextLevelWave(levelWaveIndex);
                    yield return new WaitForSeconds(timeBeforeLevelWaves);
                }
            }
        }

        private IEnumerator SpawnLevelWave(LevelWaveConfig currentLevelWave)
        {
            currentLevelWave.WaveSpawning = true;
            ChangeMusic(currentLevelWave);
            for (var i = currentLevelWave.StartingEnemyWave; i < currentLevelWave.EnemyWaveConfigs.Count; i++)
            {
                SpawnEnemyWave(currentLevelWave, i);
                yield return new WaitForSeconds(currentLevelWave.TimeBetweenEnemyWaveSpawns);
            }
        }

        private void ChangeMusic(LevelWaveConfig currentLevelWave)
        {
            if (audioSource == null) return;
            var currentClip = audioSource.clip;
            var nextClip = defaultClip;

            if (currentLevelWave.LevelWaveMusic != null) nextClip = currentLevelWave.LevelWaveMusic;

            if (currentClip == nextClip) return;
            audioSource.clip = nextClip;
            audioSource.Play();
        }

        private void SpawnEnemyWave(LevelWaveConfig currentLevelWave, int i)
        {
            var currentEnemyWave = currentLevelWave.EnemyWaveConfigs[i];
            var enemyPathing = Instantiate(enemyPathingObject).GetComponent<EnemyPathing>();
            enemyPathing.SetEnemyWaveConfig(currentEnemyWave);
            StartCoroutine(SpawnAllEnemiesInEnemyWave(currentEnemyWave, enemyPathing, currentLevelWave,
                i == currentLevelWave.EnemyWaveConfigs.Count - 1));
        }

        private IEnumerator SpawnAllEnemiesInEnemyWave(EnemyWaveConfig enemyWaveConfig, EnemyPathing enemyPathing,
            LevelWaveConfig currentLevelWave, bool last)
        {
            yield return new WaitForSeconds(enemyWaveConfig.SpawnDelay);
            do
            {
                for (var i = 0; i < enemyWaveConfig.NumberOfEnemies; i++)
                {
                    SpawnEnemy(enemyWaveConfig, enemyPathing);
                    yield return new WaitForSeconds(Random.Range(enemyWaveConfig.MinTimeBetweenSpawns,
                        enemyWaveConfig.MaxTimeBetweenSpawns));
                }
            } while (enemyWaveConfig.ContinuousSpawning &&
                     (currentLevelWave.WaveSpawning || FindObjectsOfType<Enemy>().Length > 0));

            if (last)
            {
                currentLevelWave.WaveSpawning = false;
            }
        }

        private static void SpawnEnemy(EnemyWaveConfig enemyWaveConfig, EnemyPathing enemyPathing)
        {
            var enemy = Instantiate(
                enemyWaveConfig.EnemyPrefab,
                enemyWaveConfig.GetWaypoints()[0].position,
                Quaternion.identity);
            var enemyComp = enemy.GetComponent<Enemy>();
            SetEnemyAttributes(enemyWaveConfig, enemyComp);
            enemyPathing.AddEnemy(enemy);
        }

        private static void SetEnemyAttributes(EnemyWaveConfig enemyWaveConfig, Enemy enemyComp)
        {
            if (enemyWaveConfig.Health != 0) enemyComp.Health = enemyWaveConfig.Health;
            if (enemyWaveConfig.MoveSpeed != 0) enemyComp.MoveSpeed = enemyWaveConfig.MoveSpeed;
            if (enemyWaveConfig.ProjectileDamage != 0) enemyComp.ProjectileDamage = enemyWaveConfig.ProjectileDamage;
            if (enemyWaveConfig.CollisionDamage != 0) enemyComp.CollisionDamage = enemyWaveConfig.CollisionDamage;
            if (enemyWaveConfig.ProjectileSpeed != 0) enemyComp.ProjectileSpeed = enemyWaveConfig.ProjectileSpeed;
            if (enemyWaveConfig.MinTimeBetweenShots != 0)
                enemyComp.MinTimeBetweenShots = enemyWaveConfig.MinTimeBetweenShots;
            if (enemyWaveConfig.MaxTimeBetweenShots != 0)
                enemyComp.MaxTimeBetweenShots = enemyWaveConfig.MaxTimeBetweenShots;
            if (enemyWaveConfig.ShootChance != 0) enemyComp.ShootChance = enemyWaveConfig.ShootChance;
            if (enemyWaveConfig.ScoreToGive != 0) enemyComp.ScoreToGive = enemyWaveConfig.ScoreToGive;
        }

        private void NextLevelWave(int levelWaveIndex)
        {
            //Enemies are dead. Do your show and lets spawn the new level.
            int newLevelWave =
                levelWaveIndex + 2; // levelWaveIndex + 1 is current level. levelWaveIndex + 2 will give the new level.

            if (newLevelWave <= levelWaveConfigs.Count)
            {
                StartCoroutine(LevelWaveOver(newLevelWave));
            }
            else
            {
                LevelOver(); // It's here in case author forgot to set a final level.
            }
        }

        private IEnumerator LevelWaveOver(int newLevel)
        {
            waveText.text = initialWaveText.Replace("$1", newLevel.ToString());
            waveText.enabled = true;
            yield return new WaitForSeconds(waveIndicatorDuration);
            waveText.enabled = false;
        }

        private void LevelOver()
        {
            var level = FindObjectOfType<LevelLoader>();
            PlayerPrefs.SetInt("LastLevelBeaten",
                Convert.ToInt32(SceneManager.GetActiveScene().name.Replace("Level ", ""))); // Todo move it.
            if (level.Levels[level.Levels.Length - 1].Equals(SceneManager.GetActiveScene().name))
            {
                level.LoadWinScene();
            }
            else
            {
                level.LoadLevelOverScene();
            }
        }
    }
}