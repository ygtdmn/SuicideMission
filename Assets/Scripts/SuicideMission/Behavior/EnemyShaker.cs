using System.Collections;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class EnemyShaker : MonoBehaviour
    {
        [SerializeField] private float shakeXmin;
        [SerializeField] private float shakeXmax;
        [SerializeField] private float shakeYmin;
        [SerializeField] private float shakeYmax;
        [SerializeField] private float shakeFrequency = 1f;

        private IEnumerator Start()
        {
            while (true)
            {
                var shakeX = Random.Range(shakeXmin, shakeXmax);
                var shakeY = Random.Range(shakeYmin, shakeYmax);
                var shakeVector = new Vector2(shakeX, shakeY);
                GetComponent<Rigidbody2D>().velocity = shakeVector;
                yield return new WaitForSeconds(shakeFrequency);
            }
        }
    }
}