using System.Collections.Generic;
using System.Linq;


namespace ChessEngine
{
    public static class PathFinder
    {
        public static BoardPosition[] GetPath(byte chess_type, BoardPosition from, BoardPosition to, BoardPosition boardSize, bool isJump)
        {
            if (new byte[]
            {
                (byte)ClassicChessItemType.rook,
                (byte)LosAlamosChessItemType.rook,
                (byte)ChaturangaChessItemType.rook,
                (byte)CircledChessItemType.rook
            }.Contains(chess_type))
                return GetLinearPath(from, to, boardSize, isJump);
            else if (new byte[]
            {
                (byte)ClassicChessItemType.bishop,
                (byte)CircledChessItemType.bishop
            }.Contains(chess_type))
                return GetDiagonalPath(from, to, boardSize, isJump);
            else if (new byte[]
            {
                (byte)ClassicChessItemType.knight,
                (byte)LosAlamosChessItemType.knight,
                (byte)ChaturangaChessItemType.knight,
                (byte)CircledChessItemType.knight
            }.Contains(chess_type))
                return GetKnightPath(from, to, boardSize, isJump);
            else
            {
                if (from.horizontal == to.horizontal || from.vertical == to.vertical)
                    return GetLinearPath(from, to, boardSize, isJump);
                else
                    return GetDiagonalPath(from, to, boardSize, isJump);
            }
        }

        private static BoardPosition[] GetLinearPath(BoardPosition from, BoardPosition to, BoardPosition boardSize, bool isJump)
        {
            if (from.horizontal != to.horizontal && from.vertical != to.vertical)
                throw new System.ArgumentException("Wrong from or to positions!");

            var result = new List<BoardPosition>();

            if (from.horizontal == to.horizontal)
            {
                if (from.vertical < to.vertical)
                {
                    if (!isJump)
                    {
                        //move up from start point to end point
                        for (int i = from.vertical; i <= to.vertical; i++)
                            result.Add(new BoardPosition(from.horizontal, i));
                    }
                    else
                    {
                        //move down from start point to bottom board edge
                        for (int i = from.vertical; i <= 0; i--)
                            result.Add(new BoardPosition(from.horizontal, i));
                        //move down from top board edge to end point
                        for (int i = boardSize.vertical - 1; i <= to.vertical; i--)
                            result.Add(new BoardPosition(from.horizontal, i));
                    }
                }
                else
                {
                    if (!isJump)
                    {
                        //move down from start point to end point
                        for (int i = from.vertical; i >= to.vertical; i--)
                            result.Add(new BoardPosition(from.horizontal, i));
                    }
                    else
                    {
                        //move up from start point to top board edge
                        for (int i = from.vertical; i <= boardSize.vertical; i++)
                            result.Add(new BoardPosition(from.horizontal, i));
                        //move up from bottom board edge to end point
                        for (int i = 0; i <= to.vertical; i++)
                            result.Add(new BoardPosition(from.horizontal, i));
                    }
                }
            }
            else
            {
                if (from.horizontal < to.horizontal)
                {
                    if (!isJump)
                    {
                        //move right from start point to end point
                        for (int i = from.horizontal; i <= to.horizontal; i++)
                            result.Add(new BoardPosition(i, from.vertical));
                    }
                    else
                    {
                        //move left from start point to left board edge
                        for (int i = from.horizontal; i <= 0; i--)
                            result.Add(new BoardPosition(i, from.vertical));
                        //move left from right board edge to end point
                        for (int i = boardSize.horizontal - 1; i <= to.horizontal; i--)
                            result.Add(new BoardPosition(i, from.vertical));
                    }
                }
                else
                {
                    if (!isJump)
                    {
                        //move left from start point to end point
                        for (int i = from.horizontal; i >= to.horizontal; i--)
                            result.Add(new BoardPosition(i, from.vertical));
                    }
                    else
                    {
                        //move right from start point to right board edge
                        for (int i = from.horizontal; i <= boardSize.horizontal; i++)
                            result.Add(new BoardPosition(i, from.vertical));
                        //move right from left board edge to end point
                        for (int i = 0; i <= to.horizontal; i++)
                            result.Add(new BoardPosition(i, from.vertical));
                    }
                }
            }

            return result.ToArray();
        }

        private static BoardPosition[] GetDiagonalPath(BoardPosition from, BoardPosition to, BoardPosition boardSize, bool isJump)
        {
            var result = new List<BoardPosition>();
            if (to.horizontal >= from.horizontal && to.vertical >= from.vertical)
            {
                if (!isJump)
                {
                    for (int i = from.horizontal; i <= to.horizontal; i++)
                        result.Add(new BoardPosition(i, i - from.horizontal + from.vertical));
                }
                else
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                    {
                        if (--i < 0) i = boardSize.horizontal - 1;
                        if (--j < 0) j = boardSize.vertical - 1;
                        result.Add(new BoardPosition(i, j));
                    }
                }
            }
            else if (to.horizontal >= from.horizontal && to.vertical <= from.vertical)
            {
                if (!isJump)
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                        result.Add(new BoardPosition(++i, --j));

                }
                else
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                    {
                        if (--i < 0) i = boardSize.horizontal - 1;
                        j = (++j) % boardSize.vertical;
                        result.Add(new BoardPosition(i, j));
                    }
                }
            }
            else if (to.horizontal <= from.horizontal && to.vertical <= from.vertical)
            {
                if (!isJump)
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                        result.Add(new BoardPosition(--i, --j));
                }
                else
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                    {
                        i = (++i) % boardSize.horizontal;
                        j = (--j) % boardSize.vertical;
                        result.Add(new BoardPosition(i, j));
                    }
                }
            }
            else if (to.horizontal <= from.horizontal && to.vertical >= from.vertical)
            {
                if (!isJump)
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                        result.Add(new BoardPosition(--i, ++j));
                }
                else
                {
                    int i = from.horizontal;
                    int j = from.vertical;
                    while (i != to.horizontal || j != to.vertical)
                    {
                        i = (++i) % boardSize.horizontal;
                        if (++j < 0) j = boardSize.vertical - 1;
                        result.Add(new BoardPosition(i, j));
                    }
                }
            }
            else
                throw new System.NotImplementedException("GetLinearPath Not Implemented Completely!");

            return result.ToArray();
        }

        private static BoardPosition[] GetKnightPath(BoardPosition from, BoardPosition to, BoardPosition boardSize, bool isJump)
        {
            return new BoardPosition[] { to };
        }
    }
}
