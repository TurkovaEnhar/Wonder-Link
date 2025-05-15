using Game;
using UnityEngine;

namespace MoveSystem
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private MoveView view;

        private IMoveService _moveService;

        public IMoveService GetMoveService() => _moveService;

        public void Initialize(int moveCount)
        {
            _moveService = new MoveService(moveCount);
            
            _moveService.OnMoveChanged += view.UpdateMoveText;
            _moveService.OnMoveRunOut += HandleMoveRunOut;
            view.UpdateMoveText(moveCount);
            
        }
        
        private void HandleMoveRunOut()
        {
            Debug.Log("No moves Left");
        }
        private void OnDestroy()
        {
            if (_moveService != null)
                _moveService.OnMoveRunOut -= HandleMoveRunOut;
        }
    }
}