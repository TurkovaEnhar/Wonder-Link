using UnityEngine;

namespace Game
{
    [ExecuteAlways]
    public class CameraScaler : MonoBehaviour
    {
        [Header("Reference Settings")]
        public float referenceWidth = 1920f;
        public float referenceHeight = 1080f;
        public float referenceOrthoSize = 2f; 

        private void Start()
        {
            AdjustCameraToMatchReferenceWidth();
        }

        private void AdjustCameraToMatchReferenceWidth()
        {
            var cam = Camera.main;
            if (cam == null || !cam.orthographic) return;

            float referenceAspect = referenceWidth / referenceHeight;
            float screenAspect = (float)Screen.width / Screen.height;

            // Burada width sabit kalacak şekilde yüksekliği değiştireceğiz
            float orthoSize = referenceOrthoSize * (referenceAspect / screenAspect);

            cam.orthographicSize = orthoSize;
        }
    }
}