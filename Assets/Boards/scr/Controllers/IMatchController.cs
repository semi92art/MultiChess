using ChessEngine;


public interface IMatchController
{
    IChessMatchCurrentState ChessMatchCurrentState { get; }
    IFEN FEN { get; }
    IFigureOnBoard FigureOnBoard { get; }
    IChessAction ChessAction { get; }
    ICheckMate CheckMate { get; }
    IMoveBack MoveBack { get; }
    IGameSaver GameLoaderSaver { get; }
    IMatchView_ControllerAPI MatchView_ControllerAPI { get; }
}

