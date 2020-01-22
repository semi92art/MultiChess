using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public interface IChessAction
    {
        event EventHandler<ChessSideArgs> OnChanged;
        void MakeAction(BoardPosition pos);
        PlayerAction LastPlayerAction { get; }
    }
}
