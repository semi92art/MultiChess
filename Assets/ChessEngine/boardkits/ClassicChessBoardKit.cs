using System.Collections.Generic;

namespace ChessEngine
{

    /// <summary>
    /// Classic board 8x8 with classic placement of figures
    /// </summary>
    public sealed class ClassicChessBoardKit : ChessBoardKitBase
    {
        public ClassicChessBoardKit(IChessItemModel[,] figuresPlacement)
        {
            FiguresPlacement = figuresPlacement;
            Max_X = 8;
            Max_Y = 8;
            castlingPossibility = true;
        }

        public override bool CheckForCheck(ChessSide side)
        {
            var king_index = (byte)ClassicChessItemType.king;
            return CheckForCheck(side, king_index);

        }

        public override bool CheckForMate(ChessSide side)
        {
            var king_index = (byte)ClassicChessItemType.king;
            return CheckForMate(side, king_index);
        }
    }

}
