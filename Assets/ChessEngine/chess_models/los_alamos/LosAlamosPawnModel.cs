using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public sealed class LosAlamosPawnModel : ChessItemModelBase
    {
        public LosAlamosPawnModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)LosAlamosChessItemType.pawn, pos, steps) { }

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
                default:
                    throw new System.NotImplementedException("GetPossibleMoves Not Implemented Completely!");
            }
            return PossibleMovesFinder.GetPawnPossibleMoves(items, Side, Pos, false, Steps, false, direction, out isJumpList, out killPossibilityList);
        }
    }
}
