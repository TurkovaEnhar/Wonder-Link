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

        public void Initialize( IMoveService moveService,GameConfig config)
        {
            _moveService = moveService;
            _scoreService = new ScoreService(config.GetTargetScore(), config.GetAutoEndOnTarget());

            // _scoreService.OnScoreChanged += score => scoreView.SetScore(score);
            _scoreService.OnTargetReached += () => OnTargetScoreReached?.Invoke();

            _moveService.OnMoveRunOut += () => StartCoroutine(WaitForAnimationThenEndGame());
        }

        public void AddScore(int linkSize)
        {
            int points = linkSize * 10;
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