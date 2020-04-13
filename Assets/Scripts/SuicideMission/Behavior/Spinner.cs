using UnityEngine;

namespace SuicideMission.Behavior
{
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 360f;

        private void Update()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}