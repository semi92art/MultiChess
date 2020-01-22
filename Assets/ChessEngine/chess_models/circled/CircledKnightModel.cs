using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledKnightModel : ChessItemModelBase
    {
        public CircledKnightModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)CircledChessItemType.knight, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetKnightPossibleMoves(items, Side, Pos, true, out isJumpList, out killPossibilityList);
        }
    }
}
