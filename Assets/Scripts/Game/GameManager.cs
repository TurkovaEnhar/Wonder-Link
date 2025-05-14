using Board;
using Link;
using MoveSystem;
using ScoreSystem;
using UnityEngine;

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


        private void Awake()
        {
            Application.targetFrameRate = 60;
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            moveManager.Initialize(gameConfig);
            scoreManager.Initialize(moveManager,gameConfig);
            linkManager.Initialize(scoreManager,gameConfig);
            boardAnalyzer.Initialize(boardManager,gameConfig);
            boardManager.Initialize(linkManager,boardAnalyzer,gameConfig);
            endGameManager.Initialize(scoreManager);
            inputHandler.Initialize(linkManager);
            
            scoreManager.OnTargetScoreReached += EndGame;
            scoreManager.OnGameEnded += EndGame;
        }

        private void EndGame()
        {
            endGameManager.Open();
        }
    }
}