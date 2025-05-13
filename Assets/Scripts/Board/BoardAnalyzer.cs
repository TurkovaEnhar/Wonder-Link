using System.Collections.Generic;
using UnityEngine;
using Board;
using Link;

namespace Game
{
    public class BoardAnalyzer : MonoBehaviour
    {
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private GameConfig gameConfig;

        public bool HasPossibleMoves()
        {
            Tile[,] board = boardManager.GetBoard();
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            HashSet<Vector2Int> globalVisited = new();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2Int pos = new(x, y);
                    if (globalVisited.Contains(pos)) continue;

                    Chip chip = board[x, y].CurrentChip;
                    if (chip == null) continue;

                    int groupSize = FloodFill(pos, chip.Color, board, globalVisited);

                    if (groupSize >= 3)
                        return true;
                }
            }

            return false;
        }

        private int FloodFill(Vector2Int start, ChipColor color, Tile[,] board, HashSet<Vector2Int> visited)
        {
            int width = board.GetLength(0);
            int height = board.GetLength(1);

            Stack<Vector2Int> stack = new();
            stack.Push(start);
            visited.Add(start);

            int count = 0;

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Pop();
                count++;

                foreach (Vector2Int dir in GetDirections())
                {
                    Vector2Int neighbor = current + dir;

                    if (!IsValid(neighbor, width, height) || visited.Contains(neighbor))
                        continue;

                    Chip neighborChip = board[neighbor.x, neighbor.y].CurrentChip;

                    if (neighborChip != null && neighborChip.Color == color)
                    {
                        stack.Push(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            return count;
        }

        private bool IsValid(Vector2Int pos, int width, int height)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height;
        }

        private IEnumerable<Vector2Int> GetDirections()
        {
            yield return Vector2Int.up;
            yield return Vector2Int.down;
            yield return Vector2Int.left;
            yield return Vector2Int.right;

            if (gameConfig.linkMode == LinkMode.EightWay)
            {
                yield return new Vector2Int(1, 1);
                yield return new Vector2Int(-1, 1);
                yield return new Vector2Int(1, -1);
                yield return new Vector2Int(-1, -1);
            }
        }
    }
}
