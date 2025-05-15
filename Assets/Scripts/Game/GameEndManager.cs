using System;
using MoveSystem;
using ScoreSystem;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace Game
{
    public class GameEndManager : MonoBehaviour
    {
        [SerializeField] private GameObject endGameScreen;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        private ScoreController _scoreManager;
        private bool _isGameEnded;

        public void Initialize(ScoreController scoreManager)
        {
            _scoreManager = scoreManager;
            restartButton.onClick.AddListener(RestartGame);
            mainMenuButton.onClick.AddListener(MainMenu);
            
        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(RestartGame);
            mainMenuButton.onClick.RemoveListener(MainMenu);
        }

        public void Open()
        {
            statusText.text = _scoreManager.HasWon() ? "You won!" : "You lost!";
            finalScoreText.text = "Score: " + _scoreManager.GetCurrentScore();
            endGameScreen.gameObject.SetActive(true);
        }
        

        public void RestartGame()
        {
           SceneManagement.Instance.LoadGameSceneAsync();
        } 
        public void MainMenu()
        {
           SceneManagement.Instance.LoadMainMenuAsync();
        }
    }
}