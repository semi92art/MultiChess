using System;

namespace ChessEngine
{
    public class ChessSideArgs : EventArgs
    {
        public ChessSide Side { get; private set; }
        public ChessSideArgs(ChessSide side)
        {
            Side = side;
        }
    }
}
