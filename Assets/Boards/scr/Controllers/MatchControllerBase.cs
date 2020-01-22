using System;
using ChessEngine;
using UnityEngine;

public abstract class MatchControllerBase : IMatchController
{
    public IChessMatchCurrentState ChessMatchCurrentState { get; protected set; }
    public IChessAction ChessAction { get; protected set; }
    public IFigureOnBoard FigureOnBoard { get; protected set; }
    public IFEN FEN { get; protected set; }
    public ICheckMate CheckMate { get; protected set; }
    public IMoveBack MoveBack { get; protected set; }
    public IGameSaver GameLoaderSaver { get; protected set; }
    public IMatchView_ControllerAPI MatchView_ControllerAPI { get; protected set; }

    public MatchControllerBase(IChessMatchCurrentState iMatchCurrentState,
        IChessAction iChessAction, IFigureOnBoard iFigureOnBoard, ICheckMate iCheckMate, IMoveBack iMoveBack, IGameSaver iGameLoaderSaver, IMatchView_ControllerAPI iMatchView_ContollerAPI)
    {
        ChessMatchCurrentState = iMatchCurrentState;
        ChessAction = iChessAction;
        FigureOnBoard = iFigureOnBoard;
        CheckMate = iCheckMate;
        MoveBack = iMoveBack;
        GameLoaderSaver = iGameLoaderSaver;
        MatchView_ControllerAPI = iMatchView_ContollerAPI;

        ChessAction.OnChanged += OnChangedFunction;
        CheckMate.OnCheck += SetCheck;
        CheckMate.OnMate += SetMate;
        FigureOnBoard.OnFigureRecovery += OnFigureRecovery;

        MatchView_ControllerAPI.GetBoardData();
    }

    private void OnFigureRecovery(object sender, ChessItemArgs e)
    {
        var new_props = new ChessItemProperties(e.ChessItem.Pos.ToVector2Int(), e.ChessItem.Type, e.ChessItem.Side, e.ChessItem.IsNullObject);
        MatchView_ControllerAPI.RecoveryChessItem(new_props);
    }

    #region FROM MODEL TO VIEW

    protected virtual void OnChangedFunction(object sender, ChessSideArgs args)
    {
        MatchView_ControllerAPI.GetBoardData();
        switch (ChessAction.LastPlayerAction)
        {
            case PlayerAction.Nothing:
                break;
            case PlayerAction.Select:
                SelectFigure(args.Side);
                break;
            case PlayerAction.Move:
                MoveFigure(args.Side);
                break;
            case PlayerAction.MoveAndKill:
                KillFigure(args.Side);
                MoveFigure(args.Side);
                break;
            case PlayerAction.BackMove:
                break;
            default:
                throw new System.NotImplementedException("OnChengedFunction Was Not Implemented Completely!");
        }
    }

    protected virtual void SelectFigure(ChessSide side)
    {
        MatchView_ControllerAPI.SelectFigure(ChessMatchCurrentState.CurrentSelectedPosition.ToVector2Int());
    }

    protected virtual void MoveFigure(ChessSide side)
    {
        MatchView_ControllerAPI.MoveFigure(
            ChessMatchCurrentState.PreviousSelectedPosition.ToVector2Int(),
            ChessMatchCurrentState.CurrentSelectedPosition.ToVector2Int(),
            side);
    }

    protected virtual void KillFigure(ChessSide side)
    {
        MatchView_ControllerAPI.KillFigure(ChessMatchCurrentState.CurrentSelectedPosition.ToVector2Int());
    }

    protected virtual void SetCheck(object sender, ChessSideArgs args)
    {
        MatchView_ControllerAPI.SetCheck(args.Side);
    }

    protected virtual void SetMate(object sender, ChessSideArgs args)
    {
        MatchView_ControllerAPI.SetMate(args.Side);
    }
    #endregion
}

