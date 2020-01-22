using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledBishopModel : ChessItemModelBase
    {
        public CircledBishopModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)CircledChessItemType.bishop, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetBishopPossibleMoves(items, Side, Pos, true, out isJumpList, out killPossibilityList);
        }
    }
}
