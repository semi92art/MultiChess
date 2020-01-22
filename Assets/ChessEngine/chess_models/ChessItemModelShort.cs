using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public struct ChessItemModelShort : IChessItemModelShort
    {
        public ChessSide Side { get; private set; }
        public BoardPosition Pos { get; private set; }
        public byte Type { get; private set; }
        public int Steps { get; set; }
        public bool IsNullObject { get; private set; }

        public ChessItemModelShort(ChessSide Side, BoardPosition Pos, byte Type, int Steps, bool isNullObject)
        {
            this.Side = Side;
            this.Pos = Pos;
            this.Type = Type;
            this.Steps = Steps;
            this.IsNullObject = isNullObject;
        }
    }
}
