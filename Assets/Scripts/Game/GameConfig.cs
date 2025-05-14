using Board;
using Link;

namespace Game
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject,IBoardSettingsProvider
    {
        [Header("Link Settings")]
        public LinkMode linkMode = LinkMode.FourWay;
        [Header("Board Settings")]
        [SerializeField] private int width = 8;
        [SerializeField] private int height = 8;
        [Header("Game Settings")]
        [SerializeField] private int targetScore = 100;
        [SerializeField] private int moveCount = 10;
        [SerializeField] private int basePointPerChip = 10;
        [SerializeField] private bool autoEndOnTarget;

        public int GetBoardWidth() => width;
        public int GetBoardHeight() => height;
        public int GetTargetScore() => targetScore;
        public int GetMoveCount() => moveCount;
        public int GetBasePointPerChip() => basePointPerChip;
        public bool GetAutoEndOnTarget() => autoEndOnTarget;
        
    }

}