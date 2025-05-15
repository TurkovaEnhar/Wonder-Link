using System.Collections;
using UnityEngine;
using Game;
using MoveSystem;

namespace ScoreSystem
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private ScoreView scoreView;
        private IScoreService _scoreService;
        private IMoveService _moveService;

        private bool _isAnimating;

        public System.Action OnTargetScoreReached;
        public System.Action OnGameEnded;

        private int _basePointPerChip;
        public void Initialize( IMoveService moveService,int targetScore,int basePointPerChip )
        {
            _moveService = moveService;
            _scoreService = new ScoreService(targetScore);
            _basePointPerChip = basePointPerChip;

            scoreView.Initialize(targetScore);
            _scoreService.OnTargetReached += () => OnTargetScoreReached?.Invoke();

            _moveService.OnMoveRunOut += () => StartCoroutine(WaitForAnimationThenEndGame());
        }

        public void AddScore(int linkSize)
        {
            int points = linkSize * _basePointPerChip;
            _scoreService.AddScore(points);
            _moveService.ConsumeMove();

            _isAnimating = true;
            scoreView.AnimateScore(points, _scoreService.CurrentScore);
            _isAnimating = false;
        }

        private IEnumerator WaitForAnimationThenEndGame()
        {
            yield return new WaitUntil(() => !_isAnimating);
            OnGameEnded?.Invoke();
        }

        public int GetCurrentScore() => _scoreService.CurrentScore;
        public bool HasWon() => _scoreService.HasWon;
    }
}