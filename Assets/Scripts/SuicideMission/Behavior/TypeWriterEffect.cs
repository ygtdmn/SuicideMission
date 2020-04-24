using System.Collections;
using TMPro;
using UnityEngine;

namespace SuicideMission.Behavior
{
    public class TypeWriterEffect : MonoBehaviour
    {
        [SerializeField] private float delay = 0.05f;
        [SerializeField] private float delayBetweenBreaks = 0.1f;
        private string initialText;
        private string currentText = "";

        private void Start()
        {
            initialText = GetComponent<TextMeshProUGUI>().text;
            StartCoroutine(Perform());
        }

        private IEnumerator Perform()
        {
            for (int i = 0; i <= initialText.Length; i++)
            {
                currentText = initialText.Substring(0, i);
                GetComponent<TextMeshProUGUI>().text = currentText;
                if (currentText.EndsWith(".") || currentText.EndsWith("!") || currentText.EndsWith(";"))
                {
                    yield return new WaitForSeconds(delayBetweenBreaks);
                }
                else
                {
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}