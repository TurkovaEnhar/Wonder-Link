using System;

namespace MoveSystem
{
    public interface IMoveService
    {
        int GetRemainingMoves();
        void ConsumeMove();
        void ResetMoves(int amount);
        bool HasMoves();
        event Action OnMoveRunOut;
        event Action<int> OnMoveChanged;
    }
}