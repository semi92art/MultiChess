using System;
using ChessEngine;
using UnityEngine;



public sealed class CircledMatchController : MatchControllerBase
{
    public CircledMatchController(IChessMatchCurrentState iMatchCurrentState,
        IChessAction iChessAction, IFigureOnBoard iFigureOnBoard, ICheckMate iCheckMate, IMoveBack iMoveBack, IGameSaver iGameLoaderSaver, IMatchView_ControllerAPI matchView_ControllerAPI) :
        base(iMatchCurrentState, iChessAction, iFigureOnBoard, iCheckMate, iMoveBack, iGameLoaderSaver, matchView_ControllerAPI)
    { }
}
