using Board.Chips;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int GridPosition { get; private set; }
        public Chip CurrentChip { get; private set; }

        public void Initialize(int x, int y)
        {
            GridPosition = new Vector2Int(x, y);
        }

        public void SetChip(Chip chip)
        {
            CurrentChip = chip;
        }

        public void ClearChip()
        {
            CurrentChip = null;
        }
    }
}