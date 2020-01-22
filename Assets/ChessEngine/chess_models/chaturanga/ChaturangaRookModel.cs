using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ChaturangaRookModel : ChessItemModelBase
    {
        public ChaturangaRookModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ChaturangaChessItemType.rook, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetRookPossibleMoves(items, Side, Pos, Steps, true, false, out isJumpList, out killPossibilityList);
        }
    }
}
