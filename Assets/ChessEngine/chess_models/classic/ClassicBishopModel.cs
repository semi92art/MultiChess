using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ClassicBishopModel : ChessItemModelBase
    {
        public ClassicBishopModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ClassicChessItemType.bishop, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetBishopPossibleMoves(items, Side, Pos, false, out isJumpList, out killPossibilityList);
        }
    }
}
