using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitter : MonoBehaviour
{
   
    private const float ReferenceWidth = 1920f;
    private const float ReferenceHeight = 1080f;
    private const float ReferenceOrthoSize = 2f;


    private void Start()
    {
        FitBackground();
    }

    private void FitBackground()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (cam == null || sr == null || sr.sprite == null) return;
        
        float refWorldHeight = ReferenceOrthoSize * 2f;
        float refWorldWidth = refWorldHeight * (ReferenceWidth / ReferenceHeight);

  
        float actualWorldHeight = cam.orthographicSize * 2f;
        float actualWorldWidth = actualWorldHeight * ((float)Screen.width / Screen.height);

  
        float scaleX = actualWorldWidth / refWorldWidth;
        float scaleY = actualWorldHeight / refWorldHeight;

        float scale = Mathf.Max(scaleX, scaleY); 

        transform.localScale = new Vector3(scale, scale, 1f);
        
    }
}