using System.Collections.Generic;


namespace ChessEngine
{
    public interface IFigureOnBoard
    {
        event System.EventHandler<ChessItemArgs> OnFigureRecovery;
        List<BoardPosition> GetPossibleMoves(BoardPosition bp, out List<bool> isJumpList);
        ChessItemModelShort GetFigureByPosition(BoardPosition bp);
        byte BoardSizeX { get; }
        byte BoardSizeY { get; }
    }
}
