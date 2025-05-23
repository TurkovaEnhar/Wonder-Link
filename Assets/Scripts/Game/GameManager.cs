﻿using System;
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
        
        [Header("Managers")]
        [SerializeField] private MoveController moveController;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private GameEndManager endGameManager;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private LevelRequirementView levelRequirementView;
        
        private StatSystem _statSystem;
        private LinkService _linkService;
        private BoardScanService _boardScanService;
        private LevelRequirementService _levelRequirementService;


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
            _levelRequirementService = new LevelRequirementService(level);
            _linkService = new LinkService(scoreController, _statSystem,gameConfig.linkMode);
            
            moveController.Initialize(level.moveCount);
            var moveService = moveController.GetMoveService();
            
            levelRequirementView.Initialize(_levelRequirementService);
            scoreController.Initialize(moveService,_levelRequirementService,level.targetScore,gameConfig.GetBasePointPerChip());
            boardManager.Initialize(_linkService,_boardScanService,gameConfig);
            endGameManager.Initialize(scoreController,_levelRequirementService);
            inputHandler.Initialize(_linkService);
            
            
            if (level.autoEndOnTargetCompleted)
            {
                //Eğer level ile beraber auto end seçilirse hamle sayısının bitmesini beklemez eğer seçilmezse hamle sayısı bitene kadar oyun devam eder.
                scoreController.OnTargetScoreReached += EndGame;
            }
            
            _linkService.OnLinkEvaluated += _levelRequirementService.EvaluateLink;
            scoreController.OnGameEnded += EndGame;
  
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