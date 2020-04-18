using SuicideMission.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SuicideMission.Behavior
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] private GameObject checkpointCanvas;

        [SerializeField] private GameObject lockedCheckpoint;
        [SerializeField] private GameObject completedCheckpoint;
        [SerializeField] private GameObject currentCheckpoint;

        [SerializeField] private GameObject[] checkpoints;

        void Start()
        {
            var lastLevelBeaten = PlayerPrefs.GetInt("LastLevelBeaten");
            int i = 1;
            foreach (var checkpoint in checkpoints)
            {
                var levelIndex = i;
                
                if (lastLevelBeaten > i || lastLevelBeaten == i)
                {
                    var chObject = Instantiate(completedCheckpoint, checkpoint.transform.position, Quaternion.identity);
                    chObject.transform.SetParent(checkpointCanvas.transform, false);
                    chObject.GetComponent<Button>().onClick.AddListener(() => OnClick(levelIndex));
                    chObject.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                }
                else if (lastLevelBeaten + 1 == i)
                {
                    var chObject = Instantiate(currentCheckpoint, checkpoint.transform.position, Quaternion.identity);
                    chObject.transform.SetParent(checkpointCanvas.transform, false);
                    chObject.GetComponent<Button>().onClick.AddListener(() => OnClick(levelIndex));
                    chObject.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                }
                else
                {
                    var chObject = Instantiate(lockedCheckpoint, checkpoint.transform.position, Quaternion.identity);
                    chObject.transform.SetParent(checkpointCanvas.transform, false);
                    chObject.GetComponent<Button>().onClick.AddListener(() => OnClick(levelIndex));
                    chObject.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                }

                i++;
            }
        }

        private void OnClick(int i)
        {
            FindObjectOfType<LevelLoader>().LoadLevel("Level " + i);
        }
    }
}