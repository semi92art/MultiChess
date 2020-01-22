using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledKingModel : ChessItemModelBase
    {
        public CircledKingModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)CircledChessItemType.king, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetKingPossibleMoves(items, Side, Pos, true, Steps, true, out isJumpList, out killPossibilityList);
        }
    }
}
