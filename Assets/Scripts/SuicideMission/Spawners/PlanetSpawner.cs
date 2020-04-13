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
        private Level level;

        private IEnumerator Start()
        {
            level = FindObjectOfType<Level>();

            foreach (var planet in planets)
            {
                currentPlanet = Instantiate(planetPrefab);
                planetPrefab.GetComponent<SpriteRenderer>().sprite = planet;
                var planetTransform = planetPrefab.transform;
                planetTransform.position = new Vector3(Random.Range(level.MinX + paddingX, level.MaxX - paddingX),
                    level.MaxY + paddingY,
                    planetTransform.position.z);
                yield return new WaitWhile(() => currentPlanet != null);
            }
        }

        private void Update()
        {
            if (currentPlanet != null && currentPlanet.transform.position.y <= level.MinY - paddingY)
                Destroy(currentPlanet);
        }
    }
}