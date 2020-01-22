
using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledPawnModel : ChessItemModelBase
    {
        private readonly Direction direction;

        public CircledPawnModel(ChessSide side, BoardPosition pos, Direction direction, int steps = 0) : base(side, (byte)CircledChessItemType.pawn_right, pos, steps)
        {
            this.direction = direction;
            switch (direction)
            {
                case Direction.left:
                    Type = (byte)CircledChessItemType.pawn_left;
                    break;
                case Direction.right:
                    Type = (byte)CircledChessItemType.pawn_right;
                    break;
                default:
                    throw new System.NotImplementedException("CircledPawnModel Constructor Not Implemented Completely!");
            }
        }

        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            return PossibleMovesFinder.GetPawnPossibleMoves(items, Side, Pos, true, Steps, true, direction, out isJumpList, out killPossibilityList);
        }
    }
}
