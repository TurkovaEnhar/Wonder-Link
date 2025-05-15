using System;
using MoveSystem;
using ScoreSystem;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameEndManager : MonoBehaviour
    {
        [SerializeField] private GameObject endGameScreen;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private Button restartButton;

        private ScoreController _scoreManager;
        private bool _isGameEnded;

        public void Initialize(ScoreController scoreManager)
        {
            _scoreManager = scoreManager;
            restartButton.onClick.AddListener(RestartGame);
            
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(RestartGame);
        }

        public void Open()
        {
            statusText.text = _scoreManager.HasWon() ? "You won!" : "You lost!";
            finalScoreText.text = "Score: " + _scoreManager.GetCurrentScore();
            endGameScreen.gameObject.SetActive(true);
        }
        

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}