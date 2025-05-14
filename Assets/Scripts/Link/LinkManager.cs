using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using Board.Chips;
using Game;
using MoveSystem;
using ScoreSystem;
using UnityEngine;

namespace Link
{
    public class LinkManager : MonoBehaviour
    {
        public Action OnLinkSuccess;
        private ScoreManager _scoreManager;
        private GameConfig _gameConfig;

        private List<Chip> currentLink = new();
        private ChipColor _currentColor;


        public void Initialize(ScoreManager scoreManager, GameConfig gameConfig)
        {
            _scoreManager = scoreManager;
            _gameConfig = gameConfig;
            
        }
        
        public void BeginLink(Chip startChip)
        {
          
            
            currentLink.Clear();
            _currentColor = startChip.Color;
            AddChipToLink(startChip);
        }

        public void TryAddToLink(Chip candidate)
        {
            if (candidate.Color != _currentColor) return;
            if (currentLink.Contains(candidate)) return;

            Chip last = currentLink[^1];
            if (AreNeighbors(last, candidate))
            {
                AddChipToLink(candidate);
            }
        }

        public void CompleteLink()
        {
            if (currentLink.Count >= 3)
            {
                ConfirmLink();
            }
            else
            {
                CancelLink();
            }

            currentLink.Clear();
        }

        private void CancelLink()
        {
            foreach (var chip in currentLink)
            {
                chip.ResetColor();
            }
            
        }

        private void ConfirmLink()
        {
            foreach (var chip in currentLink)
            {
                chip.DestroyChip();
            }
            _scoreManager.AddScore(currentLink.Count); 
            OnLinkSuccess?.Invoke();
          
            
        }
        

        private void AddChipToLink(Chip chip)
        {
            currentLink.Add(chip);
            chip.Select();
        }

        private bool AreNeighbors(Chip a, Chip b)
        {
            Vector2Int posA = a.ParentTile.GridPosition;
            Vector2Int posB = b.ParentTile.GridPosition;
            Vector2Int diff = posB - posA;

            if (CurrentLinkMode == LinkMode.FourWay)
            {
                return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
            }
            else 
            {
                return Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1 && (diff != Vector2Int.zero);
            }
        }
        private LinkMode CurrentLinkMode => _gameConfig.linkMode;
        public  List<Chip> GetCurrentLink() => currentLink;
    }
} 