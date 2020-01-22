using System.Text;
using System.Linq;

namespace ChessEngine
{
    public static class UciConverter
    {
        public static string GetFigureLetter(bool isNullObject, ChessSide side, byte type)
        {
            if (isNullObject)
                return "-";

            switch (side)
            {
                case ChessSide.white:
                    if (new byte[] {
                        (byte)ClassicChessItemType.pawn,
                        (byte)LosAlamosChessItemType.pawn,
                        (byte)ChaturangaChessItemType.pawn,
                        (byte)CircledChessItemType.pawn_right
                    }.Contains(type))
                        return "P";
                    else if (new byte[]
                    {
                        (byte)CircledChessItemType.pawn_left
                    }.Contains(type))
                        return "T";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.rook,
                        (byte)LosAlamosChessItemType.rook,
                        (byte)ChaturangaChessItemType.rook,
                        (byte)CircledChessItemType.rook
                    }.Contains(type))
                        return "R";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.knight,
                        (byte)LosAlamosChessItemType.knight,
                        (byte)ChaturangaChessItemType.knight,
                        (byte)CircledChessItemType.knight
                    }.Contains(type))
                        return "N";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.bishop,
                    }.Contains(type))
                        return "B";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.queen,
                        (byte)LosAlamosChessItemType.queen,
                        (byte)ChaturangaChessItemType.queen,
                        (byte)CircledChessItemType.queen
                    }.Contains(type))
                        return "Q";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.king,
                        (byte)LosAlamosChessItemType.king,
                        (byte)ChaturangaChessItemType.king,
                        (byte)CircledChessItemType.king
                    }.Contains(type))
                        return "K";
                    break;
                case ChessSide.black:
                    if (new byte[] {
                        (byte)ClassicChessItemType.pawn,
                        (byte)LosAlamosChessItemType.pawn,
                        (byte)ChaturangaChessItemType.pawn,
                        (byte)CircledChessItemType.pawn_left
                    }.Contains(type))
                        return "p";
                    else if (new byte[]
                    {
                        (byte)CircledChessItemType.pawn_left
                    }.Contains(type))
                        return "t";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.rook,
                        (byte)LosAlamosChessItemType.rook,
                        (byte)ChaturangaChessItemType.rook,
                        (byte)CircledChessItemType.rook
                    }.Contains(type))
                        return "r";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.knight,
                        (byte)LosAlamosChessItemType.knight,
                        (byte)ChaturangaChessItemType.knight,
                        (byte)CircledChessItemType.knight
                    }.Contains(type))
                        return "n";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.bishop,
                    }.Contains(type))
                        return "b";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.queen,
                        (byte)LosAlamosChessItemType.queen,
                        (byte)ChaturangaChessItemType.queen,
                        (byte)CircledChessItemType.queen
                    }.Contains(type))
                        return "q";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.king,
                        (byte)LosAlamosChessItemType.king,
                        (byte)ChaturangaChessItemType.king,
                        (byte)CircledChessItemType.king
                    }.Contains(type))
                        return "k";
                    break;
                case ChessSide.red:
                    if (new byte[] {
                        (byte)ClassicChessItemType.pawn,
                        (byte)LosAlamosChessItemType.pawn,
                        (byte)ChaturangaChessItemType.pawn,
                        (byte)CircledChessItemType.pawn_left
                    }.Contains(type))
                        return "U";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.rook,
                        (byte)LosAlamosChessItemType.rook,
                        (byte)ChaturangaChessItemType.rook,
                        (byte)CircledChessItemType.rook
                    }.Contains(type))
                        return "V";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.knight,
                        (byte)LosAlamosChessItemType.knight,
                        (byte)ChaturangaChessItemType.knight,
                        (byte)CircledChessItemType.knight
                    }.Contains(type))
                        return "W";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.bishop,
                    }.Contains(type))
                        return "X";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.queen,
                        (byte)LosAlamosChessItemType.queen,
                        (byte)ChaturangaChessItemType.queen,
                        (byte)CircledChessItemType.queen
                    }.Contains(type))
                        return "Y";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.king,
                        (byte)LosAlamosChessItemType.king,
                        (byte)ChaturangaChessItemType.king,
                        (byte)CircledChessItemType.king
                    }.Contains(type))
                        return "Z";
                    break;
                case ChessSide.green:
                    if (new byte[] {
                        (byte)ClassicChessItemType.pawn,
                        (byte)LosAlamosChessItemType.pawn,
                        (byte)ChaturangaChessItemType.pawn,
                        (byte)CircledChessItemType.pawn_left
                    }.Contains(type))
                        return "u";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.rook,
                        (byte)LosAlamosChessItemType.rook,
                        (byte)ChaturangaChessItemType.rook,
                        (byte)CircledChessItemType.rook
                    }.Contains(type))
                        return "v";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.knight,
                        (byte)LosAlamosChessItemType.knight,
                        (byte)ChaturangaChessItemType.knight,
                        (byte)CircledChessItemType.knight
                    }.Contains(type))
                        return "w";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.bishop,
                    }.Contains(type))
                        return "x";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.queen,
                        (byte)LosAlamosChessItemType.queen,
                        (byte)ChaturangaChessItemType.queen,
                        (byte)CircledChessItemType.queen
                    }.Contains(type))
                        return "y";
                    else if (new byte[] {
                        (byte)ClassicChessItemType.king,
                        (byte)LosAlamosChessItemType.king,
                        (byte)ChaturangaChessItemType.king,
                        (byte)CircledChessItemType.king
                    }.Contains(type))
                        return "z";
                    break;
                default:
                    break;
            }

            return "-";
        }

        public static string ConvertToShortFEN(IChessItemModel[,] ch_on_b)
        {
            //Figure placement
            StringBuilder sb = new StringBuilder();
            int empty_count = 0;
            for (int i = ch_on_b.GetLength(1) - 1; i >= 0; i--)
            {
                for (int j = 0; j < ch_on_b.GetLength(0); j++)
                {
                    IChessItemModel ch_item = ch_on_b[j, i];

                    if (!ch_item.IsNullObject)
                    {
                        if (empty_count > 0)
                        {
                            sb.Append(empty_count);
                            empty_count = 0;
                        }

                        var s = GetFigureLetter(false, ch_item.Side, ch_item.Type);
                        sb.Append(s);
                    }
                    else
                    {
                        empty_count++;
                    }


                }

                if (empty_count > 0)
                {
                    sb.Append(empty_count);
                    empty_count = 0;
                }

                if (i != 0)
                    sb.Append('/');
            }

            return sb.ToString();
        }

        public static string ConvertToFEN(IChessItemModel[,] ch_on_b, ChessSide currentSide, Castling whitePossibleCastling, Castling blackPossibleCastling, string captureOnTheIsle, int numberOfReversibleSemiSteps, int moveCount)
        {
            StringBuilder sb = new StringBuilder();

            //Figure placement
            string figurePlacement = ConvertToShortFEN(ch_on_b);
            sb.Append(figurePlacement.Split(' ')[0]);

            //Current side (black/white)
            sb.Append(' ');
            sb.Append(currentSide == ChessSide.black ? "b" : "w");
            //Castling possibility
            sb.Append(' ');
            bool isWhiteCastlingDone = false;
            switch (whitePossibleCastling)
            {
                case Castling._none:
                    isWhiteCastlingDone = true;
                    break;
                case Castling._short:
                    sb.Append('K');
                    break;
                case Castling._long:
                    sb.Append('Q');
                    break;
                case Castling._short_and_long:
                    sb.Append("KQ");
                    break;
                case Castling._done:
                    isWhiteCastlingDone = true;
                    break;
            }

            switch (blackPossibleCastling)
            {
                case Castling._none:
                    if (isWhiteCastlingDone)
                        sb.Append('-');
                    break;
                case Castling._short:
                    sb.Append('k');
                    break;
                case Castling._long:
                    sb.Append('q');
                    break;
                case Castling._short_and_long:
                    sb.Append("kq");
                    break;
                case Castling._done:
                    if (isWhiteCastlingDone)
                        sb.Append('-');
                    break;
            }

            //Capture on the isle
            sb.Append(' ');
            sb.Append(captureOnTheIsle);
            sb.Append(' ');
            sb.Append(numberOfReversibleSemiSteps);
            sb.Append(' ');
            sb.Append(moveCount);

            return sb.ToString();
        }

        

        public static void GetBoardPositionsFromMoveCommand(string move_cmd, out BoardPosition from, out BoardPosition to)
        {
            byte x = 0, y = 0;
            x = byte.Parse(((char)((byte)(move_cmd[0].ToString().ToUpper()[0]) - 17)).ToString());
            y = (byte)(byte.Parse(move_cmd[1].ToString()));
            from = new BoardPosition(x, y - 1);
            x = byte.Parse(((char)((byte)(move_cmd[2].ToString().ToUpper()[0]) - 17)).ToString());
            y = (byte)(byte.Parse(move_cmd[3].ToString()));
            to = new BoardPosition(x, y - 1);
        }

        public static string BoardPositionToString(BoardPosition bp)
        {
            if (bp == BoardPosition.None)
                return "-";

            StringBuilder sb = new StringBuilder();
            //char let =  bp.horizontal.ToString()
            char letter = (char)((char)48 + (char)bp.horizontal + (char)17);
            sb.Append(letter);
            sb.Append(bp.vertical);
            return sb.ToString();
        }
    }
}



