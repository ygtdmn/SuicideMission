using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SuicideMission.Interface;
using UnityEngine;

namespace SuicideMission.Spawners
{
    public class PowerupSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] powerups;
        [SerializeField] private float minSpawningDelay;
        [SerializeField] private float maxSpawningDelay;
        [SerializeField] private float padding = 0.75f;

        private List<Items<Powerup>> powerupList;

        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;

        private IEnumerator Start()
        {
            InitializeChanceTable();
            SetupBoundaries();
            yield return new WaitForSeconds(Random.Range(minSpawningDelay, maxSpawningDelay));
            StartCoroutine(Spawn());
        }

        private void SetupBoundaries()
        {
            var gameCamera = Camera.main;

            xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
            xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

            yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
            yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
        }

        private void InitializeChanceTable()
        {
            powerupList = new List<Items<Powerup>>();
            foreach (var powerupObject in powerups)
            {
                var powerup = powerupObject.GetComponent<Powerup>();
                powerupList.Add(new Items<Powerup> {Probability = powerup.GetSpawnChance() / 100.0, Item = powerup});
            }

            var converted = new List<Items<Powerup>>(powerupList.Count);
            var sum = 0.0;
            foreach (var item in powerupList.Take(powerupList.Count - 1))
            {
                sum += item.Probability;
                converted.Add(new Items<Powerup> {Probability = sum, Item = item.Item});
            }

            converted.Add(new Items<Powerup> {Probability = 1.0, Item = powerupList.Last().Item});

            powerupList = converted;
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                var x = Random.Range(xMin, xMax);
                var position = new Vector2(x, yMax);
                var powerup = Instantiate(GetNextPowerup(), position, Quaternion.identity);
                powerup.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -powerup.GetDropSpeed());
                yield return new WaitForSeconds(Random.Range(minSpawningDelay, maxSpawningDelay));
            }
        }

        private Powerup GetNextPowerup()
        {
            var random = new System.Random();
            var probability = random.NextDouble();
            var selected = powerupList.SkipWhile(i => i.Probability < probability).First();
            return selected.Item;
        }
    }

    internal class Items<T>
    {
        public double Probability { get; set; }
        public T Item { get; set; }
    }
}