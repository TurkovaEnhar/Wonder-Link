using Board.Chips;

namespace Link
{
    public interface INeighborChecker
    {
        bool AreNeighbors(Chip a, Chip b, LinkMode mode);
    }
}