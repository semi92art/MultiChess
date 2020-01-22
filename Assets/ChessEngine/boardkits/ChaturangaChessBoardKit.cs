using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ChaturangaChessBoardKit : ChessBoardKitBase
    {
        public ChaturangaChessBoardKit(IChessItemModel[,] figuresPlacement)
        {
            FiguresPlacement = figuresPlacement;
            Max_X = 8;
            Max_Y = 8;
            castlingPossibility = true;
        }

        public override bool CheckForCheck(ChessSide side)
        {
            var king_index = (byte)ChaturangaChessItemType.king;
            return CheckForCheck(side, king_index);
            
        }

        public override bool CheckForMate(ChessSide side)
        {
            var king_index = (byte)ChaturangaChessItemType.king;
            return CheckForMate(side, king_index);
        }
    }

    
}
