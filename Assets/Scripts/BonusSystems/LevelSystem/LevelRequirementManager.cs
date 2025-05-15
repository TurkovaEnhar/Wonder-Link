using System;
using System.Collections.Generic;
using Board.Chips;
using Stats;
using UnityEngine;

namespace BonusSystems.LevelSystem
{
    public class LevelRequirementManager : MonoBehaviour
    {
        private LevelDataSO _levelData;

        private Dictionary<ChipColor, int> _requiredColors = new();
        private int _requiredLongLinks;
        private int _longLinkTarget = 0;

        public event Action OnAllRequirementsMet;

        public void Initialize(LevelDataSO levelData)
        {
            _levelData = levelData;
          

            _requiredColors.Clear();
            if (_levelData.HasColorTargets)
            {
                foreach (var colorReq in _levelData.colorTargets)
                    _requiredColors[colorReq.color] = colorReq.count;
            }

            _longLinkTarget = levelData.linkTarget.linkSize;
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
                }
            }

            if (_levelData.HasMinLinkTarget && linkLength >= _longLinkTarget)
            {
                _requiredLongLinks++;
                updated = true;
            }

            if (updated && IsAllRequirementsMet())
            {
                Debug.Log("Tüm özel görevler tamamlandı!");
                OnAllRequirementsMet?.Invoke();
            }
        }

        public bool IsAllRequirementsMet()
        {
            bool colorsDone = true;

            foreach (var kvp in _requiredColors)
            {
                if (kvp.Value > 0)
                    colorsDone = false;
            }

            bool longLinksDone = !_levelData.HasMinLinkTarget || _requiredLongLinks >= 1;

            return colorsDone && longLinksDone;
        }

        public Dictionary<ChipColor, int> GetRemainingColorGoals() => new(_requiredColors);
        public int GetLongLinksDone() => _requiredLongLinks;
    }
}
