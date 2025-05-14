using System;
using Game;
using TMPro;
using UnityEngine;

namespace MoveSystem
{
    public class MoveManager : MonoBehaviour
    {
        public Action OnMoveRunOut;
        [SerializeField] private TextMeshProUGUI text;
        private int _remainingMoves;

        public void Initialize(GameConfig gameConfig)
        {
            _remainingMoves = gameConfig.GetMoveCount();
        }
        private void Awake()
        {
            text.text = "Move: " + _remainingMoves;
        }

        public void ConsumeMove()
        {
            _remainingMoves--;
            text.text = "Move: " + _remainingMoves;
            if (_remainingMoves > 0) return;
            
            OnMoveRunOut?.Invoke();

        }


        public void ResetMoves(int startMoves = 20)
        {
            _remainingMoves = startMoves;
        }
        public bool HasMoves() => _remainingMoves > 0;
        
    }
}