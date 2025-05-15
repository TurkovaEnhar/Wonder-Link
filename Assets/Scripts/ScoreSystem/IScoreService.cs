using System;

namespace ScoreSystem
{
    public interface IScoreService
    {
        int CurrentScore { get; }
        int TargetScore { get; }
        bool HasWon { get; }

        event Action<int> OnScoreChanged;
        event Action OnTargetReached;

        void AddScore(int amount);
        void Reset();
    }
}