using System;
using Board;
using Link;
using MoveSystem;
using ScoreSystem;
using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private GameConfig gameConfig;
        [Header("Managers")]
        [SerializeField] private MoveManager moveManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private LinkManager linkManager;
        [SerializeField] private BoardAnalyzer boardAnalyzer;
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private GameEndManager endGameManager;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private StatUIManager statUIManager;
        [SerializeField] private Button pauseButton;
        private StatSystem _statSystem;


        private void Awake()
        {
            Application.targetFrameRate = 60;
            pauseButton.onClick.AddListener(ShowPauseMenu);
            InitializeManagers();
        }

  

        private void InitializeManagers()
        {
            _statSystem = new StatSystem();
            _statSystem = SaveSystem.LoadStats();
            moveManager.Initialize(gameConfig);
            scoreManager.Initialize(moveManager,gameConfig);
            linkManager.Initialize(scoreManager,_statSystem,gameConfig);
            boardAnalyzer.Initialize(boardManager,gameConfig);
            boardManager.Initialize(linkManager,boardAnalyzer,gameConfig);
            endGameManager.Initialize(scoreManager);
            inputHandler.Initialize(linkManager);
            
            scoreManager.OnTargetScoreReached += EndGame;
            scoreManager.OnGameEnded += EndGame;
        }
        
        private void ShowPauseMenu()
        {
            statUIManager.ShowStats(_statSystem);
        }

        private void OnDestroy()
        {
            SaveSystem.SaveStats(_statSystem);
        }

        private void EndGame()
        {
            SaveSystem.SaveStats(_statSystem);
            endGameManager.Open();
        }
    }
}