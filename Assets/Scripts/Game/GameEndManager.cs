using MoveSystem;
using ScoreSystem;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameEndManager : MonoBehaviour
    {
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;

        private ScoreManager _scoreManager;
        private bool _isGameEnded;

        public void Initialize(ScoreManager scoreManager)
        {
            _scoreManager = scoreManager;
            
        }

        public void Open()
        {
            if (_scoreManager.isWon())
            {
                ShowWinScreen();
            }
            else
            {
                ShowLoseScreen();
            }
        }

        private void ShowWinScreen()
        {
            winScreen.SetActive(true);
        }

        private void ShowLoseScreen()
        {
            loseScreen.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}