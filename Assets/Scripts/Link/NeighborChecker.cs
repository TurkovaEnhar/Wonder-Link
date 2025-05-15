using Board.Chips;
using UnityEngine;

namespace Link
{
    public class NeighborChecker : INeighborChecker
    {
        public bool AreNeighbors(Chip a, Chip b, LinkMode mode)
        {
            Vector2Int posA = a.ParentTile.GridPosition;
            Vector2Int posB = b.ParentTile.GridPosition;
            Vector2Int diff = posB - posA;

            return mode == LinkMode.FourWay
                ? Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1
                : Mathf.Abs(diff.x) <= 1 && Mathf.Abs(diff.y) <= 1 && diff != Vector2Int.zero;
        }
    }
}