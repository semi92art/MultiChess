using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ChaturangaKnightModel : ChessItemModelBase
    {
        public ChaturangaKnightModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ChaturangaChessItemType.knight, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetKnightPossibleMoves(items, Side, Pos, false, out isJumpList, out killPossibilityList);
        }
    }
}