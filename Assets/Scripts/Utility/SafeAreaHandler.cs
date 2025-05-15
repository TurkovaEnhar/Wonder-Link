using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaHandler : MonoBehaviour
{
    private RectTransform _panel;
    private Rect _lastSafeArea;
    private Vector2 _lastResolution;
    private ScreenOrientation _lastOrientation;

    void Awake()
    {
        _panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        if (_lastSafeArea != Screen.safeArea ||
            _lastResolution.x != Screen.width ||
            _lastResolution.y != Screen.height ||
            _lastOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        _lastSafeArea = safeArea;
        _lastResolution = new Vector2(Screen.width, Screen.height);
        _lastOrientation = Screen.orientation;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;

        Debug.Log($"SafeArea Applied:\nMin: {anchorMin}, Max: {anchorMax}");
    }
}