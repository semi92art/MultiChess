using System.Collections.Generic;
namespace ChessEngine
{
    public interface IChessItemModel : IChessItemModelShort
    { 
        void SetPosition(BoardPosition pos);
        List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList);
    }
}
