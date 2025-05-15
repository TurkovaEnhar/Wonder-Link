using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ScoreSystem
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalScoreText;
        [SerializeField] private TextMeshProUGUI targetScoreText;
        [SerializeField] private TextMeshProUGUI linkScoreText;

        private Vector3 _originalLinkPos;
        private Sequence _scoreSequence;

        public void Initialize(int targetScore)
        {
            _originalLinkPos = linkScoreText.transform.position;
            totalScoreText.text = "Score: 0";
            targetScoreText.text = "Target Score : " + targetScore ;
            
        }

        public void AnimateScore(int scoreAdded, int newTotalScore)
        {
            linkScoreText.text = $"+{scoreAdded}";
            linkScoreText.transform.position = _originalLinkPos;
            linkScoreText.transform.localScale = Vector3.zero;
            linkScoreText.gameObject.SetActive(true);

            if (_scoreSequence != null && _scoreSequence.IsActive())
                _scoreSequence.Kill();

            _scoreSequence = DOTween.Sequence();

            _scoreSequence.Append(linkScoreText.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack));
            _scoreSequence.Append(DOTween.To(() => 0, x => linkScoreText.text = $"+{x}", scoreAdded, 0.5f).SetEase(Ease.Linear));
            _scoreSequence.Append(linkScoreText.transform.DOMove(totalScoreText.transform.position, 0.5f));
            _scoreSequence.Join(linkScoreText.transform.DOScale(Vector3.zero, 0.5f));

            _scoreSequence.OnComplete(() =>
            {
                totalScoreText.text = $"Score: {newTotalScore}";
                linkScoreText.gameObject.SetActive(false);
            });
        }

        // public void SetScore(int score)
        // {
        //     totalScoreText.text = $"Score: {score}";
        // }
    }
}