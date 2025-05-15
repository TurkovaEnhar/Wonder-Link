using Link;

namespace Board
{
    public interface IBoardAnalyzer
    {
        bool HasPossibleMoves(Tile[,] board, LinkMode linkMode);
    }
}