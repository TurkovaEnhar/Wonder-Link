using System;
using Board;
using Link;
using MoveSystem;
using ScoreSystem;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        
        [SerializeField] private GameConfig gameConfig;
        
        
        [FormerlySerializedAs("moveManager")]
        [Header("Managers")]
        
        [SerializeField] private MoveController moveController;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private GameEndManager endGameManager;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private StatUIManager statUIManager;
        [SerializeField] private Button pauseButton;
        
        
        private StatSystem _statSystem;
        private LinkService _linkService;
        private BoardScanService _boardScanService;


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
            _boardScanService = new BoardScanService();
            
            _linkService = new LinkService(scoreManager, _statSystem,gameConfig.linkMode);
            
            moveController.Initialize(gameConfig);
            var moveService = moveController.GetMoveService();
            scoreManager.Initialize(moveService,gameConfig);
            boardManager.Initialize(_linkService,_boardScanService,gameConfig);
            endGameManager.Initialize(scoreManager);
            inputHandler.Initialize(_linkService);
            
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