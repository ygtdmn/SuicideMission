using System.Collections;
using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Spawners
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject coinObject;
        [SerializeField] private float minSpawningDelay;
        [SerializeField] private float maxSpawningDelay;
        [SerializeField] private float padding = 0.75f;

        private LevelLoader levelLoader;

        private IEnumerator Start()
        {
            levelLoader = FindObjectOfType<LevelLoader>();
            yield return new WaitForSeconds(Random.Range(minSpawningDelay, maxSpawningDelay));
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                var x = Random.Range(levelLoader.MinX + padding, levelLoader.MaxX - padding);
                var position = new Vector2(x, levelLoader.MaxY - padding);
                var coinGameObject = Instantiate(coinObject, position, Quaternion.identity);
                Coin coin = coinGameObject.GetComponent<Coin>();
                coinGameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -coin.GetDropSpeed());
                yield return new WaitForSeconds(Random.Range(minSpawningDelay, maxSpawningDelay));
            }
        }
    }
}