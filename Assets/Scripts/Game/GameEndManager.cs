using System;
using BonusSystems.LevelSystem;
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
        [SerializeField] private Button nextLevelRestartButton;
        [SerializeField] private Button mainMenuButton;
        
        private TextMeshProUGUI nextLevelRestartButtonText;
        private ScoreController _scoreManager;
        private LevelRequirementService _levelRequirementService;
        private bool _isGameEnded;
        private bool _isLevelWon;

        public void Initialize(ScoreController scoreManager,LevelRequirementService levelRequirementService)
        {
            _scoreManager = scoreManager;
            _levelRequirementService = levelRequirementService;
            nextLevelRestartButton.onClick.AddListener(NextLevelorRestart);
            mainMenuButton.onClick.AddListener(MainMenu);
            nextLevelRestartButtonText =nextLevelRestartButton.GetComponentInChildren<TextMeshProUGUI>();

        }

        private void NextLevelorRestart()
        {
            if (_isLevelWon && LevelManager.Instance.AreLevelsFinished())
            {
                NextLevel();
            }
            else
            {
                RestartGame();
            }
        }

        private void NextLevel()
        {
            LevelManager.Instance.LoadNextLevel();
                
        }

        private void OnDestroy()
        {
            nextLevelRestartButton.onClick.RemoveListener(NextLevelorRestart);
            mainMenuButton.onClick.RemoveListener(MainMenu);
        }

        public void Open()
        {
            _isLevelWon = _scoreManager.HasWon() && _levelRequirementService.IsAllRequirementsMet();
            if (_isLevelWon)
            {
                nextLevelRestartButtonText.text = LevelManager.Instance.AreLevelsFinished() ? "Next Level" : "Restart\n(No more Levels)";
                statusText.text = "You won!";
            }
            else
            {
                nextLevelRestartButtonText.text = "Restart";
                statusText.text =  "You lost!";
            }
            
            
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