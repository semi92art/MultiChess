using System;

namespace ChessEngine
{
    public interface ICheckMate
    {
        event EventHandler<ChessSideArgs> OnCheck;
        event EventHandler<ChessSideArgs> OnMate;
    }
}
