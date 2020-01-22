using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public interface IChessItemModelShort
    {
        ChessSide Side { get; }
        BoardPosition Pos { get; }
        byte Type { get; }
        int Steps { get; set; }
        bool IsNullObject { get; }
        
    }
}
