using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ClassicKingModel : ChessItemModelBase
    {
        public ClassicKingModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ClassicChessItemType.king, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetKingPossibleMoves(items, Side, Pos, true, Steps, false, out isJumpList, out killPossibilityList); 
        }
    }
}
