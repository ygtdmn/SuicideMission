using System.Collections;
using SuicideMission.Objects;
using UnityEngine;

namespace SuicideMission.Spawners
{
    public class PlanetSpawner : MonoBehaviour
    {
        [SerializeField] private Sprite[] planets;
        [SerializeField] private GameObject planetPrefab;
        [SerializeField] private float paddingX = 1.5f;
        [SerializeField] private float paddingY = 5f;

        private GameObject currentPlanet;
        private LevelLoader levelLoader;

        private IEnumerator Start()
        {
            levelLoader = FindObjectOfType<LevelLoader>();

            foreach (var planet in planets)
            {
                currentPlanet = Instantiate(planetPrefab);
                currentPlanet.GetComponent<SpriteRenderer>().sprite = planet;
                var planetTransform = currentPlanet.transform;
                planetTransform.position = new Vector3(Random.Range(levelLoader.MinX + paddingX, levelLoader.MaxX - paddingX),
                    levelLoader.MaxY + paddingY,
                    planetTransform.position.z);
                yield return new WaitWhile(() => currentPlanet != null);
            }
        }

        private void Update()
        {
            if (currentPlanet != null && currentPlanet.transform.position.y <= levelLoader.MinY - paddingY)
                Destroy(currentPlanet);
        }
    }
}