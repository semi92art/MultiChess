using System;
using System.Collections.Generic;

namespace ChessEngine
{
    /// <summary>
    /// BOARD KIT (BOARDS AND FIGURES)
    /// DATA: 
    /// information about board type, 
    /// board size, all figures, placement of figures on board
    /// FUNCTIONS:
    /// get info about figure on board by position,
    /// move chess item,
    /// convert pawn to other,
    /// castling
    /// </summary>
    public interface IChessBoardKit
    {

        /// <summary>
        /// placement of figures on board (board items with no figures are nulls)
        /// </summary>
        IChessItemModel[,] FiguresPlacement { get; }
        /// <summary>
        /// horizontal size of board in items
        /// </summary>
        byte Max_X { get; }
        /// <summary>
        /// vertical size of board in items
        /// </summary>
        byte Max_Y { get; }
        /// <summary>
        /// Get chess item by it's position
        /// </summary>
        /// <param name="pos">position of figure on board</param>
        /// <returns></returns>
        IChessItemModel GetFigureFromBoard(BoardPosition pos);
        /// <summary>
        /// Set chess item new postion
        /// </summary>
        /// <param name="pos_old">old position of item</param>
        /// <param name="pos_new">new position of item</param>
        bool TryMoveChessItem(ChessSide curr_side, BoardPosition from, BoardPosition to, out BoardPosition captureOnTheIsle, out IChessItemModel killed_item);
        /// <summary>
        /// try move chess item from one board position to another
        /// </summary>
        /// <param name="from">from this position</param>
        /// <param name="to">to this position</param>
        /// <param name="backInTime">is it a step back in</param>
        void MoveChessItem(BoardPosition from, BoardPosition to);
        /// <summary>
        /// Check board for check
        /// </summary>
        /// <param name="side">checking side</param>
        /// <returns></returns>
        bool CheckForCheck(ChessSide side);
        /// <summary>
        /// Check board for mate
        /// </summary>
        /// <param name="side">checking side</param>
        /// <returns></returns>
        bool CheckForMate(ChessSide side);
        /// <summary>
        /// Move chess for prediction. Returns item on position "to" before move. After using this function neccesarily use MoveChessItemPredictBackward.
        /// </summary>
        /// <param name="from">move from this position</param>
        /// <param name="to">move to this position</param>
        /// <returns></returns>
        IChessItemModel MoveChessItemPredictForward(BoardPosition from, BoardPosition to);
        /// <summary>
        /// Move chess back after using MoveChessItemPredictForward.
        /// </summary>
        /// <param name="temp">result of funtion MoveChessItemPredictForward</param>
        /// <param name="from">move from this positoin</param>
        /// <param name="to">move to this position</param>
        void MoveChessItemPredictBackward(IChessItemModel temp, BoardPosition from, BoardPosition to);

        void SetFigureOnBoard(BoardPosition pos, IChessItemModel chessItem);
    }
}
