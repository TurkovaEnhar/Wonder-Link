using System.Collections;
using System.Collections.Generic;
using Board;
using Game;
using MoveSystem;
using UnityEngine;

namespace Link
{
    public class LinkManager : MonoBehaviour
    {
        public BoardManager boardManager; 
        [Header("Game Systems")]
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private BoardAnalyzer boardAnalyzer;
        [SerializeField] private MoveManager moveManager;
        private List<Chip> currentLink = new();
        private ChipColor _currentColor;
        [SerializeField] private GameConfig gameConfig;

  

        public void BeginLink(Chip startChip)
        {
            if (!moveManager.HasMoves()) return;
            
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
        public void CancelLink()
        {
            foreach (var chip in currentLink)
            {
                chip.ResetColor();
            }
            
        }    
        public void ConfirmLink()
        {
            foreach (var chip in currentLink)
            {
                chip.DestroyChip();
            }
            scoreManager.AddScore(currentLink.Count); 
            moveManager.ConsumeMove();   
            StartCoroutine(Fill());
            
        }
        private IEnumerator Fill()
        {
            yield return new WaitForEndOfFrame(); // Destroylar tamamlansın
            boardManager.FillBoard();
            yield return new WaitForSeconds(0.3f); 

            if (!boardAnalyzer.HasPossibleMoves())
            {
                boardManager.ShuffleBoard();
            }
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
        private LinkMode CurrentLinkMode => gameConfig.linkMode;
    }
} 