using Game;
using TMPro;
using UnityEngine;

namespace ScoreSystem
{
    public class ScoreManager : MonoBehaviour
    {
        private int _targetScore;
        public TextMeshProUGUI text;
        private int _score;
        private int _baseScore = 10;


        public void Initialize(GameConfig gameConfig)
        {
            _baseScore = gameConfig.GetBasePointPerChip();
            _targetScore = gameConfig.GetTargetScore();
        }

        private void Awake()
        {
            text.text = "0";
        }

        public void AddScore(int linkSize)
        {
            int points = CalculateScore(linkSize);
            _score += points;
            text.text = _score.ToString();
        }

        private int CalculateScore(int linkSize)
        {
            return _baseScore * linkSize;
        }

        public void ResetScore()
        {
            _score = 0;
        }

        public bool isWon() => _score >= _targetScore;
    }
}
