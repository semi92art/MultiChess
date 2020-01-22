

namespace ChessEngine
{
    public interface ICastling
    {
        Castling WhitePossibleCastling { get; }
        Castling BlackPossibleCastling { get; }
    }
}
