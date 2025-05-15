using System;
using System.Collections.Generic;
using Board.Chips;
using Stats;
using UnityEngine;

namespace BonusSystems.LevelSystem
{
    public class LevelRequirementService 
    {
        public event Action<ChipColor, int> OnColorTargetChanged;
        public event Action<int> OnLongLinkProgressed;
        public event Action OnAllRequirementsMet;
        
        private LevelDataSO _levelData;
        private Dictionary<ChipColor, int> _requiredColors = new();
        private int _requiredLinkAmount;
        private int _requiredlinkLength;


        public LevelRequirementService(LevelDataSO levelData)
        {
            _levelData = levelData;
            _requiredColors.Clear();
            if (_levelData.HasColorTargets)
            {
                foreach (var colorReq in _levelData.colorTargets)
                    _requiredColors[colorReq.color] = colorReq.count;
            }

            _requiredlinkLength = levelData.linkTarget.linkSize;
            _requiredLinkAmount = _levelData.linkTarget.amount;
        }

        public void EvaluateLink(int linkLength, ChipColor color)
        {
            bool updated = false;

            if (_levelData.HasColorTargets)
            {
                if (_requiredColors.ContainsKey(color) && _requiredColors[color] > 0)
                {
                    _requiredColors[color] -= linkLength;
                    _requiredColors[color] = Mathf.Max(0, _requiredColors[color]); // Asla negatif olmasın
                    updated = true;
                    OnColorTargetChanged?.Invoke(color, _requiredColors[color]);
                }
            }

            if (_levelData.HasMinLinkTarget && linkLength >= _requiredlinkLength)
            {
                _requiredLinkAmount--;
                OnLongLinkProgressed?.Invoke(_requiredLinkAmount);
                updated = true;
            }

            if (!updated || !IsAllRequirementsMet()) return;
            Debug.Log("Tüm özel görevler tamamlandı!");
            OnAllRequirementsMet?.Invoke();
        }

        public bool IsAllRequirementsMet()
        {
            bool colorsDone = true;

            foreach (var kvp in _requiredColors)
            {
                if (kvp.Value > 0)
                    colorsDone = false;
            }

            bool longLinksDone = !_levelData.HasMinLinkTarget || _requiredLinkAmount <=0;

            return colorsDone && longLinksDone;
        }

        public Dictionary<ChipColor, int> GetRemainingColorGoals() => new(_requiredColors);
        public int GetRemainingLongLinks() => _requiredLinkAmount;
        public int GetRequiredLinkLength() => _requiredlinkLength;
        public bool HasColorTargets() => _levelData.HasColorTargets;
        public bool HasMinLinkTarget() => _levelData.HasMinLinkTarget;
    }
}
