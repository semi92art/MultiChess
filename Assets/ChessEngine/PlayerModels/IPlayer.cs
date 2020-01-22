using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public interface IPlayer
    {
        PlayerType Type { get; }
        ChessSide Side { get; }

        void DoAction(BoardPosition bp, bool back = false);

        IFigureOnBoard FigureOnBoard { get; }
        IFEN FEN { get; }
        IChessMatchCurrentState ChessMatchCurrentState { get; }
        IMessaging Messaging { get; }
        void InitInterfaces(IMessaging messaging, IFigureOnBoard figureOnBoard, IFEN fen, IChessMatchCurrentState chessMatchCurrentState);
    }
}
