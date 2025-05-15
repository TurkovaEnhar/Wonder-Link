using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Stats;

public class StatUIManager : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI totalLinksText;
    [SerializeField] private TextMeshProUGUI maxLinkText;
    [SerializeField] private TextMeshProUGUI colorStatsText;

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        panel.SetActive(false);
        closeButton.onClick.AddListener(HideStats);
    }

    public void ShowStats(StatSystem statSystem)
    {
        panel.SetActive(true);

        totalLinksText.text = $"Total Links: {statSystem.TotalLinks}";
        maxLinkText.text = $"Longest Link: {statSystem.MaxLinkLength}";

        string colorText = "";
        foreach (var pair in statSystem.ChipDestroyCount)
        {
            colorText += $"{pair.Key}: {pair.Value} destroyed\n";
        }

        colorStatsText.text = colorText;
    }

    public void HideStats()
    {
        panel.SetActive(false);
    }
}