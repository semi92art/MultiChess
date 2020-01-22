using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ChaturangaPawnModel : ChessItemModelBase
    {
        public ChaturangaPawnModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ChaturangaChessItemType.pawn, pos, steps) { }

        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            Direction direction;
            switch (Side)
            {
                case ChessSide.white:
                    direction = Direction.top;
                    break;
                case ChessSide.black:
                    direction = Direction.bottom;
                    break;
                case ChessSide.red:
                    direction = Direction.right;
                    break;
                case ChessSide.green:
                    direction = Direction.left;
                    break;
                default:
                    throw new System.NotImplementedException("GetPossibleMoves Not Implemented Completely!");
            }
            return PossibleMovesFinder.GetPawnPossibleMoves(items, Side, Pos, true, Steps, false, direction, out isJumpList, out killPossibilityList);
        }
    }
}
