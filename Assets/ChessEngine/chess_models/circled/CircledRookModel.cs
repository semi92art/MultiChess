using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledRookModel : ChessItemModelBase
    {
        public CircledRookModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)CircledChessItemType.rook, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetRookPossibleMoves(items, Side, Pos, Steps, true, true, out isJumpList, out killPossibilityList);
        }
    }
}
