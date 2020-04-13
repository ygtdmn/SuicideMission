using UnityEngine;

namespace SuicideMission.Behavior
{
    public class CameraResolution : MonoBehaviour
    {
        [SerializeField] private float horizontalAspect = 9;
        [SerializeField] private float verticalAspect = 16;

        private int screenSizeX;
        private int screenSizeY;

        private void RescaleCamera()
        {
            if (Screen.width == screenSizeX && Screen.height == screenSizeY) return;

            var targetaspect = horizontalAspect / verticalAspect;
            var windowaspect = Screen.width / (float) Screen.height;
            var scaleheight = windowaspect / targetaspect;
            var camera = GetComponent<Camera>();

            if (scaleheight < 1.0f)
            {
                var rect = camera.rect;

                rect.width = 1.0f;
                rect.height = scaleheight;
                rect.x = 0;
                rect.y = (1.0f - scaleheight) / 2.0f;

                camera.rect = rect;
            }
            else // add pillarbox
            {
                var scalewidth = 1.0f / scaleheight;

                var rect = camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }

            screenSizeX = Screen.width;
            screenSizeY = Screen.height;
        }

        private void OnPreCull()
        {
            if (Application.isEditor) return;
            var wp = Camera.main.rect;
            var nr = new Rect(0, 0, 1, 1);

            Camera.main.rect = nr;
            GL.Clear(true, true, Color.black);

            Camera.main.rect = wp;
        }

        private void Start()
        {
            RescaleCamera();
        }

        private void Update()
        {
            RescaleCamera();
        }
    }
}