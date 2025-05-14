using UnityEngine;

namespace Game
{
    public class CameraScaler : MonoBehaviour
    {
        public float referenceWidth = 1920f;
        
        public float referenceHeight = 1080f;
        
        public float baseOrthoSize ;

        void Start()
        {
            ScaleTheCam();
        }
        
        [ContextMenu("ScaleTheCam")]
        private void ScaleTheCam()
        {
            Camera cam = Camera.main;

            float targetAspect = referenceWidth / referenceHeight;
            float screenAspect = (float)Screen.width / Screen.height;
            
            const float epsilon = 0.01f;

            if (Mathf.Abs(screenAspect - targetAspect) < epsilon)
            {
                cam.orthographicSize = baseOrthoSize;
            }
            else if (screenAspect < targetAspect)
            {
                float scale = targetAspect / screenAspect;
                cam.orthographicSize = baseOrthoSize * scale;
            }
            else
            {
                cam.orthographicSize = baseOrthoSize;
            }
        }
    }
}