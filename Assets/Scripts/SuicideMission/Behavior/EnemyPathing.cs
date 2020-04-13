using System.Collections.Generic;
using SuicideMission.Objects;
using SuicideMission.ScriptableObjects;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class EnemyPathing : MonoBehaviour
    {
        private WaveConfig waveConfig;
        private List<Transform> waypoints;
        private readonly Dictionary<GameObject, int>
            enemies = new Dictionary<GameObject, int>(); // Enemy, Waypoint Index
        private readonly List<int> fullWaypoints = new List<int>();

        private void Update()
        {
            foreach (var enemyObject in new List<GameObject>(enemies.Keys))
                if (enemyObject == null)
                    enemies.Remove(enemyObject);
                else
                    Move(enemyObject);
        }

        private void Move(GameObject enemyObject)
        {
            var enemy = enemyObject.GetComponent<Enemy>();
            var waypointIndex = enemies[enemyObject];

            if (waveConfig.GetDestroyAfterPathEnded() || !waveConfig.GetPathLooping())
                if (waypointIndex >= waypoints.Count || fullWaypoints.Contains(waypointIndex))
                {
                    if (waveConfig.GetDestroyAfterPathEnded())
                    {
                        Destroy(enemyObject);
                        return;
                    }

                    fullWaypoints.Add(waypointIndex - 1);
                    return;
                }

            if (waypointIndex >= waypoints.Count) waypointIndex = 0;

            if (waypointIndex < waypoints.Count && !fullWaypoints.Contains(waypointIndex))
            {
                var targetWaypoint = waypoints[waypointIndex].transform;
                var targetPosition = targetWaypoint.position;
                var movemeentThisFrame = enemy.GetMoveSpeed() * Time.deltaTime;
                enemy.transform.position =
                    Vector2.MoveTowards(enemy.transform.position, targetPosition, movemeentThisFrame);

                if (enemy.transform.position == targetPosition) enemies[enemyObject] = ++waypointIndex;
            }
        }

        public void SetWaveConfig(WaveConfig waveConfig)
        {
            this.waveConfig = waveConfig;
            waypoints = waveConfig.GetWaypoints();
        }

        public void AddEnemy(GameObject enemy)
        {
            enemies.Add(enemy, 0);
            enemy.transform.position = waypoints[0].position;
        }
    }
}