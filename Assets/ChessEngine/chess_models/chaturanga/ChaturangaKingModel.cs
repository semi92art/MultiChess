
using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ChaturangaKingModel : ChessItemModelBase
    {
        public ChaturangaKingModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ChaturangaChessItemType.king, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetKingPossibleMoves(items, Side, Pos, false, Steps, false, out isJumpList, out killPossibilityList);
        }
    }
}
