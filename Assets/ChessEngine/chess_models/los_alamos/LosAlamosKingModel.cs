
using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class LosAlamosKingModel : ChessItemModelBase
    {
        public LosAlamosKingModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)LosAlamosChessItemType.king, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetKingPossibleMoves(items, Side, Pos, true, Steps, false, out isJumpList, out killPossibilityList);
        }
    }
}
