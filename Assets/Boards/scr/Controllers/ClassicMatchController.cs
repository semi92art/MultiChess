using System;
using ChessEngine;
using UnityEngine;


public sealed class ClassicMatchController : MatchControllerBase
{
    public ClassicMatchController(IChessMatchCurrentState iMatchCurrentState,
        IChessAction iChessAction, IFigureOnBoard iFigureOnBoard, ICheckMate iCheckMate, IMoveBack iMoveBack, IGameSaver iGameLoaderSaver, IFEN iFEN, IMatchView_ControllerAPI matchView_ControllerAPI) :
        base (iMatchCurrentState, iChessAction, iFigureOnBoard, iCheckMate, iMoveBack, iGameLoaderSaver, matchView_ControllerAPI)
    {
        FEN = iFEN;
    }
}

