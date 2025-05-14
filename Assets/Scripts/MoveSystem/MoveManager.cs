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
            text.text = _remainingMoves.ToString();
        }

        public void ConsumeMove()
        {
            if (_remainingMoves <= 0)
            {
                OnMoveRunOut?.Invoke();
                return;
            }
            _remainingMoves--;
            text.text = _remainingMoves.ToString();
        }


        public void ResetMoves(int startMoves = 20)
        {
            _remainingMoves = startMoves;
        }
        public bool HasMoves() => _remainingMoves > 0;
        
    }
}