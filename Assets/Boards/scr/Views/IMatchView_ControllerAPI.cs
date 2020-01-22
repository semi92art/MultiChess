using UnityEngine;
using ChessEngine;

public interface IMatchView_ControllerAPI
{
    void StartMatch();
    void GetBoardData();
    void SelectFigure(Vector2Int pos);
    void MoveFigure(Vector2Int prev_pos, Vector2Int new_pos, ChessSide side);
    void KillFigure(Vector2Int pos);
    void SetCheck(ChessSide side);
    void SetMate(ChessSide side);
    void RecoveryChessItem(ChessItemProperties props);
}