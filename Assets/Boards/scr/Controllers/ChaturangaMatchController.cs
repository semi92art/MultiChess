using ChessEngine;

public sealed class ChaturangaMatchController : MatchControllerBase
{
    public ChaturangaMatchController(IChessMatchCurrentState iMatchCurrentState,
        IChessAction iChessAction, IFigureOnBoard iFigureOnBoard, ICheckMate iCheckMate, IMoveBack iMoveBack, IGameSaver iGameLoaderSaver, IMatchView_ControllerAPI matchView_ControllerAPI) :
        base(iMatchCurrentState, iChessAction, iFigureOnBoard, iCheckMate, iMoveBack, iGameLoaderSaver, matchView_ControllerAPI)
    { }
}

