using System.Collections.Generic;
using System.Linq;
using Board.Chips;
using UnityEngine;

namespace BonusSystems.LevelSystem
{
    public class LevelRequirementView : MonoBehaviour
    {
        private LevelRequirementService _requirementService;
        [Header("Color Target UI")]
        [SerializeField] private GameObject colorParent;
        [SerializeField] private List<ChipEntry> colorTargetDisplay;
        [Header("Long Link UI")]
        [SerializeField] private RequirementDisplayEntry linkGoalDisplay;

        public void Initialize(LevelRequirementService requirementService)
        {
            _requirementService = requirementService;
            SetupColorGoals();
            SetupLinkGoal();
            if (_requirementService == null) return;
            _requirementService.OnColorTargetChanged += HandleColorTargetChanged;
            _requirementService.OnLongLinkProgressed += HandleLongLinkProgress;
        }
        

        private void OnDestroy()
        {
            _requirementService.OnColorTargetChanged -= HandleColorTargetChanged;
            _requirementService.OnLongLinkProgressed -= HandleLongLinkProgress;
        }

 

        private void SetupColorGoals()
        {
            var activeGoals = _requirementService.GetRemainingColorGoals();

            bool hasAny = false;

            foreach (var entry in colorTargetDisplay)
            {
                if (activeGoals.TryGetValue(entry.color, out int count) && count > 0)
                {
                    entry.countText.text = $"x{count}";
                    entry.countText.gameObject.SetActive(true);
                    hasAny = true;
                }
                else
                {
                    entry.countText.gameObject.SetActive(false);
                }
            }

            colorParent.SetActive(hasAny);
        }
        
        private void SetupLinkGoal()
        {
            if (_requirementService.HasMinLinkTarget())
            {

                string text = $"{_requirementService.GetRequiredLinkLength()}+ link x{_requirementService.GetRemainingLongLinks()} ";
                linkGoalDisplay.Set(text, true);
            }
            else
            {
                linkGoalDisplay.SetVisible(false);
            }
        }
        
        private void HandleColorTargetChanged(ChipColor color, int amount)
        {
            var a = FindTarget(color);
            
            a.countText.text = $"x{amount}";
        }
  

        private void HandleLongLinkProgress(int doneCount)
        {
            int required = _requirementService.GetRemainingLongLinks();
            bool complete = required <= 0;
            string text = "";
            text = complete ? "Done" :  $"{_requirementService.GetRequiredLinkLength()}+ link x{_requirementService.GetRemainingLongLinks()} ";

            linkGoalDisplay.SetText(text);
        }
        
        private ChipEntry FindTarget(ChipColor color) => colorTargetDisplay.FirstOrDefault(c => c.color == color);
        
        
        
    }
}