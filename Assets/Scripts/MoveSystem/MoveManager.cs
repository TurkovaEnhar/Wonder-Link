using System;
using TMPro;
using UnityEngine;

namespace MoveSystem
{
    public class MoveManager : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public int RemainingMoves { get; private set; } = 20;

        private void Awake()
        {
            text.text = RemainingMoves.ToString();
        }

        public void ConsumeMove()
        {
            if (RemainingMoves <= 0) return;
            RemainingMoves--;
            text.text = RemainingMoves.ToString();
        }

        public bool HasMoves() => RemainingMoves > 0;

        public void ResetMoves(int startMoves = 20)
        {
            RemainingMoves = startMoves;
        }
    }
}