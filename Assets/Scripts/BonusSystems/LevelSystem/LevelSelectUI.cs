using System.Collections.Generic;
using System.Linq;
using Board.Chips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BonusSystems.LevelSystem
{
    [System.Serializable]
    public class RequirementDisplayEntry
    {
        [SerializeField] private GameObject rootObject;
        [SerializeField] private TextMeshProUGUI textField;

        public void SetVisible(bool visible)
        {
            if (rootObject != null)
                rootObject.SetActive(visible);
        }

        public void SetText(string text)
        {
            if (textField != null)
                textField.text = text;
        }

        public void Set(string text, bool visible)
        {
            SetText(text);
            SetVisible(visible);
        }
    }

    [System.Serializable]
    public class ChipEntry
    {
        public ChipColor color;
        public TextMeshProUGUI countText;
    }
  
    public class LevelSelectUI : MonoBehaviour
    {
        [Header("Level References")]
        [SerializeField] private LevelDatabaseSO levelDatabase;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        
        [Header("Requirement Displays")]
        [SerializeField] private RequirementDisplayEntry moveDisplay;
        [SerializeField] private RequirementDisplayEntry scoreDisplay;
        [SerializeField] private RequirementDisplayEntry linkGoalDisplay;
        [SerializeField] private GameObject colorParent;
        [SerializeField] private List<ChipEntry> colorTargetDisplay;
        
        [Header("UI Controls")]
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button levelButton;
        [SerializeField] private GameObject levelWindow;

        private int _currentLevelIndex;

        private void Start()
        {
            prevButton.onClick.AddListener(GoToPreviousLevel);
            nextButton.onClick.AddListener(GoToNextLevel);
            playButton.onClick.AddListener(PlayCurrentLevel);
            levelButton.onClick.AddListener(ShowLevelWindow);

            _currentLevelIndex = 0;
            UpdateLevelUI();
        }

        private void ShowLevelWindow()
        {
            UpdateLevelUI();
            levelWindow.gameObject.SetActive(true);
        }

        private void UpdateLevelUI()
        {
            if (_currentLevelIndex < 0 || _currentLevelIndex >= levelDatabase.GetLevelCount()) return;

            LevelDataSO level = levelDatabase.GetLevel(_currentLevelIndex);
            levelNumberText.text = $"Level {_currentLevelIndex + 1}";

            moveDisplay.Set($"Moves: {level.moveCount}", true);
            scoreDisplay.Set($"Target : {level.targetScore} pts", true);

            if (level.HasColorTargets)
            {
                colorParent.SetActive(true);
                foreach (var colors in level.colorTargets)
                {
                    var a = FindTarget(colors.color);
                    a.countText.text = colors.count.ToString();
                    
                }
            }
            else
            {
                colorParent.SetActive(false);
            }

            if (level.HasMinLinkTarget)
            {
                linkGoalDisplay.Set($"{level.linkTarget.linkSize}+ link  x{level.linkTarget.amount}", true);
            }
            else
            {
                linkGoalDisplay.SetVisible(false);
            }

            prevButton.interactable = _currentLevelIndex > 0;
            nextButton.interactable = _currentLevelIndex < levelDatabase.GetLevelCount() - 1;
        }

        private void GoToPreviousLevel()
        {
            _currentLevelIndex--;
            UpdateLevelUI();
        }

        private void GoToNextLevel()
        {
            _currentLevelIndex++;
            UpdateLevelUI();
        }

        private void PlayCurrentLevel()
        {
            LevelManager.Instance.LoadLevel(_currentLevelIndex);
        }

        private ChipEntry FindTarget(ChipColor color) => colorTargetDisplay.FirstOrDefault(c => c.color == color);
            
    }
}