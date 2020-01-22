using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public abstract class ChessBoardKitBase  : IChessBoardKit
    {
        public IChessItemModel[,] FiguresPlacement { get; protected set; }
        public byte Max_X { get; protected set; }
        public byte Max_Y { get; protected set; }

        protected bool castlingPossibility;

        public IChessItemModel GetFigureFromBoard(BoardPosition pos)
        {
            return FiguresPlacement[pos.horizontal, pos.vertical];
        }

        public void SetFigureOnBoard(BoardPosition pos, IChessItemModel chessItem)
        {
            FiguresPlacement[pos.horizontal, pos.vertical] = chessItem;
            if (!chessItem.IsNullObject)
                FiguresPlacement[pos.horizontal, pos.vertical].SetPosition(pos);
        }

        public virtual bool TryMoveChessItem(ChessSide curr_side, BoardPosition from, BoardPosition to, out BoardPosition captureOnTheIsle, out IChessItemModel killed_model)
        {
            killed_model = new NullChessItemModel();
            captureOnTheIsle = BoardPosition.None;
            //If figure wasn't chosen before or new selected figure is the same
            if (GetFigureFromBoard(from).IsNullObject || from == to)
            {
                captureOnTheIsle = BoardPosition.None;
                return false;
            }
            else
            {
                //If selected board item has figure
                if (!GetFigureFromBoard(to).IsNullObject)
                {
                    //If sides of previous selected figure and current are same
                    if (GetFigureFromBoard(to).Side == curr_side)
                    {
                        captureOnTheIsle = BoardPosition.None;

                        return castlingPossibility &&
                            GetFigureFromBoard(to).Type == (byte)ClassicChessItemType.king && GetFigureFromBoard(from).Type == (byte)ClassicChessItemType.rook ||
                                GetFigureFromBoard(to).Type == (byte)ClassicChessItemType.rook && GetFigureFromBoard(from).Type == (byte)ClassicChessItemType.king;
                    }
                    //If side of previous selected and current are different
                    else
                    {
                        captureOnTheIsle = BoardPosition.None;

                        //MoveChessItem(from, to, false);
                        var temp_chess_item = MoveChessItemPredictForward(from, to);
                        if (CheckForCheck(curr_side))
                        {
                            killed_model = new NullChessItemModel();
                            MoveChessItemPredictBackward(temp_chess_item, from, to);
                            return false;
                        }
                        else
                        {
                            MoveChessItemPredictBackward(temp_chess_item, from, to);
                            killed_model = GetFigureFromBoard(to);
                            MoveChessItem(from, to);
                            return true;
                        }
                    }
                }
                //If selected board item doesn't have figure
                else
                {
                    //If it's pawn, check for capture on the isle
                    if (GetFigureFromBoard(from).Type == (byte)ClassicChessItemType.pawn)
                        if (System.Math.Abs(from.vertical - to.vertical) == 2)
                        {
                            byte hor = 0;
                            byte vert = 0;
                            switch (GetFigureFromBoard(from).Side)
                            {
                                case ChessSide.white:
                                    hor = from.horizontal;
                                    vert = (byte)(from.vertical + 1);
                                    break;
                                case ChessSide.black:
                                    hor = from.horizontal;
                                    vert = (byte)(from.vertical - 1);
                                    break;
                                case ChessSide.red:
                                    hor = (byte)(from.horizontal + 1);
                                    vert = from.vertical;
                                    break;
                                case ChessSide.green:
                                    hor = (byte)(from.horizontal - 1);
                                    vert = from.vertical;
                                    break;
                                default:
                                    break;
                            }

                            captureOnTheIsle = new BoardPosition(hor, vert);
                        }

                    var temp_chess_item = MoveChessItemPredictForward(from, to);
                    if (CheckForCheck(curr_side))
                    {
                        MoveChessItemPredictBackward(temp_chess_item, from, to);
                        return false;
                    }
                    else
                    {
                        MoveChessItemPredictBackward(temp_chess_item, from, to);
                        MoveChessItem(from, to);

                        var side = GetFigureFromBoard(to).Side;
                        bool isMomentToMakeQueenFromPawn = false;
                        if (side == ChessSide.white && to.vertical == Max_Y - 1)
                            isMomentToMakeQueenFromPawn = true;
                        else if (side == ChessSide.black && to.vertical == 0)
                            isMomentToMakeQueenFromPawn = true;
                        else if (side == ChessSide.red && to.horizontal == Max_X - 1)
                            isMomentToMakeQueenFromPawn = true;
                        else if (side == ChessSide.green && to.horizontal == 0)
                            isMomentToMakeQueenFromPawn = true;

                        if (isMomentToMakeQueenFromPawn)
                            if (GetFigureFromBoard(to).Type == (byte)ClassicChessItemType.pawn)
                            {
                                var ci = GetFigureFromBoard(to);
                                SetFigureOnBoard(to, new ClassicQueenModel(ci.Side, to));
                            }

                        return true;
                    }
                }
            }
        }

        public void MoveChessItem(BoardPosition from, BoardPosition to)
        {
            SetFigureOnBoard(to, GetFigureFromBoard(from));
            if (to != from)
                SetFigureOnBoard(from, new NullChessItemModel());
        }

        public IChessItemModel MoveChessItemPredictForward(BoardPosition from, BoardPosition to)
        {
            var temp = GetFigureFromBoard(to);
            SetFigureOnBoard(to, GetFigureFromBoard(from));
            SetFigureOnBoard(from, new NullChessItemModel());
            return temp;
        }

        public void MoveChessItemPredictBackward(IChessItemModel temp, BoardPosition from, BoardPosition to)
        {
            SetFigureOnBoard(from, GetFigureFromBoard(to));
            GetFigureFromBoard(from).Steps -= 2;
            SetFigureOnBoard(to, temp);
        }

        public abstract bool CheckForCheck(ChessSide side);
        protected bool CheckForCheck(ChessSide side, byte king_index)
        {
            IChessItemModel cm = new NullChessItemModel();
            for (int i = 0; i < FiguresPlacement.GetLength(0); i++)
                for (int j = 0; j < FiguresPlacement.GetLength(1); j++)
                {
                    var cm_ij = FiguresPlacement[i, j];
                    if (!cm_ij.IsNullObject && cm_ij.Type == king_index && cm_ij.Side == side)
                    {
                        cm = cm_ij;
                        break;
                    }
                }
            if (cm.IsNullObject)
                return false;


            foreach (var cm_ij in FiguresPlacement)
            {
                if (!cm_ij.IsNullObject && cm_ij != cm && cm_ij.Side != side)
                {
                    List<bool> isJumpTemp;
                    List<bool> killPossibilityList;

                    var poss_moves = cm_ij.GetPossibleMoves(FiguresPlacement, out isJumpTemp, out killPossibilityList);
                    int k = -1;
                    foreach (var poss_move in poss_moves)
                    {
                        k++;
                        if (poss_move == cm.Pos && killPossibilityList[k])
                            return true;
                    }
                }
            }

            return false;
        }


        public abstract bool CheckForMate(ChessSide side);
        public virtual bool CheckForMate(ChessSide side, byte king_index)
        {
            IChessItemModel kingModel = new NullChessItemModel();
            for (int i = 0; i < FiguresPlacement.GetLength(0); i++)
                for (int j = 0; j < FiguresPlacement.GetLength(1); j++)
                {
                    var cm_ij = FiguresPlacement[i, j];
                    if (!cm_ij.IsNullObject && cm_ij.Type == king_index && cm_ij.Side == side)
                    {
                        kingModel = cm_ij;
                        break;
                    }
                }

            if (kingModel.IsNullObject)
                throw new Exception(side.ToString() + " king is missing!");

            foreach (var cm_ij in FiguresPlacement)
            {
                if (!cm_ij.IsNullObject && cm_ij.Side == side)
                {
                    List<bool> isJumpTemp;
                    List<bool> killPossibilityList;
                    foreach (var poss_move in cm_ij.GetPossibleMoves(FiguresPlacement, out isJumpTemp, out killPossibilityList))
                    {
                        var prev_pos = cm_ij.Pos;
                        var temp_chess_item = MoveChessItemPredictForward(cm_ij.Pos, poss_move);
                        bool isCheck = CheckForCheck(side);
                        MoveChessItemPredictBackward(temp_chess_item, prev_pos, cm_ij.Pos);

                        if (!isCheck && poss_move != prev_pos)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
