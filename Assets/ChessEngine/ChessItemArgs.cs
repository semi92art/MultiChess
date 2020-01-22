using System;


namespace ChessEngine
{
    public class ChessItemArgs : EventArgs
    {
        public IChessItemModel ChessItem { get; private set; }

        public ChessItemArgs(IChessItemModel chessItem)
        {
            ChessItem = chessItem;
        }
    }
}
