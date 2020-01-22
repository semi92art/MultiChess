
using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class NullChessItemModel : ChessItemModelBase
    {
        public NullChessItemModel() : base (ChessSide.white, 0, BoardPosition.None, 0, true)
        { }

        public NullChessItemModel(BoardPosition pos) : base(ChessSide.white, 0, pos, 0, true)
        { }

        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            isJumpList = new List<bool>();
            killPossibilityList = new List<bool>();
            return new List<BoardPosition>();
        }
    }
}
