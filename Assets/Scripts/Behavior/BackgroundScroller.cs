using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;

    private Material myMaterial;
    private Vector2 offset;

    void Start()
    {
        myMaterial = gameObject.GetComponent<Renderer>().material;
        offset = new Vector2(0, scrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}