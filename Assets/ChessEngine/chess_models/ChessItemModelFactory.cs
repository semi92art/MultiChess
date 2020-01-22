

namespace ChessEngine
{
    public sealed class ChessItemModelFactory
    {
        public IChessItemModel CreateChessItemModel(BoardPosition pos, byte type, int steps, ChessSide side)
        {
            switch (type)
            {
                //Classic
                case (byte)ClassicChessItemType.pawn:
                    return new ClassicPawnModel(side, pos, steps);
                case (byte)ClassicChessItemType.rook:
                    return new ClassicRookModel(side, pos, steps);
                case (byte)ClassicChessItemType.knight:
                    return new ClassicKnightModel(side, pos, steps);
                case (byte)ClassicChessItemType.bishop:
                    return new ClassicBishopModel(side, pos, steps);
                case (byte)ClassicChessItemType.queen:
                    return new ClassicQueenModel(side, pos, steps);
                case (byte)ClassicChessItemType.king:
                    return new ClassicKingModel(side, pos, steps);
                //Los Alamos
                case (byte)LosAlamosChessItemType.pawn:
                    return new LosAlamosPawnModel(side, pos, steps);
                case (byte)LosAlamosChessItemType.rook:
                    return new LosAlamosRookModel(side, pos, steps);
                case (byte)LosAlamosChessItemType.knight:
                    return new LosAlamosKnightModel(side, pos, steps);
                case (byte)LosAlamosChessItemType.queen:
                    return new LosAlamosQueenModel(side, pos, steps);
                case (byte)LosAlamosChessItemType.king:
                    return new LosAlamosKingModel(side, pos, steps);
                //Chaturanga
                case (byte)ChaturangaChessItemType.pawn:
                    return new ChaturangaPawnModel(side, pos, steps);
                case (byte)ChaturangaChessItemType.rook:
                    return new ChaturangaRookModel(side, pos, steps);
                case (byte)ChaturangaChessItemType.knight:
                    return new ChaturangaKnightModel(side, pos, steps);
                case (byte)ChaturangaChessItemType.queen:
                    return new ChaturangaQueenModel(side, pos, steps);
                case (byte)ChaturangaChessItemType.king:
                    return new ChaturangaKingModel(side, pos, steps);
                //Circled
                case (byte)CircledChessItemType.pawn_left:
                    return new CircledPawnModel(side, pos, Direction.left, steps);
                case (byte)CircledChessItemType.pawn_right:
                    return new CircledPawnModel(side, pos, Direction.right, steps);
                case (byte)CircledChessItemType.rook:
                    return new CircledRookModel(side, pos, steps);
                case (byte)CircledChessItemType.knight:
                    return new CircledKnightModel(side, pos, steps);
                case (byte)CircledChessItemType.bishop:
                    return new CircledBishopModel(side, pos, steps);
                case (byte)CircledChessItemType.queen:
                    return new CircledQueenModel(side, pos, steps);
                case (byte)CircledChessItemType.king:
                    return new CircledKingModel(side, pos, steps);
                default:
                    throw new System.NotImplementedException("CreateChessItemModel Not Implemented Completely!");
            }
        }
    }
}
