using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}