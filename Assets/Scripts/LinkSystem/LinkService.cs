using System;
using System.Collections.Generic;
using Board.Chips;
using Game;
using ScoreSystem;
using Stats;

namespace Link
{
    public class LinkService
    {
        public event Action OnLinkSuccess;
        public event Action<int,ChipColor> OnLinkEvaluated;
        private readonly ScoreController _scoreManager;
        private readonly StatSystem _statSystem;
        private readonly INeighborChecker _neighborChecker;
        private readonly LinkMode _linkMode;

        private List<Chip> _currentLink = new();
        private ChipColor _currentColor;


        public LinkService(ScoreController scoreManager, StatSystem statSystem, LinkMode linkMode)
        {
            _scoreManager = scoreManager;
            _statSystem = statSystem;
            _neighborChecker = new NeighborChecker();
            _linkMode = linkMode;
        }

        public void BeginLink(Chip startChip)
        {
            _currentLink.Clear();
            _currentColor = startChip.Color;
            AddChipToLink(startChip);
        }

        public void TryAddToLink(Chip candidate)
        {
            if (_currentLink.Count == 0) return;

            Chip last = _currentLink[^1];

            // Geri adım atma (undo)
            if (_currentLink.Count > 1 && candidate == _currentLink[^2])
            {
                Chip removed = _currentLink[^1];
                removed.ResetColor();
                _currentLink.RemoveAt(_currentLink.Count - 1);
                return;
            }

            if (candidate.Color != _currentColor || _currentLink.Contains(candidate)) return;

            if (_neighborChecker.AreNeighbors(last, candidate, _linkMode))
            {
                AddChipToLink(candidate);
            }
        }

        public void CompleteLink()
        {
            if (_currentLink.Count >= 3)
            {
                foreach (var chip in _currentLink)
                    chip.DestroyChip();

                _statSystem.RecordLink(_currentLink.Count, _currentColor);
                _scoreManager.AddScore(_currentLink.Count);
                OnLinkEvaluated?.Invoke(_currentLink.Count, _currentColor);
                OnLinkSuccess?.Invoke();
            }
            else
            {
                CancelLink();
            }

            _currentLink.Clear();
        }

        private void AddChipToLink(Chip chip)
        {
            chip.Select();
            _currentLink.Add(chip);
        }

        private void CancelLink()
        {
            foreach (var chip in _currentLink)
                chip.ResetColor();
        }

        public List<Chip> GetCurrentLink() => _currentLink;
    }
}
