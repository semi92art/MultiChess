using UnityEngine;
using ChessEngine;

public interface IMatchView_Internal
{
    void InitObjects(IChessMatchCurrentState iChessMatchCurrentState, IChessAction iChessAction, IMoveBack iMoveBack, IFigureOnBoard iFigureOnBoard);
    void SelectBoardItem(Vector2Int pos);
    void SetScreenPosition(int x_pos, int y_pos);
    void FinishMove();
    Vector3 BoardPositionMono { get; }
    bool IsBoardPositionUpdated { get; }
}
