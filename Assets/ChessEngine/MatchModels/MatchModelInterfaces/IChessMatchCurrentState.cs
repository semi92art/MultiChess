using System;
using System.Collections.Generic;


namespace ChessEngine
{
    public interface IChessMatchCurrentState
    {
        IPlayer CurrentPlayer { get; }
        IPlayer PreviousPlayer { get; }
        List<FigureMove> AllMoves { get; }
        BoardPosition CurrentSelectedPosition { get; }
        BoardPosition PreviousSelectedPosition { get; }   
        bool IsJump { get; }
        MatchStage Match_Stage { get; }
    }
}


