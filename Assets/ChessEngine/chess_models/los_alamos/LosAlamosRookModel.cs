using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public sealed class LosAlamosRookModel : ChessItemModelBase
    {
        public LosAlamosRookModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)LosAlamosChessItemType.rook, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetRookPossibleMoves(items, Side, Pos, Steps, false, false, out isJumpList, out killPossibilityList);
        }
    }
}
