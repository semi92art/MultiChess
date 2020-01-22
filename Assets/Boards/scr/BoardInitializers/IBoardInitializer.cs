using System.Collections.Generic;
using ChessEngine;
public interface IBoardInitializer
{
    IMatchController InitializeBoard();
    IMatchController InitializeBoard(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions);
}

