
using System.Collections.Generic;

namespace ChessEngine
{
    public class NullChessItemModelShort : IChessItemModelShort
    {
        public ChessSide Side { get; private set; }
        public BoardPosition Pos { get; private set; }
        public byte Type { get; private set; }
        public int Steps { get; set; }
        public bool IsNullObject { get; private set; }

        public NullChessItemModelShort()
        {
            Side = ChessSide.white;
            Pos = BoardPosition.None;
            Type = 0;
            Steps = 0;
            IsNullObject = true;
        }
    }
}

