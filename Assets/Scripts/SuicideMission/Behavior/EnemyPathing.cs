using System.Collections.Generic;
using System.Linq;
using SuicideMission.Objects;
using SuicideMission.ScriptableObjects;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class EnemyPathing : MonoBehaviour
    {
        private EnemyWaveConfig enemyWaveConfig;
        private List<Transform> waypoints;
        private readonly Dictionary<GameObject, int> enemies = new Dictionary<GameObject, int>(); // Enemy, Current Waypoint Index
        private readonly Dictionary<GameObject, int> enemiesGoingTo = new Dictionary<GameObject, int>(); // Enemy, Target Waypoint Index
        private readonly List<Transform> fullWaypoints = new List<Transform>();

        private void Update()
        {
            foreach (var enemyObject in new List<GameObject>(enemies.Keys))
                if (enemyObject == null)
                {
                    enemies.Remove(enemyObject);
                    enemiesGoingTo.Remove(enemyObject);
                }
                else
                {
                    Move(enemyObject);
                }
        }

        private void Move(GameObject enemyObject)
        {
            var enemy = enemyObject.GetComponent<Enemy>();
            var waypointIndex = enemies[enemyObject];

            if (enemyWaveConfig.PathLooping)
            {
                if (waypointIndex >= waypoints.Count) waypointIndex = 0;
            }

            if (enemiesGoingTo.ContainsKey(enemyObject) && waypointIndex <= enemiesGoingTo[enemyObject])
            {
                var targetWaypoint = waypoints[waypointIndex].transform;
                var targetPosition = targetWaypoint.position;
                var movemeentThisFrame = enemy.MoveSpeed * Time.deltaTime;
                enemy.transform.position =
                    Vector2.MoveTowards(enemy.transform.position, targetPosition, movemeentThisFrame);

                if (enemy.transform.position == targetPosition)
                {
                    if (enemyWaveConfig.DestroyAfterPathEnded && enemiesGoingTo[enemyObject] <= waypointIndex)
                    {
                        Destroy(enemyObject);
                    }
                    
                    enemies[enemyObject] = ++waypointIndex;
                }
            }
        }

        public void SetEnemyWaveConfig(EnemyWaveConfig enemyWaveConfig)
        {
            this.enemyWaveConfig = enemyWaveConfig;
            waypoints = enemyWaveConfig.GetWaypoints();
        }

        public void AddEnemy(GameObject enemy)
        {
            List<Transform> emptyWaypoints = waypoints.Except(fullWaypoints).ToList();
            enemies.Add(enemy, 0);
            if (!enemyWaveConfig.PathLooping && !enemyWaveConfig.DestroyAfterPathEnded && !enemyWaveConfig.ContinuousSpawning)
            {
                if (emptyWaypoints.Count <= 0)
                {
                    Debug.LogWarning("Waypoints are full. You shouldn't add more enemies! EnemyWaweConfig: " + enemyWaveConfig.name);
                }
                else
                {
                    enemiesGoingTo.Add(enemy, emptyWaypoints.IndexOf(emptyWaypoints.Last()));
                    fullWaypoints.Add(emptyWaypoints.Last());
                }
            }
            else
            {
                enemiesGoingTo.Add(enemy, waypoints.Count - 1);
            }

            enemy.transform.position = waypoints[0].position;
        }
    }
}