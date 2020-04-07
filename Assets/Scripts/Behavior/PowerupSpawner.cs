using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;

namespace Behavior
{
    public class PowerupSpawner : MonoBehaviour
    {
        [SerializeField] private Powerup[] powerups;
        [SerializeField] private float minSpawningDelay;
        [SerializeField] private float maxSpawningDelay;

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
            Camera gameCamera = Camera.main;

            xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y ;
        }

        private void InitializeChanceTable()
        {
            powerupList = new List<Items<Powerup>>();
            foreach (var powerup in powerups)
            {
                powerupList.Add(new Items<Powerup> {Probability = (powerup.GetSpawnChance() / 100.0), Item = powerup});
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
                float x = Random.Range(xMin, xMax);
                Vector2 position = new Vector2(x, yMax);
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

    class Items<T>
    {
        public double Probability { get; set; }
        public T Item { get; set; }
    }
}