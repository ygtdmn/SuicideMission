using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private Sprite[] planets;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private float padding = 1.5f;

    private bool settedUp;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private GameObject currentPlanet;
    
    private IEnumerator Start()
    {
        SetupMoveBoundaries();
        
        foreach (var planet in planets)
        {
            currentPlanet = Instantiate(planetPrefab);
            planetPrefab.GetComponent<SpriteRenderer>().sprite = planet;
            Transform planetTransform = planetPrefab.transform;
            planetTransform.position = new Vector3(Random.Range(xMin, xMax), yMax + 5, planetTransform.position.z);
            yield return new WaitWhile(() => currentPlanet != null);
        }
    }

    private void Update()
    {
        if (currentPlanet != null && currentPlanet.transform.position.y <= yMin - 5)
        {
            Destroy(currentPlanet);
        }
    }

    private void SetupMoveBoundaries()
    {
        if (settedUp) return;
        
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;

        settedUp = true;
    }
}
