using System.Collections;
using BonusSystems.LevelSystem;
using UnityEngine;
using Game;
using MoveSystem;

namespace ScoreSystem
{
    public class ScoreController : MonoBehaviour
    {
        public System.Action OnTargetScoreReached;
        public System.Action OnGameEnded;
        [SerializeField] private ScoreView scoreView;
        
        private LevelRequirementService _levelRequirementService;
        private IScoreService _scoreService;
        private IMoveService _moveService;
        

        private bool _isAnimating;
        private int _basePointPerChip;
        public void Initialize( IMoveService moveService,LevelRequirementService levelRequirementService,int targetScore,int basePointPerChip )
        {
            _moveService = moveService;
            _levelRequirementService = levelRequirementService;
            
            _scoreService = new ScoreService(targetScore);
            _basePointPerChip = basePointPerChip;
            
            scoreView.Initialize(targetScore);
            
            _scoreService.OnTargetReached += CheckEndGame;
            _moveService.OnMoveRunOut += () => StartCoroutine(WaitForAnimationThenEndGame());
        }

        private void CheckEndGame()
        {
            if (_levelRequirementService.IsAllRequirementsMet())
            {
                OnTargetScoreReached?.Invoke();
            }
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