using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Board Settings")]
    public class BoardSettings : ScriptableObject, IBoardSettingsProvider
    {
        [SerializeField] private int width = 8;
        [SerializeField] private int height = 8;

        public int GetBoardWidth() => width;
        public int GetBoardHeight() => height;
    }
}