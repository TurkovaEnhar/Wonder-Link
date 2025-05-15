using System;
using Board;
using BonusSystems.LevelSystem;
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
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private GameEndManager endGameManager;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private StatUIManager statUIManager;
        [SerializeField] private LevelRequirementManager levelRequirementManager;
        
        private StatSystem _statSystem;
        private LinkService _linkService;
        private BoardScanService _boardScanService;


        private void Awake()
        {
            Application.targetFrameRate = 60;
            InitializeManagers();
        }

  

        private void InitializeManagers()
        {
            LevelDataSO level = LevelManager.Instance.CurrentLevel;
            _statSystem = SaveSystem.LoadStats();
            _boardScanService = new BoardScanService();
            
            _linkService = new LinkService(scoreController, _statSystem,gameConfig.linkMode);
            _linkService.OnLinkEvaluated += levelRequirementManager.EvaluateLink;
            moveController.Initialize(level.moveCount);
            var moveService = moveController.GetMoveService();
            scoreController.Initialize(moveService,level.targetScore,gameConfig);
            boardManager.Initialize(_linkService,_boardScanService,gameConfig);
            endGameManager.Initialize(scoreController);
            inputHandler.Initialize(_linkService);
            levelRequirementManager.Initialize(level);
            scoreController.OnTargetScoreReached += EndGame;
            scoreController.OnGameEnded += EndGame;
            levelRequirementManager.OnAllRequirementsMet += EndGame;
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