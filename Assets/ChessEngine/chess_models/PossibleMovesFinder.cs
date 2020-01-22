using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public static class PossibleMovesFinder
    {
        public static List<BoardPosition> GetPawnPossibleMoves(IChessItemModel[,] items, ChessSide side, BoardPosition pos, bool doubleStepPossibility, int Steps, bool isCircled, Direction moveDirection, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            var result = new List<BoardPosition>();
            isJumpList = new List<bool>();
            killPossibilityList = new List<bool>();

            if (doubleStepPossibility && Steps == 0)
            {
                switch (moveDirection)
                {
                    case Direction.top:
                        if (items[pos.horizontal, pos.vertical + 1].IsNullObject)
                        {
                            if (items[pos.horizontal, pos.vertical + 2].IsNullObject || items[pos.horizontal, pos.vertical + 2].Side != side)
                                result.Add(new BoardPosition(pos.horizontal, pos.vertical + 2));
                        }
                        break;
                    case Direction.bottom:
                        if (items[pos.horizontal, pos.vertical - 1].IsNullObject)
                        {
                            if (items[pos.horizontal, pos.vertical - 2].IsNullObject || items[pos.horizontal, pos.vertical - 2].Side != side)
                                result.Add(new BoardPosition(pos.horizontal, pos.vertical - 2));
                        }
                        break;
                    case Direction.right:
                        if (items[pos.horizontal + 1, pos.vertical].IsNullObject)
                        {
                            if (items[pos.horizontal + 2, pos.vertical].IsNullObject || items[pos.horizontal + 2, pos.vertical].Side != side)
                                result.Add(new BoardPosition(pos.horizontal + 2, pos.vertical));
                        }
                        break;
                    case Direction.left:
                        if (items[pos.horizontal - 1, pos.vertical].IsNullObject)
                        {
                            if (items[pos.horizontal - 2, pos.vertical].IsNullObject || items[pos.horizontal - 2, pos.vertical].Side != side)
                                result.Add(new BoardPosition(pos.horizontal - 2, pos.vertical));
                        }
                        break;
                }
                isJumpList.Add(false);
                killPossibilityList.Add(false);
            }

            int min_x = 0;
            int min_y = 0;
            int max_x = items.GetLength(0) - 1;
            int max_y = items.GetLength(1) - 1;
            bool isJump;
           
            int new_x;
            int new_y;
            switch (moveDirection)
            {
                case Direction.top:
                    if (pos.vertical + 1 <= max_y && items[pos.horizontal, pos.vertical + 1].IsNullObject)
                    {
                        result.Add(new BoardPosition(pos.horizontal, pos.vertical + 1));
                        isJumpList.Add(false);
                        killPossibilityList.Add(false);
                    }
                    if (pos.horizontal + 1 <= max_x && pos.vertical + 1 <= max_y && !items[pos.horizontal + 1, pos.vertical + 1].IsNullObject && items[pos.horizontal + 1, pos.vertical + 1].Side != side)
                    {
                        result.Add(new BoardPosition(pos.horizontal + 1, pos.vertical + 1));
                        isJumpList.Add(false);
                        killPossibilityList.Add(true);
                    }
                    if (pos.horizontal - 1 >= 0 && pos.vertical + 1 <= max_y && !items[pos.horizontal - 1, pos.vertical + 1].IsNullObject && items[pos.horizontal - 1, pos.vertical + 1].Side != side)
                    {
                        result.Add(new BoardPosition(pos.horizontal - 1, pos.vertical + 1));
                        isJumpList.Add(false);
                        killPossibilityList.Add(true);
                    }
                    break;
                case Direction.bottom:
                    if (pos.vertical - 1 >= 0 && items[pos.horizontal, pos.vertical - 1].IsNullObject)
                    {
                        result.Add(new BoardPosition(pos.horizontal, pos.vertical - 1));
                        isJumpList.Add(false);
                        killPossibilityList.Add(false);
                    }
                    if (pos.horizontal + 1 <= max_x && pos.vertical - 1 >= 0 && !items[pos.horizontal + 1, pos.vertical - 1].IsNullObject && items[pos.horizontal + 1, pos.vertical - 1].Side != side)
                    {
                        result.Add(new BoardPosition(pos.horizontal + 1, pos.vertical - 1));
                        isJumpList.Add(false);
                        killPossibilityList.Add(true);
                    }
                    if (pos.horizontal - 1 >= 0 && pos.vertical - 1 >= 0 && !items[pos.horizontal - 1, pos.vertical - 1].IsNullObject && items[pos.horizontal - 1, pos.vertical - 1].Side != side)
                    {
                        result.Add(new BoardPosition(pos.horizontal - 1, pos.vertical - 1));
                        isJumpList.Add(false);
                        killPossibilityList.Add(true);
                    }
                    break;
                case Direction.right:
                    isJump = pos.horizontal + 1 >= max_x;

                    new_x = isCircled ? (pos.horizontal + 1) % (max_x + 1) : pos.horizontal + 1;
                    new_y = pos.vertical;
                    if (new_x <= max_x && items[new_x, new_y].IsNullObject)
                    {
                        result.Add(new BoardPosition(new_x, new_y));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(false);
                    }
                    new_x = isCircled ? (pos.horizontal + 1) % (max_x + 1) : pos.horizontal + 1;
                    new_y = pos.vertical - 1;
                    if (new_x <= max_x && new_y >= min_y && !items[new_x, new_y].IsNullObject && items[new_x, new_y].Side != side)
                    {
                        result.Add(new BoardPosition(new_x, new_y));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    new_x = isCircled ? (pos.horizontal + 1) % (max_x + 1) : pos.horizontal + 1;
                    new_y = pos.vertical + 1;
                    if (new_x >= min_x && new_y <= max_y && !items[new_x, new_y].IsNullObject && items[new_x, new_y].Side != side)
                    {
                        result.Add(new BoardPosition(new_x, new_y));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    break;
                case Direction.left:
                    isJump = pos.horizontal - 1 < min_x;

                    new_x = isCircled ? (pos.horizontal - 1 < min_x ? max_x : pos.horizontal - 1) : pos.horizontal - 1;
                    new_y = pos.vertical;
                    if (new_x >= 0 && items[new_x, new_y].IsNullObject)
                    {
                        result.Add(new BoardPosition(new_x, new_y));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(false);
                    }
                    new_x = isCircled ? (pos.horizontal - 1 < min_x ? max_x : pos.horizontal - 1) : pos.horizontal - 1;
                    new_y = pos.vertical - 1;
                    if (new_x >= min_x && new_y >= min_y && !items[new_x, new_y].IsNullObject && items[new_x, new_y].Side != side)
                    {
                        result.Add(new BoardPosition(new_x, new_y));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    new_x = isCircled ? (pos.horizontal - 1 < min_x ? max_x : pos.horizontal - 1) : pos.horizontal - 1;
                    new_y = pos.vertical + 1;
                    if (new_x >= min_x && new_y <= max_y && !items[new_x, new_y].IsNullObject && items[new_x, new_y].Side != side)
                    {
                        result.Add(new BoardPosition(new_x, new_y));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    break;
            }

            result.Add(pos);
            isJumpList.Add(false);
            killPossibilityList.Add(false);
            return result;
        }

        public static List<BoardPosition> GetRookPossibleMoves(IChessItemModel[,] items, ChessSide side, BoardPosition pos, int Steps, bool checkCastling, bool isCircled, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            List<BoardPosition> result = new List<BoardPosition>();
            isJumpList = new List<bool>();
            killPossibilityList = new List<bool>();

            //Check for castling
            if (Steps == 0 && checkCastling)
            {
                int vert_pos = -1;
                if (pos.vertical == 0 && side == ChessSide.white)
                    vert_pos = 0;
                else if (pos.vertical == items.GetLength(1) - 1 && side == ChessSide.black)
                    vert_pos = items.GetLength(1) - 1;

                IChessItemModel el4h = new NullChessItemModel();
                if (vert_pos != -1)
                    el4h = items[4, vert_pos];

                if (vert_pos != -1 && !el4h.IsNullObject && el4h.Type == (byte)ClassicChessItemType.king && el4h.Steps == 0 && el4h.Side == side)
                {
                    if (pos.horizontal == 0)
                    {
                        if (items[1, vert_pos].IsNullObject && items[2, vert_pos].IsNullObject && items[3, vert_pos].IsNullObject)
                        {
                            bool areNotUnderCheck = true;
                            foreach (var bp in new BoardPosition[] { new BoardPosition(2, vert_pos), new BoardPosition(3, vert_pos) })
                            {
                                foreach (var item in items)
                                {
                                    if (!item.IsNullObject && item.Side != side)
                                    {
                                        List<bool> bools;
                                        List<bool> bools2;
                                        var moves = GetRookPossibleMoves(items, side, pos, Steps, false, false, out bools, out bools2);
                                        foreach (var move in moves)
                                        {
                                            if (pos == move)
                                            {
                                                areNotUnderCheck = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (areNotUnderCheck)
                                {
                                    isJumpList.Add(false);
                                    killPossibilityList.Add(false);
                                    result.Add(new BoardPosition(4, vert_pos));
                                }
                            }
                        }
                    }
                    if (pos.horizontal == 7)
                    {
                        if (items[5, vert_pos].IsNullObject && items[6, vert_pos].IsNullObject)
                        {
                            bool areNotUnderCheck = true;
                            foreach (var bp in new BoardPosition[] { new BoardPosition(5, vert_pos), new BoardPosition(6, vert_pos) })
                            {
                                foreach (var item in items)
                                {
                                    if (!item.IsNullObject && item.Side != side)
                                    {
                                        List<bool> bools;
                                        List<bool> bools1;
                                        var moves = GetRookPossibleMoves(items, side, pos, Steps, false, false, out bools, out bools1);
                                        foreach (var move in moves)
                                        {
                                            if (pos == move)
                                            {
                                                areNotUnderCheck = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (areNotUnderCheck)
                                {
                                    result.Add(new BoardPosition(4, vert_pos));
                                    isJumpList.Add(false);
                                    killPossibilityList.Add(false);
                                }
                            }
                        }
                    }
                }
            }

            int min_x = 0;
            int min_y = 0;
            int max_x = items.GetLength(0) - 1;
            int max_y = items.GetLength(1) - 1;
            bool isJump;

            int k = pos.horizontal;
            if (isCircled && k <= min_x)
                k = max_x + 1;
            isJump = false;
            while (k > min_x)
            {
                if (items[k - 1, pos.vertical].IsNullObject)
                {
                    result.Add(new BoardPosition(k - 1, pos.vertical));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[k - 1, pos.vertical].Side != side)
                    {
                        result.Add(new BoardPosition(k - 1, pos.vertical));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }

                    break;
                }
                k--;

                if (isCircled && k <= min_x)
                {
                    k = max_x + 1;
                    isJump = true;
                }
                    
            }

            k = pos.horizontal;
            if (isCircled && k >= max_x)
                k = min_x - 1;
            isJump = false;
            while (k < max_x)
            {
                if (items[k + 1, pos.vertical].IsNullObject)
                {
                    result.Add(new BoardPosition(k + 1, pos.vertical));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[k + 1, pos.vertical].Side != side)
                    {
                        result.Add(new BoardPosition(k + 1, pos.vertical));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }

                    break;
                }
                k++;

                if (isCircled && k >= max_x)
                {
                    k = min_x - 1;
                    isJump = true;
                }
            }

            k = pos.vertical;
            isJump = false;
            while (k > 0)
            {
                if (items[pos.horizontal, k - 1].IsNullObject)
                {
                    result.Add(new BoardPosition(pos.horizontal, k - 1));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[pos.horizontal, k - 1].Side != side)
                    {
                        result.Add(new BoardPosition(pos.horizontal, k - 1));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }

                    break;
                }
                k--;
            }

            k = pos.vertical;
            isJump = false;
            while (k < items.GetLength(1) - 1)
            {
                if (items[pos.horizontal, k + 1].IsNullObject)
                {
                    result.Add(new BoardPosition(pos.horizontal, k + 1));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[pos.horizontal, k + 1].Side != side)
                    {
                        result.Add(new BoardPosition(pos.horizontal, k + 1));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }

                    break;
                }
                k++;
            }

            result.Add(pos);
            isJumpList.Add(false);
            killPossibilityList.Add(false);
            return result;
        }
        
        public static List<BoardPosition> GetKnightPossibleMoves(IChessItemModel[,] items, ChessSide side, BoardPosition pos, bool isCircled, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            List<BoardPosition> result = new List<BoardPosition>();
            isJumpList = new List<bool>();
            killPossibilityList = new List<bool>();
            killPossibilityList = new List<bool>();
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal - 2, pos.vertical + 1), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal - 1, pos.vertical + 2), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal + 1, pos.vertical + 2), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal + 2, pos.vertical + 1), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal + 2, pos.vertical - 1), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal + 1, pos.vertical - 2), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal - 1, pos.vertical - 2), side, isCircled, ref isJumpList, ref killPossibilityList);
            CheckAndAddKnightPosition(result, ref items, new BoardPosition(pos.horizontal - 2, pos.vertical - 1), side, isCircled, ref isJumpList, ref killPossibilityList);

            result.Add(pos);
            isJumpList.Add(false);
            killPossibilityList.Add(false);
            return result;
        }

        private static void CheckAndAddKnightPosition(List<BoardPosition> result, ref IChessItemModel[,] items, BoardPosition pos, ChessSide side, bool isCircled, ref List<bool> isJumpList, ref List<bool> killPossibilityList)
        {
            if (pos.vertical < 0 || pos.vertical >= items.GetLength(1))
                return;

            bool isJump = false;
            if (!isCircled)
            {
                if (pos.horizontal < 0 || pos.horizontal > items.GetLength(0) - 1)
                    return;
            }
            else
            {
                if (pos.horizontal < 0)
                {
                    pos.horizontal = (byte)(items.GetLength(0) + pos.horizontal);
                    isJump = true;
                }
                else if (pos.horizontal >= items.GetLength(0))
                {
                    pos.horizontal = (byte)(pos.horizontal % items.GetLength(0));
                    isJump = true;
                }

            }

            if (items[pos.horizontal, pos.vertical].IsNullObject || items[pos.horizontal, pos.vertical].Side != side)
            {
                result.Add(new BoardPosition(pos.horizontal, pos.vertical));
                isJumpList.Add(isJump);
                killPossibilityList.Add(true);
            }
        }

        public static List<BoardPosition> GetBishopPossibleMoves(IChessItemModel[,] items, ChessSide side, BoardPosition pos, bool isCircled, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            List<BoardPosition> result = new List<BoardPosition>();
            isJumpList = new List<bool>();
            killPossibilityList = new List<bool>();

            int min_x = 0;
            int min_y = 0;
            int max_x = items.GetLength(0) - 1;
            int max_y = items.GetLength(1) - 1;
            bool isJump;

            int kk, mm;

            isJump = false;
            int k = pos.horizontal;
            if (isCircled && k <= min_x)
            {
                k = max_x + 1;
                isJump = true;
            }
            int m = pos.vertical;

            //kk = max_x;
            //mm = max_y;
            while (k > min_x && m > min_y)
            {
                //kk--;
                //mm--;
                //if (kk < 0 || mm < 0)
                //    break;

                if (items[k - 1, m - 1].IsNullObject)
                {
                    result.Add(new BoardPosition(k - 1, m - 1));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[k - 1, m - 1].Side != side)
                    {
                        result.Add(new BoardPosition(k - 1, m - 1));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    break;
                }
                k--;
                m--;

                if (isCircled && k <= min_x)
                {
                    k = max_x + 1;
                    isJump = true;
                }
            }
            k = pos.horizontal;
            isJump = false;
            if (isCircled && k >= max_x)
            {
                k = min_x - 1;
                isJump = true;
            }
            m = pos.vertical;

            //kk = max_x;
            //mm = max_y;
            while (k < max_x && m > min_y)
            {
                //kk--;
                //mm--;
                //if (kk < 0 || mm < 0)
                //    break;

                if (items[k + 1, m - 1].IsNullObject)
                {
                    result.Add(new BoardPosition(k + 1, m - 1));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[k + 1, m - 1].Side != side)
                    {
                        result.Add(new BoardPosition(k + 1, m - 1));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    break;
                }
                k++;
                m--;

                if (isCircled && k >= max_x)
                {
                    k = min_x - 1;
                    isJump = true;
                }
            }
            k = pos.horizontal;
            m = pos.vertical;
            isJump = false;
            if (isCircled && k <= min_x)
            {
                k = max_x + 1;
                isJump = true;
            }

            //kk = max_x;
            //mm = max_y;
            while (k > min_x && m < max_y)
            {
                //kk--;
                //mm--;
                //if (kk < 0 || mm < 0)
                //    break;

                if (items[k - 1, m + 1].IsNullObject)
                {
                    result.Add(new BoardPosition(k - 1, m + 1));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[k - 1, m + 1].Side != side)
                    {
                        result.Add(new BoardPosition(k - 1, m + 1));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    break;
                }
                k--;
                m++;

                if (isCircled && k <= min_x)
                {
                    k = max_x + 1;
                    isJump = true;
                }
            }

            k = pos.horizontal;
            isJump = false;
            if (isCircled && k >= max_x)
            {
                k = min_x - 1;
                isJump = true;
            }
            m = pos.vertical;
            //kk = max_x;
            //mm = max_y;
            while (k < max_x && m < max_y)
            {
                //kk--;
                //mm--;
                //if (kk < 0 || mm < 0)
                //    break;

                if (items[k + 1, m + 1].IsNullObject)
                {
                    result.Add(new BoardPosition(k + 1, m + 1));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
                else
                {
                    if (items[k + 1, m + 1].Side != side)
                    {
                        result.Add(new BoardPosition(k + 1, m + 1));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                    break;
                }
                k++;
                m++;

                if (isCircled && k >= max_x)
                {
                    k = min_x - 1;
                    isJump = true;
                }
            }

            result.Add(pos);
            isJumpList.Add(false);
            killPossibilityList.Add(false);
            return result;
        }


        public static List<BoardPosition> GetKingPossibleMoves(IChessItemModel[,] items, ChessSide side, BoardPosition pos, bool checkForCastling, int Steps, bool isCircled, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            var result = new List<BoardPosition>();
            isJumpList = new List<bool>();
            killPossibilityList = new List<bool>();

            //Check for castling
            if (Steps == 0 && checkForCastling)
            {
                if (pos.horizontal == 4)
                {
                    int vert_pos = -1;
                    if (side == ChessSide.white && pos.vertical == 0)
                        vert_pos = 0;
                    else if (side == ChessSide.black && pos.vertical == items.GetLength(1) - 1)
                        vert_pos = items.GetLength(1) - 1;

                    if (vert_pos != -1)
                    {
                        if (!items[0, vert_pos].IsNullObject && items[0, vert_pos].Type == (byte)ClassicChessItemType.rook && items[0, vert_pos].Steps == 0)
                        {
                            if (items[1, vert_pos].IsNullObject && items[2, vert_pos].IsNullObject && items[3, vert_pos].IsNullObject)
                            {
                                bool areNotUnderCheck = true;
                                foreach (var pos_ in new BoardPosition[] { new BoardPosition(2, vert_pos), new BoardPosition(3, vert_pos) })
                                {
                                    foreach (var item in items)
                                    {
                                        if (!item.IsNullObject && item.Side != side)
                                        {
                                            List<bool> temp;
                                            List<bool> temp1;
                                            var moves = GetKingPossibleMoves(items, side, pos, false, Steps, false, out temp, out temp1);
                                            foreach (var move in moves)
                                            {
                                                if (pos_ == move)
                                                {
                                                    areNotUnderCheck = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (areNotUnderCheck)
                                {
                                    result.Add(new BoardPosition(0, vert_pos));
                                    isJumpList.Add(false);
                                    killPossibilityList.Add(true);
                                }
                            }
                        }

                        if (!items[items.GetLength(0) - 1, vert_pos].IsNullObject && items[items.GetLength(0) - 1, vert_pos].Type == (byte)ClassicChessItemType.rook && items[items.GetLength(0) - 1, vert_pos].Steps == 0)
                        {
                            if (items[5, vert_pos].IsNullObject && items[6, vert_pos].IsNullObject)
                            {
                                bool areNotUnderCheck = true;
                                foreach (var pos_ in new BoardPosition[] { new BoardPosition(2, vert_pos), new BoardPosition(3, vert_pos) })
                                {
                                    foreach (var item in items)
                                    {
                                        if (!item.IsNullObject && item.Side != side)
                                        {
                                            List<bool> temp;
                                            List<bool> temp1;
                                            var moves = GetKingPossibleMoves(items, side, pos, false, Steps, false, out temp, out temp1);
                                            foreach (var move in moves)
                                            {
                                                if (pos_ == move)
                                                {
                                                    areNotUnderCheck = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (areNotUnderCheck)
                                {
                                    result.Add(new BoardPosition(items.GetLength(1) - 1, vert_pos));
                                    isJumpList.Add(false);
                                    killPossibilityList.Add(true);
                                }
                            }
                        }
                    }
                }
            }

            int min_x = 0;
            int min_y = 0;
            int max_x = items.GetLength(0) - 1;
            int max_y = items.GetLength(1) - 1;
            bool isJump;

            bool l = false, r = false, b = false, t = false;

            if (pos.vertical > 0)
                b = true;
            if (pos.vertical < max_y)
                t = true;
            if (isCircled || (!isCircled && pos.horizontal > 0))
                l = true;
            if (isCircled || (!isCircled && pos.horizontal < max_x))
                r = true;

            isJump = false;
            int h, v;
            if (l)
            {
                h = pos.horizontal - 1;
                if (pos.horizontal - 1 < min_x)
                {
                    h = max_x;
                    isJump = true;
                }
                v = pos.vertical;

                if (items[h, v].IsNullObject || items[h, v].Side != side)
                {
                    result.Add(new BoardPosition(h, v));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }

                if (b)
                {
                    v = pos.vertical - 1;
                    if (items[h, v].IsNullObject || items[h, v].Side != side)
                    {
                        result.Add(new BoardPosition(h, v));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                }

                if (t)
                {
                    v = pos.vertical + 1;
                    if (items[h, v].IsNullObject || items[h, v].Side != side)
                    {
                        result.Add(new BoardPosition(h, v));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                }
            }

            isJump = false;
            if (r)
            {
                if (pos.horizontal + 1 > max_x)
                {
                    h = min_x;
                    isJump = true;
                }
                h = pos.horizontal + 1 > max_x ? min_x : pos.horizontal + 1;
                v = pos.vertical;
                if (items[h, v].IsNullObject || items[h, v].Side != side)
                {
                    result.Add(new BoardPosition(h, v));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }

                if (b)
                {
                    v = pos.vertical - 1;
                    if (items[h, v].IsNullObject || items[h, v].Side != side)
                    {
                        result.Add(new BoardPosition(h, v));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                }

                if (t)
                {
                    v = pos.vertical + 1;
                    if (items[h, v].IsNullObject || items[h, v].Side != side)
                    {
                        result.Add(new BoardPosition(h, v));
                        isJumpList.Add(isJump);
                        killPossibilityList.Add(true);
                    }
                }
            }

            if (t)
            {
                h = pos.horizontal;
                v = pos.vertical + 1;
                if (items[h, v].IsNullObject || items[h, v].Side != side)
                {
                    result.Add(new BoardPosition(h, v));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
            }

            if (b)
            {
                h = pos.horizontal;
                v = pos.vertical - 1;
                if (items[h, v].IsNullObject || items[h, v].Side != side)
                {
                    result.Add(new BoardPosition(h, v));
                    isJumpList.Add(isJump);
                    killPossibilityList.Add(true);
                }
            }

            result.Add(pos);
            isJumpList.Add(false);
            killPossibilityList.Add(false);
            return result;
        }
    }
}
