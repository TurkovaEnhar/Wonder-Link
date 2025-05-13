using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public int Score { get; private set; }

        [SerializeField] private int baseScore = 10;

        private void Awake()
        {
            text.text = "0";
        }

        public void AddScore(int linkSize)
        {
            int points = CalculateScore(linkSize);
            Score += points;
            text.text = Score.ToString();
        }

        private int CalculateScore(int linkSize)
        {
            return baseScore * linkSize;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}
