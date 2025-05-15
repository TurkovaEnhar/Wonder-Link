using System;

namespace ScoreSystem
{
    public class ScoreService : IScoreService
    {
        private int _score;
        private readonly int _targetScore;
        private readonly bool _autoEnd;

        public event Action<int> OnScoreChanged;
        public event Action OnTargetReached;

        public int CurrentScore => _score;
        public int TargetScore => _targetScore;
        public bool HasWon => _score >= _targetScore;

        public ScoreService(int targetScore)
        {
            _targetScore = targetScore;
        }

        public void AddScore(int amount)
        {
            _score += amount;
            OnScoreChanged?.Invoke(_score);

            if (_score >= _targetScore)
                OnTargetReached?.Invoke();
        }

        public void Reset()
        {
            _score = 0;
            OnScoreChanged?.Invoke(_score);
        }
        public bool isTargetReached() => _score >= _targetScore;
    }
}