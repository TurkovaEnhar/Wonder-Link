using System;

namespace MoveSystem
{
    public class MoveService : IMoveService
    {
        private int _remainingMoves;
        public event Action<int> OnMoveChanged;
        public event Action OnMoveRunOut;

        public MoveService(int startMoves)
        {
            _remainingMoves = startMoves;
        }

        public int GetRemainingMoves() => _remainingMoves;

        public void ConsumeMove()
        {
            _remainingMoves--;
            OnMoveChanged?.Invoke(_remainingMoves);
            if (_remainingMoves <= 0)
                OnMoveRunOut?.Invoke();
        }

        public void ResetMoves(int amount)
        {
            _remainingMoves = amount;
        }

        public bool HasMoves() => _remainingMoves > 0;
    }
}