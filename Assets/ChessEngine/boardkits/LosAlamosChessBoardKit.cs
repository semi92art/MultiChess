using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class LosAlamosChessBoardKit : ChessBoardKitBase
    {
        public LosAlamosChessBoardKit(IChessItemModel[,] figuresPlacement)
        {
            FiguresPlacement = figuresPlacement;
            Max_X = 6;
            Max_Y = 6;
            castlingPossibility = false;
        }

        public override bool CheckForCheck(ChessSide side)
        {
            var king_index = (byte)LosAlamosChessItemType.king;
            return CheckForCheck(side, king_index);

        }

        public override bool CheckForMate(ChessSide side)
        {
            var king_index = (byte)LosAlamosChessItemType.king;
            return CheckForMate(side, king_index);
        }
    }
}
