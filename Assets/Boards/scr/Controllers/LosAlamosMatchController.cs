using ChessEngine;

public sealed class LosAlamosMatchController : MatchControllerBase
{
    public LosAlamosMatchController(IChessMatchCurrentState matchCurrentState,
        IChessAction chessAction, IFigureOnBoard figureOnBoard, ICheckMate checkMate, IMoveBack moveBack, IGameSaver gameLoaderSaver, IMatchView_ControllerAPI matchView_ControllerAPI) :
        base(iMatchCurrentState: matchCurrentState,
            iChessAction: chessAction,
            iFigureOnBoard: figureOnBoard,
            iCheckMate: checkMate,
            iMoveBack : moveBack,
            iGameLoaderSaver : gameLoaderSaver,
            iMatchView_ContollerAPI: matchView_ControllerAPI)
    { }
}

