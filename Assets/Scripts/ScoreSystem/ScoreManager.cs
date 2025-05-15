using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using MoveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScoreSystem
{
    public class ScoreManager : MonoBehaviour
    {
        public Action OnTargetScoreReached;
        public Action OnGameEnded;
        public TextMeshProUGUI totalScoreText;
        public TextMeshProUGUI linkScoreText;
        private int _targetScore;
        private int _currentScore;
        private int _baseScore = 10;
        private Vector3 _linkScoreTextOriginalPosition;
        private Sequence _currentMoveSequence;
        private Tween _currentScoreTween;
        private bool _endGameOnScore;
        private IMoveService _moveManager;
        private bool _isAnimationPlaying;

        public void Initialize(IMoveService moveManager, GameConfig gameConfig)
        {
            _moveManager = moveManager;
            _baseScore = gameConfig.GetBasePointPerChip();
            _targetScore = gameConfig.GetTargetScore();
            _linkScoreTextOriginalPosition = linkScoreText.transform.position;
            _endGameOnScore = gameConfig.GetAutoEndOnTarget();
            _moveManager.OnMoveRunOut += OnMoveRunsOut;
        }

        private void OnMoveRunsOut()
        {
            StartCoroutine(WaitForAnimationThenEndGame());
        }

        private IEnumerator WaitForAnimationThenEndGame()
        {
            
            yield return new WaitUntil(() => !_isAnimationPlaying);

            // Then invoke the event
            OnGameEnded?.Invoke();
        }

        private void Awake()
        {
            totalScoreText.text = "Score: " + _currentScore;
        }

        public void AddScore(int linkSize)
        {
            int points = CalculateScore(linkSize);

            linkScoreText.text = "+ " + points;
            _moveManager.ConsumeMove();
            PlayAnimation(points);
        }

        private void PlayAnimation(int points)
        {
            //For calling multiple times the animation before it ends
            _isAnimationPlaying = true;
            var animationScore = _currentScore;
            _currentScore += points;
            
            if (_currentScore >= _targetScore && _endGameOnScore)
            {
                OnTargetScoreReached?.Invoke();
            }

            if (_currentScoreTween != null && _currentScoreTween.IsActive())
            {
                // For keeping logic safe 
                _currentScoreTween.Complete();
            }


            if (_currentMoveSequence != null && _currentMoveSequence.IsActive())
            {
                _currentMoveSequence.Kill();
            }


            linkScoreText.transform.position = _linkScoreTextOriginalPosition;
            linkScoreText.transform.localScale = Vector3.zero;
            linkScoreText.gameObject.SetActive(true);
            linkScoreText.text = "0";


            _currentMoveSequence = DOTween.Sequence();

            _currentMoveSequence.Append(linkScoreText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));
            _currentMoveSequence.Append(DOTween.To(() => 0, x => linkScoreText.text = x.ToString(), points, 0.5f)
                .SetEase(Ease.Linear));
            _currentMoveSequence.Append(linkScoreText.transform.DOMove(totalScoreText.transform.position, 0.5f)
                .SetEase(Ease.InOutSine));
            _currentMoveSequence.Join(linkScoreText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine));


            _currentMoveSequence.OnComplete(() =>
            {
                int newScore = animationScore + points;

                _currentScoreTween = DOTween.To(() => _currentScore, x =>
                    {
                        animationScore = x;
                        totalScoreText.text = "Score: " + Mathf.FloorToInt(_currentScore);
                    }, newScore, 1f).SetEase(Ease.InOutSine)
                    .OnComplete(() => totalScoreText.text = "Score: " + _currentScore);

                linkScoreText.gameObject.SetActive(false);

             
            });
            _isAnimationPlaying = false;
        }


        private int CalculateScore(int linkSize)
        {
            return _baseScore * linkSize;
        }

        public void ResetScore()
        {
            _currentScore = 0;
        }

        public bool isWon() => _currentScore >= _targetScore;
        public int GetCurrentScore() => _currentScore;
    }
}