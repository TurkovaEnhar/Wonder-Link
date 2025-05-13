using System.Collections.Generic;
using Board;
using UnityEngine;

namespace Link
{
    public class LinkManager : MonoBehaviour
    {
        [SerializeField] private LinkMode linkMode = LinkMode.FourWay;
        private List<Chip> currentLink = new();
        private ChipColor _currentColor;

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

            if (linkMode == LinkMode.FourWay)
            {
                return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
            }
            else 
            {
                return Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1 && (diff != Vector2Int.zero);
            }
        }
    }
} 