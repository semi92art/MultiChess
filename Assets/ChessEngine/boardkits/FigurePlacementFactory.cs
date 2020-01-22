
namespace ChessEngine
{
    public sealed class FigurePlacementFactory
    {
        public IChessItemModel[,] FiguresPlacement(string short_fen, int size_x, int size_y, ChessGameType type)
        {
            var result = new IChessItemModel[size_x, size_y];

            if (short_fen.Contains(" "))
                short_fen = short_fen.Split(' ')[0];

            string[] split = short_fen.Split('/');
            int y = size_y - 1;
            int x = 0;
            foreach (var fen_item in split)
            {
                x = 0;
                bool skip_next = false;
                foreach (var char_item in fen_item)
                {
                    if (skip_next)
                    {
                        skip_next = false;
                        continue;
                    }

                    if (char.IsDigit(char_item))
                    {
                        int count;
                        if (x + 1 < fen_item.Length && char.IsDigit(fen_item[x + 1]))
                        {
                            count = int.Parse((fen_item[x] + fen_item[x + 1]).ToString());
                            x++;
                            skip_next = true;
                        }
                        else
                            count = int.Parse(char_item.ToString());

                        for (int i = 0; i < count; i++)
                        {
                            result[x, y] = new NullChessItemModel(new BoardPosition(x, y));
                            x++;
                        }
                    }
                    else
                    {
                        var bp = new BoardPosition(x, y);
                        ChessSide side;
                        switch (type)
                        {
                            case ChessGameType.classic:
                                side = char.IsUpper(char_item) ? ChessSide.white : ChessSide.black;
                                switch (char.ToUpper(char_item))
                                {
                                    case 'P':
                                        result[x, y] = new ClassicPawnModel(side, bp);
                                        break;
                                    case 'R':
                                        result[x, y] = new ClassicRookModel(side, bp);
                                        break;
                                    case 'N':
                                        result[x, y] = new ClassicKnightModel(side, bp);
                                        break;
                                    case 'B':
                                        result[x, y] = new ClassicBishopModel(side, bp);
                                        break;
                                    case 'Q':
                                        result[x, y] = new ClassicQueenModel(side, bp);
                                        break;
                                    case 'K':
                                        result[x, y] = new ClassicKingModel(side, bp);
                                        break;
                                    default:
                                        throw new System.FormatException("FEN String Wrong Format!");
                                }
                                break;
                            case ChessGameType.los_alamos:
                                side = char.IsUpper(char_item) ? ChessSide.white : ChessSide.black;
                                switch (char.ToUpper(char_item))
                                {
                                    case 'P':
                                        result[x, y] = new LosAlamosPawnModel(side, bp);
                                        break;
                                    case 'R':
                                        result[x, y] = new LosAlamosRookModel(side, bp);
                                        break;
                                    case 'N':
                                        result[x, y] = new LosAlamosKnightModel(side, bp);
                                        break;
                                    case 'Q':
                                        result[x, y] = new LosAlamosQueenModel(side, bp);
                                        break;
                                    case 'K':
                                        result[x, y] = new LosAlamosKingModel(side, bp);
                                        break;
                                    default:
                                        throw new System.FormatException("FEN String Wrong Format!");
                                }
                                break;
                            case ChessGameType.chaturanga:
                                if ("PRNQK".Contains(char_item.ToString()))
                                    side = ChessSide.white;
                                else if ("prnqk".Contains(char_item.ToString()))
                                    side = ChessSide.black;
                                else if ("UVWYZ".Contains(char_item.ToString()))
                                    side = ChessSide.red;
                                else if ("uvwyz".Contains(char_item.ToString()))
                                    side = ChessSide.green;
                                else
                                    throw new System.NotImplementedException("Side was not implemented!");

                                if ("PpUu".Contains(char_item.ToString()))
                                    result[x, y] = new ChaturangaPawnModel(side, bp);
                                else if ("RrVv".Contains(char_item.ToString()))
                                    result[x, y] = new ChaturangaRookModel(side, bp);
                                else if ("NnWw".Contains(char_item.ToString()))
                                    result[x, y] = new ChaturangaKnightModel(side, bp);
                                else if ("QqYy".Contains(char_item.ToString()))
                                    result[x, y] = new ChaturangaQueenModel(side, bp);
                                else if ("KkZz".Contains(char_item.ToString()))
                                    result[x, y] = new ChaturangaKingModel(side, bp);
                                else
                                    throw new System.FormatException("FEN String Wrong Format!");
                                break;
                            case ChessGameType.circled:
                                side = char.IsUpper(char_item) ? ChessSide.white : ChessSide.black;
                                switch (char.ToUpper(char_item))
                                {
                                    case 'P':
                                        result[x, y] = new CircledPawnModel(side, bp, Direction.right);
                                        break;
                                    case 'T':
                                        result[x, y] = new CircledPawnModel(side, bp, Direction.left);
                                        break;
                                    case 'R':
                                        result[x, y] = new CircledRookModel(side, bp);
                                        break;
                                    case 'N':
                                        result[x, y] = new CircledKnightModel(side, bp);
                                        break;
                                    case 'B':
                                        result[x, y] = new CircledBishopModel(side, bp);
                                        break;
                                    case 'Q':
                                        result[x, y] = new CircledQueenModel(side, bp);
                                        break;
                                    case 'K':
                                        result[x, y] = new CircledKingModel(side, bp);
                                        break;
                                    default:
                                        throw new System.FormatException("FEN String Wrong Format!");
                                }
                                break;
                            default:
                                throw new System.NotImplementedException("FiguresPlacement Function Not Implemented Completely!");
                        }
                        x++;
                    }
                }
                y--;
            }

            return result;
        }
    }
}
