using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledBoardKit : ChessBoardKitBase
    {
        public CircledBoardKit(IChessItemModel[,] figuresPlacement)
        {
            Max_X = 16;
            Max_Y = 4;
            castlingPossibility = false;
            FiguresPlacement = figuresPlacement;
        }

        public override bool CheckForCheck(ChessSide side)
        {
            var king_index = (byte)CircledChessItemType.king;
            return CheckForCheck(side, king_index);

        }

        public override bool CheckForMate(ChessSide side)
        {
            var king_index = (byte)CircledChessItemType.king;
            return CheckForMate(side, king_index);
        }
    }
}
