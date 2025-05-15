using Game;
using UnityEngine;

namespace MoveSystem
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private MoveView view;

        private IMoveService _moveService;

        public IMoveService GetMoveService() => _moveService;

        public void Initialize(GameConfig config)
        {
            _moveService = new MoveService(config.GetMoveCount());
            
            _moveService.OnMoveChanged += view.UpdateMoveText;
            _moveService.OnMoveRunOut += HandleMoveRunOut;
            view.UpdateMoveText(config.GetMoveCount());
            
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