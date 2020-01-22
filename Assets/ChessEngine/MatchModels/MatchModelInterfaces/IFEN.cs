using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public interface IFEN
    {
        string GetFEN();
        bool WasCaptureOnTheIsle { get; }
        BoardPosition CaptureOnTheIsle { get; }
        int NumberOfReversibleSemiSteps { get; }
        int TotalSteps { get; }
        Castling WhiteCastling { get; }
        Castling BlackCastling { get; }
    }
}
